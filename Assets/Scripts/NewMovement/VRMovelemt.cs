using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class VRMovelemt : MonoBehaviour
{
    // Gravity that affects player (forces player to stay on the ground)
    public float Gravity = 62.0f;

    // Max movement speed
    public float MaxSpeed = 1.0f;
    public AudioSource audio;


    public SteamVR_Action_Boolean m_MovePress = null;
    public SteamVR_Action_Vector2 m_MoveValue = null;
    public SteamVR_Input_Sources source1;

    private CharacterController CharacterController = null;
    private Transform CameraRig = null;
    private Transform Head = null;


    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        CameraRig = SteamVR_Render.Top().origin;
        Head = SteamVR_Render.Top().head;
    }

    void Update()
    {
        //HandleHead();
        HandleHight();
        CalculateMovement();

        if (m_MovePress.GetLastStateDown(source1))
        {
            audio.Play();
        }

    }

    private void HandleHead()
    {
        // Get values
        Vector3 oldPossition = CameraRig.position;
        Quaternion oldRotation = CameraRig.rotation;

        // Rotation
        transform.eulerAngles = new Vector3(0.0f, Head.rotation.eulerAngles.y, 0.0f);

        // Restore
        CameraRig.position = oldPossition;
        CameraRig.rotation = oldRotation;

    }

    private void CalculateMovement()
    {
        Vector3 direction = Head.transform.TransformDirection(new Vector3(m_MoveValue.GetAxis(source1).x, 0, m_MoveValue.GetAxis(source1).y));

        if(direction != Vector3.zero)
        {
            audio.pitch = direction.magnitude + 1f;
            if (!audio.isPlaying)
                audio.Play();
        }
        else
        {
            audio.Stop();
        }

        direction.y -= Gravity * Time.deltaTime;

        CharacterController.Move(new Vector3((direction.x * MaxSpeed), direction.y, (direction.z * MaxSpeed)) * Time.deltaTime);
    }

    private void HandleHight()
    {
        float headHeight = Mathf.Clamp(Head.localPosition.y, 1, 2);
        CharacterController.height = headHeight;

        Vector3 newCenter = Vector3.zero;

        newCenter.y = CharacterController.height / 2;
        newCenter.y += CharacterController.skinWidth;

        // MoveCapsule in space
        newCenter.x = Head.localPosition.x;
        newCenter.z = Head.localPosition.z;

        newCenter = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * newCenter;

        // Apply

        CharacterController.center = newCenter;

    }

    float pushPower = 2.0f;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Apply the push
        body.velocity = pushDir * pushPower;
    }

}
