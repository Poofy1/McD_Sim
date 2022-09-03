using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject camera;
    public float slowMove;
    public float fastMove;
    public float zoomSize;

    private float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = fastMove;
        }
        else
        {
            moveSpeed = slowMove;
        }

        if (Input.GetKey(KeyCode.A))
        {
            camera.GetComponent<Rigidbody2D>().AddForce(new Vector2(-moveSpeed,0), ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.D))
        {
            camera.GetComponent<Rigidbody2D>().AddForce(new Vector2(moveSpeed, 0), ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.W))
        {
            camera.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, moveSpeed), ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.S))
        {
            camera.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -moveSpeed), ForceMode2D.Force);
        }


        if(Input.GetAxis("Mouse ScrollWheel") > 0 && zoomSize > .5f)
        {
            zoomSize -= .2f;
            fastMove *= .95f;
            slowMove *= .95f;
            GetComponent<Camera>().orthographicSize = zoomSize;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && zoomSize < 15)
        {
            zoomSize += .2f;
            fastMove *= 1.05f;
            slowMove *= 1.05f;
            GetComponent<Camera>().orthographicSize = zoomSize;
        }

    }


}
