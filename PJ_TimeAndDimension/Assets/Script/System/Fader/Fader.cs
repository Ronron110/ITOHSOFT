using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.UI;

namespace AppSystem
{
    public class Fader : MonoBehaviour
    {
        /// <summary>
        /// イメージコンポーネント
        /// </summary>
        [SerializeField]
        private Image image = null;

        /// <summary>
        /// 開始カラー
        /// </summary>
        private Color start = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>
        /// ターゲットカラー
        /// </summary>
        private Color target = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>
        /// 現在のフェードカラー
        /// </summary>
        private Color current = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>
        /// フェード時間
        /// </summary>
        private double duration = 0.0;

        /// <summary>
        /// フェード開始時刻
        /// </summary>
        private double begun = 0.0;

        /// <summary>
        /// フェード終了
        /// </summary>
        private bool toBeHidden = false;

        private void Start()
        {
            this.image = this.GetComponent<Image>();
            this.image.rectTransform.rect.Set(0, 0, Screen.width, Screen.height);
        }

        private void Update()
        {
            if (this.gameObject.activeInHierarchy)
            {
                if (this.toBeHidden == true)
                {
                    this.gameObject.SetActive(false);
                    this.duration = 0.0;
                    this.begun = 0.0;
                    this.toBeHidden = false;
                }
                else
                {
                    if (Time.timeScale > 0.0)
                    {
                        if (this.duration > 0.0)
                        {
                            double elapsed = (Time.time - this.begun) / Time.timeScale;
                            float ratio = (float)(elapsed / this.duration);
                            if (ratio > 1.0f)
                            {
                                ratio = 1.0f;
                                if (target.a == 0.0f)
                                {
                                    this.toBeHidden = true;
                                }
                            }
                            this.current.r = Mathf.Lerp(this.start.r, this.target.r, ratio);
                            this.current.g = Mathf.Lerp(this.start.g, this.target.g, ratio);
                            this.current.b = Mathf.Lerp(this.start.b, this.target.b, ratio);
                            this.current.a = Mathf.Lerp(this.start.a, this.target.a, ratio);
                            this.image.color = this.current;
                        }
                    }
                }
            }
        }

        /// <summary>フェード中かどうか</summary>
        /// <returns>true: フェード中です / false: フェード中ではありません</returns>
        public bool IsActive()
        {
            return this.gameObject.activeInHierarchy;
        }

        /// <summary>フリーなフェード</summary>
        /// <param name="d">durationフェード時間</param>
        /// <param name="t">target フェードターゲットカラー</param>
        /// <param name="s">start フェード開始カラー省略すると現在のフェーダーの色からスタート</param>
        public void Request(Color t, double d, Color? s = null)
        {
            this.start = this.current;
            if (s != null)
            {
                this.start = s.Value;
            }
            this.target = t;
            this.duration = d;
            this.begun = Time.time;
            this.image.color = this.current;
            this.gameObject.SetActive(true);
            this.toBeHidden = false;
        }

        /// <summary>デフォルトのフェードアウト（黒フェード）</summary>
        /// <param name="d">durationフェード時間</param>
        /// <param name="t">target フェードターゲットカラー</param>
        public void FadeOut(double d, Color? t = null)
        {
            if (t == null)
            {
                t = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            }
            Request(t.Value, d);
        }

        /// <summary>デフォルトのフェードイン（黒フェード）</summary>
        /// <param name="d">durationフェード時間</param>
        /// <param name="t">target フェードターゲットカラー</param>
        public void FadeIn(double d, Color? t = null)
        {
            if (t == null)
            {
                t = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }
            Request(t.Value, d);
        }
    }
}
