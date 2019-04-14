using UnityEngine;

public class DayNightController : MonoBehaviour
{
    public Light sun;
    public float secondsInFullDay = 120f;

    [Range(0, 1)]
    public float currentTimeOfDay;

    public float timeMultiplier = 1f;

    float m_sunInitialIntensity;

    void Start()
    {
        m_sunInitialIntensity = sun.intensity;
        currentTimeOfDay = .3f;
    }

    void Update()
    {
        UpdateSun();
        currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

        if (currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
        }
    }

    void UpdateSun()
    {
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);

        float intensityMultiplier = 1;
        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)     //Night
        {
            intensityMultiplier = .005f;
            sun.colorTemperature += sun.colorTemperature;
        }
        else if (currentTimeOfDay <= 0.25f)
        {
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));  //Sunrise
        }
        else if (currentTimeOfDay >= 0.73f)
        {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));    //Sunset
        }

        sun.intensity = m_sunInitialIntensity * intensityMultiplier;
    }
}
