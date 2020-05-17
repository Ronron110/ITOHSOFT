using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleObject: MonoBehaviour
{
    /// <summary>
    /// タイトル画面の正しい角度
    /// </summary>
    private Quaternion titleZero;

    /// <summary>
    /// ステート
    /// </summary>
    private double startTime;

    /// <summary>
    /// チューニングが合う（外れる）アニメーションの時間
    /// </summary>
    private const double kTuningDuration = 1.5;

    /// <summary>
    /// チューニングがずれてからフェードまでの時間
    /// </summary>
    private const double kPreFadeDuration = 1.5;

    /// <summary>
    /// フェードの時間
    /// </summary>
    private const double kFadeDuration = 1.0;

    /// <summary>
    /// チューニングを合わせる動きの軌跡を記録するリスト
    /// </summary>
    private Stack<Quaternion> trailRotation = new Stack<Quaternion>();

    /// <summary>
    /// ステート変数
    /// </summary>
    private int titleState = 1;

    /// <summary>
    /// 最大のステート番号
    /// </summary>
    private const int kTitleStateMax = 5;

    /// <summary>
    /// ステートを一つ進める
    /// </summary>
    /// <returns>true: 最終ステートに到達した / false: 最終ステートに到達していません</returns>
    public void ProceedState()
    {
        if (this.titleState == 1)
        {
            this.startTime = Time.time;
            this.titleState = 2;
        }
    }

    /// <summary>
    /// シーン終了したかどうかのチェック
    /// </summary>
    /// <returns>true: 終了しました / false: 終了していません</returns>
    public bool IsFinished()
    {
        return (titleState >= kTitleStateMax);
    }

    // Start is called before the first frame update
    void Start()
    {
        titleZero = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        switch ( titleState )
        {
            case 1:
                ScatterRotation();
                break;

            case 2:
                //スタートが押されてタイトルが見える状態
                transform.rotation = Quaternion.Lerp(transform.rotation, titleZero, 0.5f);
                this.trailRotation.Push(transform.rotation);
                double elapsedTime = Time.time - this.startTime;
                if (elapsedTime >= (kTuningDuration * Time.timeScale))
                {
                    titleState = 3;
                    this.startTime = Time.time;
                }
                break;

            case 3:
                // 動き出す
                transform.rotation = this.trailRotation.Pop();
                if(this.trailRotation.Count == 0)
                {
                    this.titleState = 4;
                    this.startTime = Time.time;
                }
                break;

            case 4:
                elapsedTime = Time.time - this.startTime;
                if (elapsedTime >= (kPreFadeDuration * Time.timeScale))
                {
                    titleState = 5;
                    this.startTime = Time.time;
                }

                ScatterRotation();
                break;

            case 5:
                // フェード開始
                ScatterRotation();
                break;
        }
    }

    /// <summary>
    /// ロゴの角度をぐるぐる回す
    /// </summary>
    private void ScatterRotation()
    {
        //グルグル回っている状態
        Quaternion rot = Quaternion.AngleAxis(313f * Time.deltaTime, Vector3.up);
        Quaternion q = this.transform.rotation;
        this.transform.rotation = q * rot;
        rot = Quaternion.AngleAxis(211f * Time.deltaTime, Vector3.forward);
        q = this.transform.rotation;
        this.transform.rotation = q * rot;
        rot = Quaternion.AngleAxis(99f * Time.deltaTime, Vector3.right);
        // 現在の自信の回転の情報を取得する。

        q = this.transform.rotation;
        // 合成して、自身に設定
        this.transform.rotation = q * rot;

        Time.timeScale = 100f;
    }
}
