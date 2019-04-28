using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    float m_cameraTimer;

    Vector3 m_originalCamPos;

    public IEnumerator Shake(float t_duration, float t_magnitude)
    {
        m_cameraTimer = .0f;
        m_originalCamPos = transform.localPosition;

        while (m_cameraTimer < t_duration)
        {
            float x = Random.Range(-1f, 1f) * t_magnitude;
            float y = Random.Range(-1f, 1f) * t_magnitude;

            transform.localPosition = new Vector3(x, y, m_originalCamPos.z);

            m_cameraTimer += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = m_originalCamPos;
    }
}
