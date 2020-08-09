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
//        private RaycastHit hit;//Raycastの当たり判定で使う。
//        private float maxDistance = 0.5f;//Rayの長さを設定
        
    }




void OnCollisionStay(Collision collision) 
    {
        if (collision.collider.name == "Player")
        {
            //collapseFloor.OnDamaged(1);
            Debug.Log(collision.collider.name);
        }
    }
}