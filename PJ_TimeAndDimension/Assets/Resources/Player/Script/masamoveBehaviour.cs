using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements.Experimental;


public class masamoveBehaviour : MonoBehaviour
{

    [SerializeField] public float PlayerHP=300;//プレイヤーの体力
    [SerializeField] public float controllerDeadzone = 0.2f;//コントローラーのアソび

    public float Damage;
    public float slowTimeRemain = 5000;  //スローモーション残り時間
    public Animator anim;               //アニメーター
    public Rigidbody rb;                //プレイヤーのRigidbody
    private bool isRun=false;           //走り中スイッチ
    private bool slowSwitch=false;      //スローモーションスイッチ

    void Start()
    {
        // アニメーターの取得
        anim = GetComponent<Animator>();
        // rigidbodyの取得
        rb = GetComponent<Rigidbody>();

        //時間の初期化
            anim.speed=1f;              //アニメーションの再生スピード
            Time.timeScale=1f;          //世の中の時間の進み方

        //入力の初期値をキーボードにセット（パペットキー入力データと切り替え可能）
        this.playerInput = LisntenFromKey;

        
    }

    /// <summary>
    /// 入力メッセージたちをBitで表現
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
    //キー入力
    void Update()
    {
        //-------------------------この実装ってここまでしかやってないね。先にAND取らなあかんかな？
        uint msg = this.playerInput(this);
        //---------------------------------

        //シフトキーを押しているかどうか？走っているかどうかのフラグセット
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRun = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
        }
        // Spaceキーでスローモーションモードへ突入
        if (Input.GetKey(KeyCode.Space) && slowSwitch == false)
        {
            //if (Time.fixedTime>4 && slowSwitch == false)
            //{
                slowSwitch =true;                //スローモーション状態をTrueに
                anim.speed=1;                  //アニメーションの再生スピードはノーマルと同じ
                Time.fixedDeltaTime=0.0002f;    //当たり判定を100倍の頻度で判定
                Time.timeScale=0.1f;            //世界のタイムスケールを10分の1に

            //} 
        }
        if (slowSwitch == true)
        {
            slowTimeRemain -= 1f;             //スローモーション時間を減らしていく
            if (anim.speed < 10.0f)            //プレイヤーのアニメーションが10になるまで・・(1の理由は世界のタイムスケールが0.01だから)
            {
                //プレイヤーを徐々に動けるようにしていく
                anim.speed += 0.01f;
            }
        }
        if (slowTimeRemain<0){              //スローモーション時間切れ
            Time.fixedDeltaTime=0.02f;      //当たり判定の頻度も元にもどす
            anim.speed=1f;                  //プレイヤーの動きを通常の状態へ
            Time.timeScale=1f;              //世界の時間を元に戻す
            slowSwitch=false;               //スローモーションスイッチをOff
            slowTimeRemain = 5000;          //スローモーション時間をリセット
        }

        //前後移動ロジック
        //
        if (Input.GetAxis("Vertical") >controllerDeadzone) //前進キーを押したら（デッドゾーン以上になったら）
        {
            //走り状態
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
            // x軸を軸にして毎フレーム-2度、回転させるQuaternionを作成（変数をrotとする）
            Quaternion rot = Quaternion.AngleAxis(-2, Vector3.up);
            // 現在の自信の回転の情報を取得する。
            Quaternion q = this.transform.rotation;
            // 合成して、自身に設定
            this.transform.rotation = q * rot;
        } 
        //右回転
        if (Input.GetKey(KeyCode.D))
        {
            // x軸を軸にして毎フレーム2度、回転させるQuaternionを作成（変数をrotとする）
            Quaternion rot = Quaternion.AngleAxis(2, Vector3.up);
            // 現在の自信の回転の情報を取得する。
            Quaternion q = this.transform.rotation;
            // 合成して、自身に設定
            this.transform.rotation = q * rot;
        }


    }

    private void LateUpdate()
    {
        //ダメージ処理
        PlayerHP -= Damage * Time.timeScale;//スローになっていたらダメージも小さくするため

        Damage = 0f;//今回のフレームの積算ダメージをリセット

        //プレイヤーのHPがゼロになったら
        if (PlayerHP <= 0f)
        {
            GameObject ragdoll = (GameObject)Resources.Load("Player/PreFab/PlayerRagdoll");
            Instantiate(ragdoll, this.transform.position, Quaternion.identity);//ラグドールの生成

            this.gameObject.SetActive(false);
            //Destroy(this.gameObject);//プレイヤーキャラの消去
        }
    }


}
