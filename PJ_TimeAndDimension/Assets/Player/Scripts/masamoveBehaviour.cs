using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

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
        this.playerInput = LisntenFromKey;

    }

    /// <summary>
    /// 入力メッセージたち
    /// </summary>
    public const uint kRequestRunning = 0x08;
    public const uint kRotateRight = 0x04;
    public const uint kRotateLeft = 0x02;
    public const uint kForward = 0x01;
    public const uint kNone = 0x00;

    /// <summary>
    /// インプットリスナーのデリゲート
    /// </summary>
    /// <param name="instance">インスタンス：いらないかもしれない</param>
    /// <returns>入力メッセージ</returns>
    public delegate uint InputListener(masamoveBehaviour instance);

    /// <summary>入力リスナーデリゲートのインスタンス</summary>
    private InputListener playerInput = null;

    /// <summary>
    /// キー入力から入力メッセージを返す
    /// </summary>
    /// <param name="instance">インスタンス this なので使わない</param>
    /// <returns>入力メッセージ</returns>
    private uint LisntenFromKey(masamoveBehaviour instance)
    {

        return kNone; 
    }

    /// <summary>
    /// パペット用のカウント（例）
    /// </summary>
    private int count = 0;

    /// <summary>
    /// パペット用の入力ソース（例）
    /// </summary>
    /// <param name="instance">インスタンス</param>
    /// <returns>入力メッセージ</returns>
    private uint PuppetDemo(masamoveBehaviour instance)
    {
        uint[] values = new uint[]
        {
            kForward,
            kForward,
            kForward | kRotateLeft,
            kForward | kRotateLeft,
            kForward | kRotateLeft,
            kForward | kRotateLeft,
            kForward | kRotateLeft,
        };
        return values[this.count++];
    }

    void Update()
    {
        uint msg = this.playerInput(this);

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
        if (Input.GetAxis("Vertical") == 1)
        {
            //アニメーションによる移動をせずにTransformで移動
            //transform.position += transform.forward*speed*playerTimeScale* Time.deltaTime;
            if (isRun == true)
            {
                //Run状態へ
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
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
            anim.SetBool("Run", false);
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
