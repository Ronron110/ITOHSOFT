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
 
    private bool slowSwitch=false;      //スローモーションスイッチ
    public float gravity = UnityEngine.Physics.gravity.y;      //プレイヤーの重力


    public float runningSpeed=0.0f;
    public float walkingSpeed=0.0f;
    public float playerSpeed = 1.0f;      //プレイヤーの移動速度
    public float playerTimescale = 1f;    //プレイヤーの時間の進み方
    public float playerGravity = 1f;    //プレイヤーの重力
    
    private const float kRotationSpeed = 200.0f;
    private const float kRayMagnification = 10.0f;
    private const float kRayHeight = 0.84f;
    private Vector3 forceToBeAdd = Vector3.zero; // モーション等による外力
    private Vector3 velocity = Vector3.zero; // 移動入力による速度
    private bool uncontrollable = false; // 外力により制御できない状況
    private Rigidbody rigidBody = null;

    private  const float kMoveMagnification = 21.0f;
    private const float kJumpHeigtForce = 2.8f;
    [SerializeField] private float kSlideForwardForce = 4.5f;
    private bool isRun = false;//走り中スイッチ
    private bool isDive = false;//ダイブ中スイッチ
    private bool isDiving=false;
    private bool isSliding=false;
    private bool isFalling=false;//落下中 
    private bool isLand=false;//着地

    void Start()
    {
        // Animatorコンポーネントのインスタンス解決
        anim = GetComponent<Animator>();
        // CapsuleColliderコンポーネントのインスタンス解決　ジャンプしたときにコライダーの形を変えたい
        cupsuleCollider = GetComponent<CapsuleCollider>();
        //Rigidbody コンポーネントのインスタンス解決
        rigidBody = GetComponent<Rigidbody>();
        //フレームレートの固定
        Application.targetFrameRate=60;

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
        // 前のフレームで計算された加算されるべき力を verocity に渡す
        Vector3 v = rigidBody.velocity;
        if (!uncontrollable)
        {
            v.x = velocity.x;
            v.z = velocity.z;
            forceToBeAdd = Vector3.zero;
        }
        else
        {
            v = forceToBeAdd;
        }
        velocity = Vector3.zero;
        rigidBody.velocity = v;
    }

    /// <summary>
    /// フレーム内でかかる力をリセットする
    /// </summary>
    private void ResetForces()
    {
        // 慣性がない設定なので x, z はプレイヤーが意図しない限り動かない
        // したがって、0 でリセット
        forceToBeAdd.x = 0.0f;
        forceToBeAdd.z = 0.0f;

        // y は重力挙動を反映する必要があるので慣性を残し重力加速度を加算
        forceToBeAdd.y += gravity * Time.fixedDeltaTime * playerTimescale;

    }

    /// <summary>
    /// アップデート
    /// </summary>
    void Update()
    {
        //ResetForces();
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
        // Enterキーでスローモーションモードへ突入
        if (Input.GetKey(KeyCode.Return) && slowSwitch == false)
        {
            slowSwitch =true;                //スローモーション状態をTrueに
            anim.speed=1;                  //アニメーションの再生スピードはノーマルと同じ

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
            anim.speed=1f;                  //プレイヤーの動きを通常の状態へ
             slowSwitch=false;               //スローモーションスイッチをOff
            slowTimeRemain = 5000;          //スローモーション時間をリセット
        }

        //前後移動ロジック
        //
        if (Input.GetAxisRaw("Vertical") >controllerDeadzone) //前進キーを押したら（デッドゾーン以上になったら）
        {
            //走り状態
            if (isRun == true)
            {
                //Run状態へ
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
                playerSpeed = runningSpeed;
            }
            else
            {
                //Walk状態へ
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                anim.SetBool("Slide", false);
                playerSpeed = walkingSpeed;
                
            }
        }
        else
        {
            //Idle状態へ
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.SetBool("Slide", false);
            playerSpeed = 0.0f;
        }

        // 回転速度
        float rotateSpeed = kRotationSpeed * Time.deltaTime;

        //回転
        float rotate = Input.GetAxisRaw("Horizontal");
        if (System.Math.Abs(rotate) >controllerDeadzone)
        {
            // x軸を軸にして毎フレーム-2度、回転させるQuaternionを作成（変数をrotとする）
           
            Quaternion rot = Quaternion.AngleAxis(-rotate*-100f*Time.deltaTime*playerTimescale, Vector3.up);

            // 現在の自信の回転の情報を取得する。
            Quaternion q = this.transform.rotation;
            
            // 合成して、自身に設定
            this.transform.rotation = q * rot;
        } 

        //ステータスごとのアニメーションの再生処理

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0); //アニメーションの再生状態を取得

        if (Input.GetKey(KeyCode.Space))
        {
            if (isRun)
            {
                //Slide状態へ
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                //anim.SetBool("Dive", true);//Dive
                anim.SetBool("Slide", true); //Slide
            }
        }
        if (isFalling==true && state.IsName("Fall")==false)
        {
                //fall状態へ
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                anim.SetBool("Dive", false);//Dive
                anim.SetBool("Slide", false); //Slide
                anim.SetBool("Land",false);
                anim.SetBool("Fall", true); 
                
            
        }
        if (isLand==true)
        {
            anim.SetBool("Fall", false);
            anim.SetBool("Land", true);  
            if (state.IsName("Land"))
            {
                anim.SetBool("Land", false);  
                isLand=false;
            }
        }
       


        
        /*
        if (state.IsName("Dive")) //Dive
        {
            anim.SetBool("Dive", false);
        }
        */


        if (state.IsName("Slide")) //Slide
        {
            anim.SetBool("Slide", false);
        }
        
        this.UpdatePosition(this.transform.forward, playerSpeed * Time.fixedDeltaTime * playerTimescale);
        //Debug.Log("isFalling"+isFalling);
    }




   /// <summary>
    /// Slideモーションに入ったときのコールバック
    /// </summary>
    private void SlideStart()
    {
        isSliding = true;
        forceToBeAdd = transform.forward * kSlideForwardForce;
        uncontrollable = true;
        //カプセルコライダーを横向きにする
        cupsuleCollider.direction = 2;
        cupsuleCollider.radius = 0.1f;
        cupsuleCollider.height = 1.25f;

        Vector3 center = cupsuleCollider.center;
        center.y = 0.32f;
        cupsuleCollider.center = center;
    }

    /// <summary>
    /// Slideモーションが終わったときのコールバック
    /// </summary>
    private void SlideFinish()
    {
        isSliding = false;
        uncontrollable = false;
        cupsuleCollider.direction = 1;
        cupsuleCollider.radius = 0.15f;
        cupsuleCollider.height = 1.77f;

        Vector3 center = cupsuleCollider.center;
        center.y = 0.84f;
        cupsuleCollider.center = center;
    }


/*
    /// Diveモーションに入ったときのコールバック
    /// </summary>
    private void DiveStart()
    {
        isDiving = true;
        forceToBeAdd = transform.up * kJumpHeigtForce;
        //カプセルコライダーを横向きにする
        cupsuleCollider.direction = 2;
        cupsuleCollider.radius = 0.15f;
        cupsuleCollider.height = 1.25f;

        Vector3 center = cupsuleCollider.center;
        center.y = 1.06f;
        cupsuleCollider.center = center;
    }

    /// <summary>
    /// 飛び込みモーションが終わったときのコールバック
    /// </summary>
    private void DiveFinish()
    {
        isDiving = false;
        cupsuleCollider.direction = 1;
        cupsuleCollider.radius = 0.15f;
        cupsuleCollider.height = 1.77f;

        Vector3 center = cupsuleCollider.center;
        center.y = 0.84f;
        cupsuleCollider.center = center;
    }
*/

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
        float len = speed * kRayMagnification;
#if SHOW_DEBUG_RAYS
        Debug.DrawRay(ray.origin, ray.direction * len, Color.yellow, 1.0f);
#endif
        if(isDiving == false)
        {
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
        }
        // 水平移動分の計算は終わったのでここで一旦加算
        velocity += dir * speed * kMoveMagnification;

        //落下判定
            //足元にRayを飛ばして床があるかないかを判定する
            ray = new Ray(org, Vector3.down*kRayHeight);
            bool isHit=Physics.Raycast(ray, out hit,kRayHeight*2);
            //Debug.Log("Rayが当たってるかどうか"+isHit);
            Debug.DrawRay(ray.origin, Vector3.down*0.84f, Color.green, 1.0f);
            // レイキャスト
            if (isHit==false)
            {
                isFalling=true;
                
                //Debug.Log("落下中");
            }
            //落下終了判定
            if (isFalling==true && isHit==true)
            {
                isFalling=false;
                isLand=true;
                Debug.Log("着地");
            }

    /*
        // 落下中（Y がマイナス）の場合地面とのコリジョン判定をする
        if (forceToBeAdd.y < 0.0f)
        {
            // 下向きにレイを飛ばす
            ray = new Ray(org+new Vector3(0f,0.84f,0f), Vector3.down);

            // 現在の重力加速度（下向きはマイナスなので絶対値補正）
            len = -this.forceToBeAdd.y;
            Debug.DrawRay(ray.origin, ray, Color.green, 1.0f);

            // レイキャスト
            if (Physics.Raycast(ray, out hit, len))
            {
                // ヒットする場合は地面までの距離が移動値
                this.forceToBeAdd.y = this.transform.position.y - hit.point.y;
            }
        }
    */
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