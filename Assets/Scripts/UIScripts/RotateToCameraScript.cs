using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCameraScript : MonoBehaviour
{
  
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
