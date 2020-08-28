using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoddyFollow : MonoBehaviour
{
    public CharacterController controller;

    // Current position of the player body
    private Vector3 worldPos;
    [SerializeField]
    private Camera camera;

    private void Update()
    {
        worldPos = transform.parent.position + controller.center;
        //this.transform.position = worldPos;
        this.transform.position = Vector3.MoveTowards(this.transform.position, worldPos, 10 * Time.deltaTime);
        this.transform.eulerAngles = new Vector3(this.transform.rotation.x, camera.transform.eulerAngles.y, this.transform.rotation.z);

    }

}
