using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム状態を遷移するオブジェクト
/// </summary>
public class Transition : StateInstance
{
    #region StateInstances
    /// <summary>ゲーム状態</summary>
    /// <TODO>partial 化する</TODO>
    private Dictionary<string, StateInstance> gameStates = new Dictionary<string, StateInstance>()
    {
    };

    /// <summary>スタートアップステート</summary>
    private const string kStartupState = "Splash";
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
        this.nextState = new KeyValuePair<string, StateInstance>(state, gameStates[state]);
    }

    public Transition() : base("System", "Transition")
    {

    }
}

