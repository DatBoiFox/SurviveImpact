using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 3;
    public float turnSpeed = 5;
    public float height = 0.6f;
    public float heightPadding = 0.05f;

    public bool Follow;

    public GameObject camera;

    public LayerMask ground;

    public float maxGroundAngle = 120;
    public bool debug;

    public float gravity;

    public float smth;

    Vector2 input;
    public float groundAngle;

    Vector3 forward;
    RaycastHit hitInfo;
    public bool grounded;

    GameObject Boddy;


    private void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<Rigidbody>().AddForce(this.transform.up * 3, ForceMode.Impulse);
        }
        if (grounded)
        {
            if(Follow)
                this.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
            else
                this.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        //Debug.Log(this.transform.parent.GetComponent<Rigidbody>().velocity);
        if (groundAngle >= maxGroundAngle)
        {

        }else
        {
            float step = movementSpeed * Time.deltaTime;
            
            if (Follow)
            {
                //transform.position = Vector3.MoveTowards(transform.position, new Vector3(camera.transform.position.x, transform.position.y, camera.transform.position.z), step);
                transform.position = new Vector3(camera.transform.position.x, transform.position.y, camera.transform.position.z);
                transform.eulerAngles = new Vector3(0, camera.transform.rotation.eulerAngles.y, 0);
            }
            else
            {
                transform.position += (forward * Time.deltaTime * movementSpeed) * input.y;
            }
            
        }
        //
        //if(Follow)
        //    transform.eulerAngles = camera.transform.eulerAngles;
        //else
        //    transform.eulerAngles += (this.transform.up * Time.deltaTime * turnSpeed) * input.x;
        CalculateForward();
        CalculateGroundAngle();
        CheckGround();
        ApplyGravity();
        DrawDebugLines();
    }

    void CalculateForward()
    {
        if (!grounded)
        {
            forward = transform.forward;
            return;
        }

        forward = Vector3.Cross(hitInfo.normal, -transform.right);

    }

    void CalculateGroundAngle()
    {
        if (!grounded)
        {
            groundAngle = 90;
            return;
        }

        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
    }

    void CheckGround()
    {
        if (Follow)
        {
            if (Physics.Raycast(transform.transform.position, -Vector3.up, out hitInfo, height + heightPadding, ground))
            {
                Debug.Log("AAA");
                if (Vector3.Distance(transform.position, hitInfo.point) < height)
                {
                    transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, smth * Time.deltaTime);
                }
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }
        else
        {
            if (Physics.Raycast(transform.transform.position, -Vector3.up, out hitInfo, height + heightPadding, ground))
            {
                Debug.Log("AAA");
                if (Vector3.Distance(transform.position, hitInfo.point) < height)
                {
                    transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, smth * Time.deltaTime);
                }
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }
    }

    void ApplyGravity()
    {
        if (!grounded)
        {
            transform.position += -Vector3.up * gravity * Time.deltaTime;
        }
    }

    void DrawDebugLines()
    {
        if (!debug)
            return;


         Debug.DrawLine(transform.position, (transform.position + forward * height * 2), Color.blue);
         Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);

    }

}
