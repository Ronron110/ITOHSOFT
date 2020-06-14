using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class bricksBehaviour : MonoBehaviour
{

    public AudioClip sound1;
    AudioSource audioSource;
    private bool isBlockFall = false;  //ブロック落下し始めたかどうかのフラグ
    private bool isBlockhit = false;    //プレイヤーにヒットしたかどうかのフラグ（何回も同じブロックでダメージを食らわないための仕掛け）
    private bool isBlockDropped = false; //ブロックが床に落ちたかどうかのフラグ


    private GameObject PlayerObj;
    void Start()
    {
        PlayerObj = GameObject.Find("Player");        
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //経過時間が4秒を過ぎたらブロックを落下させる
        if (Time.fixedTime > 4 && isBlockFall==false)
        {
            this.gameObject.GetComponent<Rigidbody>().isKinematic =false;//物理エンジンを有効
            isBlockFall = true;         //落下ステータスOn

        }


    }

    void OnCollisionEnter(Collision collision)
    {

        //ブロックがまだ床に落ちてない間
        if (isBlockDropped != true)
        {
            //ブロックがプレイヤーに初めてヒットしたか？

            if (collision.collider.name == "Player" && isBlockhit != true)
            {
                isBlockhit = true;      //次からヒットしない
                PlayerObj.GetComponent<masamoveBehaviour>().Damage += 300;  //プレイヤーにダメージを300追加
            }
            //ブロックが床に初めてヒットしたか？
            if (collision.collider.name == "corridor-Floor" && isBlockDropped != true )
            {
                audioSource.PlayOneShot(sound1); //落下音を再生
                isBlockDropped = true; //次からヒットしない
                gameObject.name = gameObject.name + "Dropped"; //床に落ちたブロックとしてオブジェクトの名前を変更

            }

        }

    }

}
