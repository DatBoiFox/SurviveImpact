using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // instance of itself
    private static GameManager instance;

    // Player object.
    [SerializeField]
    private PlayerObject player;

    // Time
    public DayNightCycle gameTime;
    public int currentDay;

    // Post-Processing
    public PostProcessVolume postProcessing;
    private TextureOverlay bloodOverlay;
    private BloodEffect bloodEffect;

    public AudioSource musicAudio;

    // Death UI
    private GameObject deathUI;
    private Text respawnTimer; 
    private Text deathText;
    private float respawnTime = 10;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        deathUI = GameObject.Find("DeathScreen");
        respawnTimer = deathUI.transform.Find("returnTimer").GetComponent<Text>();
        deathText = deathUI.transform.Find("deathText").GetComponent<Text>();
        //player = FindObjectOfType<PlayerObject>();
    }

    private void FixedUpdate()
    {
        if(player == null)
        {
            player = FindObjectOfType<PlayerObject>();
            //player.transform.position = this.transform.position;
            deathUI = GameObject.Find("DeathScreen");
            respawnTimer = deathUI.transform.Find("returnTimer").GetComponent<Text>();
            deathText = deathUI.transform.Find("deathText").GetComponent<Text>();
        }
        if(gameTime == null)
        {
            gameTime = FindObjectOfType<DayNightCycle>();
            currentDay = gameTime.Day;
        }

        if(currentDay != gameTime.Day)
        {
            currentDay = gameTime.Day;
        }

        ApplyStatusChange(player);
    }

    private void Update()
    {
        if (bloodEffect != null && bloodOverlay != null)
        {
            bloodOverlay.colorCutoutThreshold.value = Mathf.InverseLerp(1, 0, Mathf.Clamp(player.playerStatus.GetHealth() * 0.01f, 0, 1));
            bloodEffect.blend.value = Mathf.InverseLerp(1, 0, Mathf.Clamp(player.playerStatus.GetHealth() * 0.01f, 0, 1));
        }
        else
        {
            postProcessing.profile.TryGetSettings(out bloodOverlay);
            postProcessing.profile.TryGetSettings(out bloodEffect);
        }

        if (!musicAudio.isPlaying)
            musicAudio.Play();

    }

    // Handles player in-game status (dead or alive) and acts on it
    private void ApplyStatusChange(PlayerObject player)
    {
        //if (player.GetStatusSystem().GetHealth() <= 0)
        //    KillPlayer(player);

        if(player.isDead || player.playerStatus.GetHealth() <= 0)
        {
            player.isDead = true;
            respawnTime -= Time.deltaTime;
            respawnTime = Mathf.Clamp(respawnTime, 0, 10);
            player.GetComponent<VRMovelemt>().enabled = false;
            deathUI.active = true;
            deathText.text = "You Are Dead";
            respawnTimer.text = "Returning to menu in " + Mathf.Round(respawnTime) + "s.";
        }

        if(player.isDead && respawnTime <= 0)
        {
            ReturnToMainMenu();
        }

    }

    public void ReturnToMainMenu()
    {
        KillPlayer(player);
        foreach (DontDestroyObject o in FindObjectsOfType<DontDestroyObject>())
            Destroy(o.gameObject);

        FindObjectOfType<SaveManager>().CleanScenes();
        Destroy(FindObjectOfType<SaveManager>().gameObject);
        Destroy(gameTime.gameObject);
        Destroy(FindObjectOfType<VRInputModule>().gameObject);
        Destroy(this.gameObject, 1f);
        SceneManager.LoadScene("Menu");
    }

    private void KillPlayer(PlayerObject player)
    {
        Destroy(player.gameObject);
    }

}
