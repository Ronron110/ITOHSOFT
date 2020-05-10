using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppSystem
{
    public class Title : GameState
    {
        Transition subScene = null;

        /// <summary>導入処理</summary>
        public override void Enter()
        {
            SceneManager.LoadScene("Scenes/OutGame/Title");
        }

        /// <summary>アップデート処理</summary>
        public override void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                this.ChangeState("Stage1");
            }
        }

        /// <summary>終了処理</summary>
        public override void Leave()
        {
            SceneManager.UnloadSceneAsync("Scenes/OutGame/Title");
        }

        /// <summary>コンストラクタ</summary>
        /// <param name="i">Transition インターフェイス</param>
        public Title(ITransition i) : base(i, "OutGame", "Title")
        {

        }

    }
}