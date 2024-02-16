//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2022/2/14 17:40:49
//========================================================

using GameFramework.Common;
using UnityEngine;

namespace GameFramework.Setting
{
    public class ShowFPS : SingleMonoBase<ShowFPS>
    {
        public float f_UpdateInterval = 0.5F;
        private float f_LastInterval;
        private int i_Frames = 0;
        private float f_Fps;

        void Start()
        {
            f_LastInterval = UnityEngine.Time.realtimeSinceStartup;
            i_Frames = 0;
            DontDestroyOnLoad(this.gameObject);
        }

        private GUIStyle _labelStyle;

        private GUIStyle LabelStyle
        {
            get
            {
                if (_labelStyle == null)
                {
                    _labelStyle = new GUIStyle()
                    {
                        fontSize = 30,
                        normal = new GUIStyleState()
                        {
                            textColor = Color.white
                        }
                    };
                }

                return _labelStyle;
            }
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0, 0, 200, 200), "FPS:" + f_Fps.ToString("f2"), LabelStyle);
        }

        void Update()
        {
            ++i_Frames;
            if (UnityEngine.Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
            {
                f_Fps = i_Frames / (UnityEngine.Time.realtimeSinceStartup - f_LastInterval);
                i_Frames = 0;
                f_LastInterval = UnityEngine.Time.realtimeSinceStartup;
            }
        }
    }
}