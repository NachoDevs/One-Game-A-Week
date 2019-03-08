using UnityEngine;

public class Target : MonoBehaviour
{
    public void TakeDamage()
    {
        Destroy(gameObject, 10f);        
    }


}
