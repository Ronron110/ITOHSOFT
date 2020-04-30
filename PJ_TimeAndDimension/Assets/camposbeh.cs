using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camposbeh : MonoBehaviour
{
    private RaycastHit hit;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
           
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(transform.position, transform.rotation * Vector3.forward, out hit, 1.0f);
        
        Debug.DrawRay(transform.position, transform.rotation * Vector3.forward*1.0f ,Color.red, 1, false);


    }
}
