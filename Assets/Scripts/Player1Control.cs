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
    //public Light nightLight;

    GameObject[] cameraList;

    int currentCamera = 0;
    

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

            RenderSettings.ambientIntensity = 0.35f;

            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentCamera++;
                if(currentCamera >= cameraList.Length)
                {
                    currentCamera = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentCamera--;
                if (currentCamera <= 0)
                {
                    currentCamera = cameraList.Length - 1;
                }
            }

            if (cameraList.Length > 0)
            {
                transform.position = cameraList[currentCamera].transform.position;
                transform.LookAt(cameraList[currentCamera].transform.GetChild(0),Vector3.up);
                cam.localRotation = Quaternion.Euler(0,0,0);
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
