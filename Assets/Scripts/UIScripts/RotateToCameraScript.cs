using UnityEngine;

public class RotateToCameraScript : MonoBehaviour
{  
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
