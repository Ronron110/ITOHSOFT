using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppSystem
{
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
}