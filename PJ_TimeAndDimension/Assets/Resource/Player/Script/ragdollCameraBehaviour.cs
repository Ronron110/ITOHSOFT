using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollCameraBehaviour : MonoBehaviour
{       
    
       void Start()
    {

        Time.timeScale=1f;
        Time.fixedDeltaTime=0.02f;
 
    }

    // Update is called once per frame

    void Update()
    {

        transform.RotateAround(transform.position, Vector3.up, 20f*Time.deltaTime);

    }
}
