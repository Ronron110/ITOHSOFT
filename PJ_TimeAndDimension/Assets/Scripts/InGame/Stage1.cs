using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームの状態を監視するオブジェクトの基底
/// </summary>
public class GameObserver : GameState
{
    /// <summary>プレイヤーインスタンス：状態監視用</summary>
    protected masamoveBehaviour player = null;

    /// <TODO>更に子ステート化する</TODO>

    /// <summary>ゲームオーバー状態かどうか</summary>
    private bool isGameOver = false;

    /// <summary>ゲームオーバー待ちカウント</summary>
    private float gameOverCount = 0.0f;

    /// <summary>導入処理</summary>
    public override void Enter()
    {
        this.isGameOver = false;
        this.gameOverCount = 0.0f;
    }

    /// <summary>アップデート処理</summary>
    public override void Update()
    {
        // ゲームオーバーじゃない場合
        if (this.isGameOver == false)
        {
            // プレイヤーのコンポーネントが取得済みの場合
            if (this.player != null)
            {
                // プレイヤーの HP でゲームオーバー判定
                if (this.player.PlayerHP <= 0.0f)
                {
                    this.isGameOver = true;
                    this.gameOverCount = 10.0f;
                }
            }
            // プレイヤーのコンポーネントが未取得の場合
            else
            {
                // 取得を試みる
                this.player = GameObject.Find("Player").GetComponent<masamoveBehaviour>();
            }
        }
        // ゲームオーバー中
        else
        {
            // 死亡演出の表示時間を待つ
            this.gameOverCount -= Time.deltaTime;
            if (this.gameOverCount < 0.0f)
            {
                // タイトルに戻る
                ChangeState("Title");
            }

        }

        // 

    }

    /// <summary>終了処理</summary>
    public override void Leave()
    {

    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="c">カテゴリ</param>
    /// <param name="n">インスタンス名</param>
    public GameObserver(ITransition i, string c, string n) : base(i, c, n) 
    {
    }
}

public class Stage1 : GameObserver
{
    /// <summary>導入処理</summary>
    public override void Enter()
    {
        SceneManager.LoadScene("Scenes/Stage1");
        base.Enter();
    }

    /// <summary>アップデート処理</summary>
    public override void Update()
    {
        base.Update();
    }

    /// <summary>終了処理</summary>
    public override void Leave()
    {
        SceneManager.UnloadSceneAsync("Scenes/Stage1");
    }

    public Stage1(ITransition i) : base(i, "InGame", "Stage1") { }
}
