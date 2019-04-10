using UnityEngine;

public class CanvasAlwaysLookAtCamera : MonoBehaviour
{
    Camera m_cameraRef;

    // Start is called before the first frame update
    void Start()
    {
        m_cameraRef = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + m_cameraRef.transform.rotation * Vector3.forward
                        , m_cameraRef.transform.rotation * Vector3.up);
    }
}
