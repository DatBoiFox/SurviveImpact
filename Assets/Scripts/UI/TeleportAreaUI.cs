using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportAreaUI : MonoBehaviour
{
    public string sceneName;
    public Canvas canvas;
    public Text timerText;
    public float afterTravelTimer;

    private float useTimer;
    [SerializeField]
    private bool timerActive;

    private PlayerObject player;

    private void Start()
    {
        canvas.worldCamera = FindObjectOfType<Pointer>().GetComponent<Camera>();
        useTimer = afterTravelTimer;
        if (player == null)
        {
            player = FindObjectOfType<PlayerObject>();
            player.transform.position = this.transform.position;
        }
    }

    private void Update()
    {
        

        if (useTimer <= 0)
        {
            timerActive = false;
        }

        if(!timerActive)
            timerText.text = "Ready";

        if (timerActive)
        {
            useTimer -= Time.deltaTime;
            useTimer = Mathf.Clamp(useTimer, 0, afterTravelTimer);
            timerText.text = "" + Mathf.Round(useTimer);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canvas.gameObject.active = true;
            other.transform.parent.GetComponent<PlayerObject>().Pointer.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canvas.gameObject.active = false;
            other.transform.parent.GetComponent<PlayerObject>().Pointer.gameObject.SetActive(false);
        }
    }

    public void TravelToTown()
    {
        Debug.Log("Teleporting");
        if (!timerActive)
        {
            try
            {
                if (FindObjectOfType<SaveManager>().prepareForNextScene())
                    Application.LoadLevel(sceneName);
            }
            catch
            {
                Application.LoadLevel(sceneName);
            }
        }
    }

    public void SetTimerState(bool active)
    {
        timerActive = active;
    }
}
