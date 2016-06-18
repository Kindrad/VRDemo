using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class Player1Control : NetworkBehaviour {


    public float mouseSensitivity = 200f;
    public float walkSpeed = 1f;
    public Transform cam;

    CharacterController controller;
    public Light flashLight;

    GameObject[] cameraList;
    

	// Use this for initialization
	void Awake () {

        controller = GetComponent<CharacterController>();

        cameraList = GameObject.FindGameObjectsWithTag( "Camera" );


    }
	
	// Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


        if (!isLocalPlayer)
        {
            cam.GetComponent<Camera>().enabled = false;
        }

        if (isServer && !isLocalPlayer)
        {
            transform.GetComponent<MeshRenderer>().enabled = false;
            flashLight.enabled = false;
        }


        if (isServer && isLocalPlayer)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime, 0, Space.Self);
        }else if(isLocalPlayer)
        {
            flashLight.enabled = false;

            RenderSettings.ambientIntensity = 0.8f;

            if (cameraList.Length > 0)
            {
                transform.position = cameraList[0].transform.position;
                transform.LookAt(cameraList[0].transform.GetChild(0),Vector3.up);
                cam.localRotation = Quaternion.Euler(10,0,0);
            }

        }
    }

	void FixedUpdate () {

        if(isServer)//i think this is correct
        {

            //basic movement
            Vector3 moveVec;

            moveVec.x = Input.GetAxis("Horizontal");
            moveVec.z = Input.GetAxis("Vertical");
            moveVec.y = 0;



            controller.Move(Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0) * moveVec * Time.fixedDeltaTime * walkSpeed);

        }
	}

}
