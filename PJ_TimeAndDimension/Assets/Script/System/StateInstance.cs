using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppSystem
{
    /// <summary>
    /// ステートインスタンス（抽象クラス）
    /// </summary>
    /// <remarks>
    /// 状態遷移システムの中の一つのインスタンス
    /// Enter - 状態がこのインスタンスに遷移したときに最初に呼ばれる想定
    /// Update - このインスタンスが実行中に毎フレーム呼ばれる想定
    /// Leave - 状態が別のインスタンスに変わるとき、この状態としては最後に呼ばれる想定
    /// 
    /// category と name はこのような相似形のクラスを使う場合
    /// インスタンスの特定が困難になることが多いので、
    /// インスタンスごとに名前を付けられるようにしてある。
    /// </remarks>
    public abstract class StateInstance
    {
        /// <summary>カテゴリ名</summary>
        public string category { get; protected set; }

        /// <summary>インスタンス名</summary>
        public string name { get; protected set; }

        /// <summary>導入処理</summary>
        public abstract void Enter();

        /// <summary>アップデート処理</summary>
        public abstract void Update();

        /// <summary>終了処理</summary>
        public abstract void Leave();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="c">カテゴリ名</param>
        /// <param name="n">名前</param>
        public StateInstance(string c, string n)
        {
            this.category = c;
            this.name = n;
        }
    }
}