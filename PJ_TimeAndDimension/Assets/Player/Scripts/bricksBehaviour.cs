using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class bricksBehaviour : MonoBehaviour
{

    public AudioClip sound1;
    AudioSource audioSource;
    private bool isBlockFall = false;
    private bool isBlockDropped = false;


    private GameObject PlayerObj;
    void Start()
    {

        PlayerObj = GameObject.Find("Player");
        
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.fixedTime > 4 && isBlockFall==false)
        {

            this.gameObject.GetComponent<Rigidbody>().isKinematic =false;
            isBlockFall = true;

        }


    }

    void OnCollisionEnter(Collision collision)
    {


            Debug.Log(collision.collider.name);


        if (isBlockDropped == false)
        {


            if (collision.collider.name == "Player" && isBlockDropped != true)
            {
                Debug.Log(collision.collider.name + "が" + this.name + "にヒット");
                PlayerObj.GetComponent<masamoveBehaviour>().Damage += 300;
            }

            if (collision.collider.name == "RoomFloor" && isBlockDropped != true )
            {
                Debug.Log(collision.collider.name + "が" + this.name + " "+isBlockDropped) ;
                
                
                audioSource.PlayOneShot(sound1);

                
                isBlockDropped = true;
                gameObject.name = gameObject.name + "Dropped";


            }

        }

    }

}
