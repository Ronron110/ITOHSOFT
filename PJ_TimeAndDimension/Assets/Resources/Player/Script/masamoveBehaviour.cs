//#define SHOW_DEBUG_RAYS
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
    public CapsuleCollider cupsuleCollider;                //プレイヤーのRigidbody
    private bool isRun = false;           //走り中スイッチ
    private bool isDive = false;           //ダイブ中スイッチ
    private bool slowSwitch=false;      //スローモーションスイッチ
    public float gravity = -0.98f;      //プレイヤーの重力
    public float playerSpeed = 0.6f;      //プレイヤーの移動速度
    public float playerTimescale = 1f;    //プレイヤーの時間の進み方
    private const float kRayMagnification = 5.0f;
    private const float kRayHeight = 0.03f;
    private Vector3 forceTobeadd = Vector3.zero; //
    private Rigidbody rb;


    void Start()
    {
        // Animatorコンポーネントのインスタンス解決
        anim = GetComponent<Animator>();
        // CapsuleColliderコンポーネントのインスタンス解決　ジャンプしたときにコライダーの形を変えたい
        cupsuleCollider = GetComponent<CapsuleCollider>();
        //Rigidbody コンポーネントのインスタンス解決
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
    private void FixedUpdate()
    {
        //   rb.AddForce(forceTobeadd * playerTimescale);
        rb.AddForce(transform.forward*5.0f* playerTimescale);
        forceTobeadd = Vector3.zero;

    }
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
        if (Input.GetKey(KeyCode.Return) && slowSwitch == false)
        {
            //if (Time.fixedTime>4 && slowSwitch == false)
            //{
                slowSwitch =true;                //スローモーション状態をTrueに
                anim.speed=1;                  //アニメーションの再生スピードはノーマルと同じ
                Time.fixedDeltaTime=0.0002f;    //当たり判定を100倍の頻度で判定
                Time.timeScale=0.1f;            //世界のタイムスケールを10分の1に
                playerTimescale = 10f;           //プレイヤーのタイムスケールを10べぇ

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
            playerTimescale = 10f;           //プレイヤーのタイムスケールを10べぇ
            gravity = -0.981f;               //プレイヤの重力をリセット
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
                playerSpeed = 1.2f;
            }
            else
            {
                //Walk状態へ
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                playerSpeed = 0.6f;
            }
        }
        else
        {
            //Idle状態へ
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            playerSpeed = 0.0f;
        }



        //左回転
        if (Input.GetKey(KeyCode.A))
        {
            // x軸を軸にして毎フレーム-2度、回転させるQuaternionを作成（変数をrotとする）
            Quaternion rot = Quaternion.AngleAxis(-1f, Vector3.up);
            // 現在の自信の回転の情報を取得する。
            Quaternion q = this.transform.rotation;
            // 合成して、自身に設定
            this.transform.rotation = q * rot;
        } 
        //右回転
        if (Input.GetKey(KeyCode.D))
        {
            // x軸を軸にして毎フレーム2度、回転させるQuaternionを作成（変数をrotとする）
            Quaternion rot = Quaternion.AngleAxis(1f, Vector3.up);
            // 現在の自信の回転の情報を取得する。
            Quaternion q = this.transform.rotation;
            // 合成して、自身に設定
            this.transform.rotation = q * rot;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            //Dive状態へ
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.SetBool("Dive", true);
        }
        
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0); //Diveアニメーションの再生状態を取得
        if (state.IsName("Dive")) //Dive
        {
            anim.SetBool("Dive", false);
        }

        this.UpdatePosition(this.transform.forward, playerSpeed * Time.deltaTime * playerTimescale);
    }

    private void Dive()
    {
        forceTobeadd = transform.up*5.0f;
        cupsuleCollider.direction = 2;
        cupsuleCollider.radius = 0.15f;
        cupsuleCollider.height = 1.25f;

        Vector3 center = cupsuleCollider.center;
        center.y = 1.06f;
        cupsuleCollider.center = center;
    }
    private void DiveFinish()
    {
        cupsuleCollider.direction = 1;
        cupsuleCollider.radius = 0.15f;
        cupsuleCollider.height = 1.77f;

        Vector3 center = cupsuleCollider.center;
        center.y = 0.84f;
        cupsuleCollider.center = center;

    }


    /// <summary>
    /// プレイヤー座標の更新
    /// </summary>
    /// <remarks>
    /// 移動方向にレイを飛ばしてヒットしたらヒットしない方向に滑らせる
    /// </remarks>
    private void UpdatePosition(Vector3 dir, float speed)
    {
        // レイキャスト
        Vector3 org = this.transform.position;
        org.y += kRayHeight;
        Ray ray = new Ray(org, dir);
        RaycastHit hit;
        float len = speed * 10.0f;
#if SHOW_DEBUG_RAYS
        Debug.DrawRay(ray.origin, ray.direction * len, Color.yellow, 1.0f);
#endif
        // ヒットするなら
        if (Physics.Raycast(ray, out hit, len))
        {
            // 補正ベクトルの算出
            Vector3 y = Vector3.Cross(dir, hit.normal);

#if SHOW_DEBUG_RAYS
            Debug.DrawRay(hit.point, hit.normal, Color.blue, 1.0f);
            Debug.DrawRay(hit.point, y, Color.green, 1.0f);
#endif

            Vector3 correction = Vector3.Cross(hit.normal, y);

#if SHOW_DEBUG_RAYS
            Debug.DrawRay(ray.origin, correction * len, Color.red, 1.0f);
#endif

            // 移動速度は補正ベクトルとの内積
            float bias = Vector3.Dot(dir, correction);

            dir = correction;
            speed *= bias;
        }

        this.forceTobeadd += dir * speed;
//        this.transform.position += dir * speed;
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