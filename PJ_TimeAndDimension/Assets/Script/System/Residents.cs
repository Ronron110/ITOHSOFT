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
        /// 配下の常駐クラスたち
        /// </summary>
        private StateInstance[] residentObjects = new StateInstance[]
        {
        new Transition(),
            //
            //
        };

        // Start is called before the first frame update
        void Start()
        {
            Object.DontDestroyOnLoad(GameObject.Find("Residents"));
            foreach (StateInstance i in this.residentObjects)
            {
                i.Enter();
            }
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