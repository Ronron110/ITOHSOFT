using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppSystem
{
    public class Title : GameState
    {
        Transition subScene = null;

        private TitleObject titleObject = null;

        /// <summary>導入処理</summary>
        public override void Enter()
        {
            SceneManager.LoadScene("Scene/OutGame/Title");
        }

        /// <summary>アップデート処理</summary>
        public override void Update()
        {
            if(titleObject == null)
            {
                GameObject obj = GameObject.Find("TitleObject");
                if (obj != null)
                {
                    titleObject = obj.GetComponent<TitleObject>();
                }
            }
            else
            {
                // スペースの受付
                if (Input.GetKey(KeyCode.Space))
                {
                    // ステートを進める
                    titleObject.ProceedState();
                }
                if (titleObject.IsFinished() == true)
                {
                    this.ChangeState("Stage1");
                }
            }
        }

        /// <summary>終了処理</summary>
        public override void Leave()
        {
            SceneManager.UnloadSceneAsync("Scene/OutGame/Title");
        }

        /// <summary>コンストラクタ</summary>
        /// <param name="i">Transition インターフェイス</param>
        public Title(ITransition i) : base(i, "OutGame", "Title")
        {

        }

    }
}