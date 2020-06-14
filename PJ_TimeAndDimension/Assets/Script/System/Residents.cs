using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppSystem
{
    /// <summary>
    /// 常駐オブジェクトの挙動
    /// </summary>
    public class Residents : MonoBehaviour
    {
        /// <summary>
        /// フェーダー名
        /// </summary>
        public const string kFader = "GlobalFader";

        /// <summary>
        /// 常駐オブジェクトのルート
        /// </summary>
        [SerializeField]
        private GameObject residents;

        /// <summary>
        /// 配下の常駐クラスたち
        /// </summary>
        private StateInstance[] residentObjects = new StateInstance[]
        {
            new Transition(),
            FadeProvider.Instance,
            //
            //
        };

        // Start is called before the first frame update
        void Start()
        {
            Object.DontDestroyOnLoad(this.residents);
            foreach (StateInstance i in this.residentObjects)
            {
                i.Enter();
            }
            FadeProvider.CreateFader(kFader, this.residents);
        }

        // Update is called once per frame
        void Update()
        {
            foreach (StateInstance i in this.residentObjects)
            {
                i.Update();
            }
        }

        private void OnDestroy()
        {
            foreach (StateInstance i in this.residentObjects)
            {
                i.Leave();
            }
        }
    }
}
