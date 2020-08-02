using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableBlock : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject playerObj;
    ICollapseFloorTriggers collapseFloor;
    ParticleSystem particle;

    void Start()
    {
        playerObj = GameObject.Find("Player");
        collapseFloor = transform.parent.GetComponent<CollapseFloor>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision collision) 
    {
        if (collision.collider.name == "Player")
        {
            collapseFloor.OnDamaged(1);
        }
    }
    
}
