using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各ステートが呼び出せるインターフェイス
/// </summary>
public interface ITransition
{
    /// <see cref="Transition.RequestChangeState(string)"/>
    void RequestChangeState(string state);
}


/// <summary>
/// ゲーム状態を遷移するオブジェクト
/// </summary>
public class Transition : StateInstance, ITransition
{
    #region StateInstances
    /// <summary>ゲーム状態</summary>
    /// <TODO>partial 化する</TODO>
    private Dictionary<string, StateInstance> gameStates = null;

    /// <summary>スタートアップステート</summary>
    private const string kStartupState = "Title";
    #endregion

    /// <summary>次のステート</summary>
    private KeyValuePair<string, StateInstance>? nextState = null;
    /// <summary>現在のステート</summary>
    private KeyValuePair<string, StateInstance>? currentState = null;

    /// <summary>
    /// ステート遷移進入処理
    /// </summary>
    public override void Enter()
    {
        this.RequestChangeState(kStartupState);
    }

    /// <summary>
    /// ステート内部処理
    /// </summary>
    public override void Update()
    {
        // ステート処理実行
        this.currentState?.Value.Update();

        // 次のステートへの変更があったら
        if(this.nextState != null)
        {
            // 現在のステートに退出処理を実行
            this.currentState?.Value.Leave();
            // 次のステートに変更するのでステート進入処理を実行
            this.nextState.Value.Value.Enter(); // 手前の Value は nullable の Value
            // 現在のステートを切り替え
            this.currentState = this.nextState;
            // 変更要求はクリアしておく
            this.nextState = null;
        }
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Leave()
    {

    }

    /// <summary>
    /// ステートの変更要求
    /// </summary>
    /// <param name="state">ステート名</param>
    /// <remarks>
    /// 変更要求がダブった場合後から要求したものが採用される
    /// </remarks>
    public void RequestChangeState(string state)
    {
        if(this.nextState != null)
        {
            Debug.LogWarning("The reserved next state " + this.nextState.Value.Key + "is discarded");
        }
        this.nextState = new KeyValuePair<string, StateInstance>(state, this.gameStates[state]);
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Transition() : base("System", "Transition")
    {
        this.gameStates = new Dictionary<string, StateInstance>()
        {
            { "Title", new Title(this) },
            { "Stage1", new Stage1(this) }

        };

    }
}

public class GameState : StateInstance
{
    /// <summary>状態遷移インスタンス</summary>
    private ITransition transition = null;

    /// <summary>導入処理</summary>
    public override void Enter()
    {
    }

    /// <summary>アップデート処理</summary>
    public override void Update()
    {

        /// <TODO>ステージノクリア判定</TODO>
    }

    /// <summary>終了処理</summary>
    public override void Leave()
    {

    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <seealso cref="StateInstance"/>
    public GameState(ITransition i, string c, string n) : base(c, n)
    {
        this.transition = i;
    }

    /// <summary>
    /// ステート切り替えを要求する
    /// </summary>
    /// <param name="state">変更するステート</param>
    protected void ChangeState(string state)
    {
        this.transition.RequestChangeState(state);
    }
}


