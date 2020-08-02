using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace AppSystem
{
    /// <summary>
    /// ゲーム内の遷移インスタンス
    /// </summary>
    /// <remarks>
    /// システムのフェードを持っていて遷移前後に
    /// フェードイン・アウトを挟むことができる
    /// フェードの呼び出しはユーザーの都合なので
    /// 遷移とは連動させていない
    /// </remarks>
    public class GameState : StateInstance
    {
        /// <summary>
        /// フェード中
        /// </summary>
        private bool stateLock = false;

        /// <summary>
        /// カテゴリ名
        /// </summary>
        protected const string kCategoryName = "GameState";

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
        protected Transition subScene = null;

        /// <summary>
        /// 状態遷移インスタンス
        /// </summary>
        private ITransition transition = null;

        /// <summary>
        /// 導入処理
        /// </summary>
        public override void Enter()
        {
            this.stateLock = false;
        }

        /// <summary>アップデート処理</summary>
        public override void Update()
        {
        }

        /// <summary>
        /// フェードアウトリクエスト
        /// </summary>
        protected void RequestFadeOut(double? duration = null, System.Action finish = null)
        {
            if (!duration.HasValue) duration = this.fadeDuration;
            AppSystem.FadeProvider.Fader[AppSystem.Residents.kFader].FadeOut(duration.Value, null, finish);
        }

        /// <summary>
        /// フェードインリクエスト
        /// </summary>
        protected void RequestFadeIn(double? duration = null, System.Action finish = null)
        {
            if (!duration.HasValue) duration = this.fadeDuration;
            AppSystem.FadeProvider.Fader[AppSystem.Residents.kFader].FadeIn(duration.Value, null, finish);
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
        /// <param name="fade">フェードアウト要求</param>
        protected void ChangeState(string state, double? duration = null)
        {
            if (this.stateLock == true) return;

            // 重複呼び出しをロック
            this.stateLock = true;
            
            // ステート変更処理をラムダ化
            System.Action change = () => { this.transition.RequestChangeState(state); };

            // フェード時間の指定がない場合はデフォルト
            if(duration == null)
            {
                duration = this.fadeDuration;
            }
            // フェード時間指定が 0 以下。つまりフェードキャンセルの場合
            if(duration.Value <= 0.0)
            {
                // ステート変更を即時実行
                change();
            }
            // デフォルトも含めフェードアウト時間が有効
            else
            {
                // 終了時にステート変更する条件でフェードアウトを要求
                this.RequestFadeOut(duration, change);
            }
            
        }
    }

    /// <summary>
    /// スタンダードゲームステート
    /// </summary>
    /// <remarks>
    /// 遷移の Enter で 1.0 秒のフェードインリクエストをする
    /// 遷移の Leave で 1.0 秒のフェードアウトリクエストをする
    /// </remarks>
    public class StandardGameState : GameState
    {
        public override void Enter()
        {
            base.Enter();
        }

        public StandardGameState(ITransition i, string n) : base(i, GameState.kCategoryName, n)
        {
        }
    }
}
