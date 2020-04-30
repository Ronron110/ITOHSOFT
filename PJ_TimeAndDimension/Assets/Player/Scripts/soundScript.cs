using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundScript : MonoBehaviour
{
    private bool isHit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.fixedTime>4 )
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(isHit==false)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip);
            isHit = true;

        }

    }
}
