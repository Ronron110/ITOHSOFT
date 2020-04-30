using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class masamoveBehaviour : MonoBehaviour
{
    public float speed = 3.5f;
    public float playerTimeScale=1.0f;
    [SerializeField] public int PlayerHP=300;
    public int Damage;

    public Animator anim;
    public Rigidbody rb;
    private bool isRun=false;
    private bool slowSwitch=false;
// スローモーションの持ち時間
    private float slowTimeRemain=10000;

    //GameObject camtarget= GameObject.Find("cameraTarget");
    // Start is called before the first frame update
    void Start()
    {
        // アニメーターの取得
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
            anim.speed=1f;
            Time.timeScale=1f;
            playerTimeScale=1f;


    }

    void Update()
    {
        
        //シフトキーを押しているかどうか？走っているかどうかのフラグセット
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRun = true;
            speed=5.0f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            speed=3.5f;
        }
        // Spaceキーでスローモーションモードへ突入
        if (Input.GetKey(KeyCode.Space) && slowSwitch == false)
        {
            //if (Time.fixedTime>4 && slowSwitch == false)
            //{
                slowSwitch =true;                //スローモーション状態をTrueに
                anim.speed=5;                  //アニメーションの再生スピードを10倍
                Time.fixedDeltaTime=0.0002f;    //当たり判定を100倍の頻度で判定
                Time.timeScale=0.05f;            //世界のタイムスケールを10分の1に
                playerTimeScale=0.01f;            //プレイヤーのタイムスケールを10倍に

            //} 
        }
        if (slowSwitch==true){
            slowTimeRemain-=1f;             //スローモーション時間を減らしていく
        }
        if (slowTimeRemain<0){              //スローモーション時間切れ
            Time.fixedDeltaTime=0.02f;      //以下元の世界の状態へ
            anim.speed=1f;
            Time.timeScale=1f;
            playerTimeScale=1f;
            slowSwitch=false;
        }
    
        //前後移動ロジック
        if (Input.GetAxis("Vertical")==1)
        {
            //アニメーションによる移動をせずにTransformで移動
            //transform.position += transform.forward*speed*playerTimeScale* Time.deltaTime;
            if (isRun==true){ 

                 //Run状態へ
                anim.SetBool("Run", true);
            }
            else
            {
                //Walk状態へ
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
            }
        }
        else
        {
            //Idle状態へ
            anim.SetBool("Walk", false);
        }
        

        //左回転
        if (Input.GetKey(KeyCode.A))
        {
            // x軸を軸にして毎秒-2度、回転させるQuaternionを作成（変数をrotとする）
            Quaternion rot = Quaternion.AngleAxis(-2, Vector3.up);
            // 現在の自信の回転の情報を取得する。
            Quaternion q = this.transform.rotation;
            // 合成して、自身に設定
            this.transform.rotation = q * rot;
        } 
        //右回転
        if (Input.GetKey(KeyCode.D))
        {
            // x軸を軸にして毎秒2度、回転させるQuaternionを作成（変数をrotとする）
            Quaternion rot = Quaternion.AngleAxis(2, Vector3.up);
            // 現在の自信の回転の情報を取得する。
            Quaternion q = this.transform.rotation;
            // 合成して、自身に設定
            this.transform.rotation = q * rot;
        }

        PlayerHP -= Damage;
         
        if (PlayerHP<=0){
            GameObject ragdoll = (GameObject)Resources.Load("PlayerRagdoll");


            Instantiate(ragdoll, this.transform.position, Quaternion.identity);
            
            Destroy(this.gameObject);

        }
    }

    

}
