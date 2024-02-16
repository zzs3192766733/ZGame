//========================================================
// 描述:Mono单例基类
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/10 10:54:22
//========================================================

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GameFramework.Common
{
    public class SingleMonoBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();
        protected static bool ApplicationIsQuitting { get; private set; } = false;
        protected static bool isGlobal = true;

        static SingleMonoBase()
        {
            ApplicationIsQuitting = false;
        }


        public static T Instance
        {
            get
            {
                if (ApplicationIsQuitting)
                {
                    if (Debug.isDebugBuild)
                    {
                        Debug.LogWarning("[Singleton] " + typeof(T) +
                                         " already destroyed on application quit." +
                                         " Won't create again - returning null.");
                    }
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                        if (FindObjectsOfType<T>().Length > 1)
                        {
                            if (Debug.isDebugBuild)
                            {
                                Debug.LogWarning("[Singleton] " + typeof(T).Name +" should never be more than 1 in scene!");
                            }

                            return _instance;
                        }
                    }

                    if (_instance == null)
                    {
                        var go = new GameObject("(Single)"+typeof(T));
                        _instance = go.AddComponent<T>();
                        if (isGlobal && Application.isPlaying)
                        {
                            DontDestroyOnLoad(go);
                        }
                    }
                }

                return _instance;
            }
        }

        private void OnApplicationQuit()
        {
            ApplicationIsQuitting = true;
        }
    }
}