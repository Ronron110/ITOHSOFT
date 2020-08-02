using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace AppSystem
{
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
        /// <summary>
        /// 状態リスト
        /// </summary>
        private Dictionary<string, StateInstance> states = null;

        /// <summary>
        /// スタートアップステート
        /// </summary>
        private string startUpState = null;

        /// <summary>
        /// 次のステート
        /// </summary>
        private string nextState = null;
        
        /// <summary>
        /// 現在のステート
        /// </summary>
        private StateInstance currentState = null;

        /// <summary>
        /// ステート遷移進入処理
        /// </summary>
        public override void Enter()
        {
            this.RequestChangeState(this.startUpState);
        }

        /// <summary>
        /// ステート内部処理
        /// </summary>
        public override void Update()
        {
            // ステート処理実行
            this.currentState?.Update();

            // 次のステートへの変更があったら
            if (this.nextState != null)
            {
                // 現在のステートに退出処理を実行
                this.currentState?.Leave();
                // 次のステートに変更
                StateInstance next = this.states[this.nextState];
                // 次のステートに変更するのでステート進入処理を実行
                next.Enter();
                // 現在のステートを切り替え
                this.currentState = next;
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
            if(this.states.ContainsKey(state) == false)
            {
                Debug.LogError(string.Format("State `{0}` not found", state));
                return;
            }
            if (this.nextState != null)
            {
                Debug.LogWarning(string.Format("The reserved next state {0} is discarded", this.nextState));
            }
            this.nextState = state;
        }

        /// <summary>ステートリストをセットする</summary>
        /// <param name="s">ステートリスト</param>
        protected void SetStates(Dictionary<string, StateInstance> s)
        {
            this.states = s;
        }

        /// <summary>コンストラクタ</summary>
        /// <param name="l">ステートのリスト</param>
        /// <param name="s">スタートアップステート</param>
        public Transition(string s) : base("System", "Transition")
        {
            this.startUpState = s;
        }
    }
}
