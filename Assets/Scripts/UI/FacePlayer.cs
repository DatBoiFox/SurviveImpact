using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Camera");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(player.transform);
    }
}
