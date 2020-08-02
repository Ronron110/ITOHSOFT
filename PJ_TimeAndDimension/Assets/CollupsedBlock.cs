using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollupsedBlock : MonoBehaviour
{
    ICollapseFloorTriggers collapseFloor;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 collupsedBlockpos = this.transform.position;
        collupsedBlockpos.y -= 10 * Time.deltaTime*Time.timeScale;
        transform.position = collupsedBlockpos;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name== "corridor3")
        {
            collapseFloor.isDropped();
        }
    }
}
