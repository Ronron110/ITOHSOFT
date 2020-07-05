using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GameObject CrumblingBlock = gameObject.


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Player")
        {

        }
    }
}
