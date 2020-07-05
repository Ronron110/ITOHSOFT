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
        private KeyValuePair<string, StateInstance>? nextState = null;
        
        /// <summary>
        /// 現在のステート
        /// </summary>
        private KeyValuePair<string, StateInstance>? currentState = null;

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
            this.currentState?.Value.Update();

            // 次のステートへの変更があったら
            if (this.nextState != null)
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
            if (this.nextState != null)
            {
                Debug.LogWarning("The reserved next state " + this.nextState.Value.Key + "is discarded");
            }
            this.nextState = new KeyValuePair<string, StateInstance>(state, this.states[state]);
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

    /// <summary>
    /// ゲーム全体の遷移
    /// </summary>
    public class GameTransition : Transition
    {
        /// <summary>
        /// スタートアップステート
        /// </summary>
        private const string kStartupState = "Title";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameTransition() : base(kStartupState)
        {
            Dictionary<string, StateInstance> states = new Dictionary<string, StateInstance>()
            {
                { kStartupState, new Title(this) },
                { "Stage1", new Stage1(this) }
            };
            base.SetStates(states);
        }
    }

    public class GameState : StateInstance
    {
        /// <summary>
        /// デフォルトのフェード時間
        /// </summary>
        private const double kDefaultSystemFadeDuration = 1.0;

        /// <summary>
        /// フェード時間
        /// </summary>
        protected double fadeDuration = kDefaultSystemFadeDuration;

        /// <summary>
        /// タイトル内のサブシーン遷移インスタンス
        /// </summary>
        Transition subScene = null;

        /// <summary>
        /// 状態遷移インスタンス
        /// </summary>
        private ITransition transition = null;

        /// <summary>
        /// 導入処理
        /// </summary>
        public override void Enter()
        {
            FadeProvider.Fader[Residents.kFader].FadeIn(1.0);
        }

        /// <summary>アップデート処理</summary>
        public override void Update()
        {
        }

        /// <summary>
        /// フェードアウトリクエスト
        /// </summary>
        protected void RequestFadeOut()
        {
            if (AppSystem.FadeProvider.Fader[AppSystem.Residents.kFader].IsActive() == false)
            {
                AppSystem.FadeProvider.Fader[AppSystem.Residents.kFader].FadeOut(this.fadeDuration);
            }
        }

        /// <summary>
        /// フェードインリクエスト
        /// </summary>
        protected void RequestFadeIn()
        {
            if (AppSystem.FadeProvider.Fader[AppSystem.Residents.kFader].IsActive() == false)
            {
                AppSystem.FadeProvider.Fader[AppSystem.Residents.kFader].FadeIn(this.fadeDuration);
            }
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
}
