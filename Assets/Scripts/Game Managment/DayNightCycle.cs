using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private static DayNightCycle instance;

    [SerializeField]
    private Light light;
    [SerializeField]
    private Lighting lightingSet;

    // The length of the day scaled up
    [SerializeField]
    private int TimeScale;

    // Current time of the day
    public float DayTime;
    // How fast the time goes.
    public float timeSpeedTemp;

    // Current Day (acts like the day counter)
    public int Day;
    // Boolean to check if the day has ended and new one started.
    private bool newDay;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if(!newDay && DayTime > 60 && DayTime < 65)
        {
            newDay = true;
            Day++;
        }else if (newDay && DayTime > 65)
        {
            newDay = false;
        }

        DayTime += Time.deltaTime * timeSpeedTemp;
        DayTime %= TimeScale*1f;
        DayCycle(DayTime / TimeScale*1f);
    }
    // handles lightning.
    private void DayCycle(float time)
    {
        RenderSettings.ambientLight = lightingSet.AmbientColor.Evaluate(time);
        RenderSettings.fogColor = lightingSet.FogColor.Evaluate(time);

        light.color = lightingSet.DirectionalColor.Evaluate(time);
        this.transform.localRotation = Quaternion.Euler(new Vector3((time * 360f) - 90f, 170f, 0));

    }
}
