using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace AppSystem
{
    /// <summary>
    /// <TODO>とりあえず、Canvas と一体化しておくが今後 UI の一元管理とかをやるうえで変わるかもしれない</TODO>
    /// </summary>
    public class FadeProvider : StateInstance, ITransition
    {
        /// <summary>
        /// フェーダーのコンポーネント
        /// </summary>
        private const string kOriginalFaderAsset = "Assets/Resources/System/Fader/FadeCanvas.prefab";

        /// <summary>
        /// フェードパネル
        /// </summary>
        private const string kOriginalFaderComponent = "FadePanel";

        /// <summary>
        /// フェーダー一元管理
        /// </summary>
        private Dictionary<string, Fader> faders = new Dictionary<string, Fader>();

        /// <summary>
        /// オリジナルのコンポーネント
        /// </summary>
        private Canvas orignal = null;

        public override void Enter()
        {
            GameObject obj = PrefabUtility.LoadPrefabContents(kOriginalFaderAsset);
            this.orignal = obj.GetComponent<Canvas>();
        }

        public override void Leave()
        {
        }

        public override void Update()
        {
        }

        public void RequestChangeState(string state)
        {
        }

        /// <summary>フェーダーを登録</summary>
        /// <param name="obj">キャンバス</param>
        /// <returns></returns>
        private Fader Register(string name, Canvas canvas)
        {
            GameObject obj = canvas.transform.Find(kOriginalFaderComponent).gameObject;
            Fader fader = obj.GetComponent<Fader>();
            this.faders.Add(name, fader);
            return fader;
        }

        /// <summary>コンストラクタ</summary>
        /// <param name="c">カテゴリ</param>
        /// <param name="n">インスタンス名</param>
        private FadeProvider(string c, string n) : base(c, n) { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private FadeProvider() : this("System", "FadeProvider") { }

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        private static FadeProvider s_instance = null;

        /// <summary>
        /// アクセサ
        /// </summary>
        public static FadeProvider Instance { 
            get
            {
                if(s_instance == null)
                {
                    s_instance = new FadeProvider();
                }
                return s_instance;
            } 
        }

        /// <summary>フェードオブジェクトを生成する</summary>
        /// <param name="parent">親となるゲームオブジェクト</param>
        /// <returns>フェードインスタンス</returns>
        public static Fader CreateFader(string name, GameObject parent)
        {
            Canvas obj = GameObject.Instantiate(Instance.orignal);
            obj.transform.SetParent(parent.transform);
            return s_instance.Register(name, obj);
        }

        /// <summary>
        /// フェーダーリストへのアクセス
        /// </summary>
        public static Dictionary<string, Fader> Fader {
            get
            {
                return s_instance.faders;
            }
        }
    }
}