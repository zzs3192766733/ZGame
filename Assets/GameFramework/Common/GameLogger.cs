//========================================================
// 描述:日志打印输出
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/26 8:53:08
//========================================================

using System;
using System.IO;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameFramework.Common
{
    public static class GameLogger
    {
        public static bool debugEnable = true;
        public static bool warningEnable = true;
        public static bool errorEnable = true;
        public static bool outPut = true;
        private static readonly StringBuilder m_logStr = new StringBuilder();

        private static readonly string outPath;

        static GameLogger()
        {
            if (outPut)
            {
                outPath = $"{Application.persistentDataPath}/debug/debug_out_{DateTime.Now:yyyyMMddhhmmss}.log";
                Application.logMessageReceived += OnLogCallBack;
            }
        }

        private static void OnLogCallBack(string condition, string stackTrace, LogType type)
        {
            m_logStr.Append(condition);
            m_logStr.Append("\n");
            m_logStr.Append(stackTrace);
            m_logStr.Append("\n");

            if (m_logStr.Length <= 0) return;

            if (!Directory.Exists($"{Application.persistentDataPath}/debug"))
                Directory.CreateDirectory($"{Application.persistentDataPath}/debug");
            if (!File.Exists(outPath))
            {
                var fs = File.Create(outPath);
                fs.Close();
            }

            using (var sw = File.AppendText(outPath))
            {
                sw.WriteLine(m_logStr.ToString());
            }

            m_logStr.Remove(0, m_logStr.Length);
        }

#if UNITY_EDITOR
        [UnityEditor.Callbacks.OnOpenAssetAttribute(0)]
        static bool OnOpenAsset(int instanceID, int line)
        {
            string stackTrace = GetStackTrace();
            if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains("GameLogger:Log"))
            {
                // 使用正则表达式匹配at的哪个脚本的哪一行
                var matches = System.Text.RegularExpressions.Regex.Match(stackTrace, @"\(at (.+)\)",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                string pathLine = "";
                while (matches.Success)
                {
                    pathLine = matches.Groups[1].Value;

                    if (!pathLine.Contains("GameLogger.cs"))
                    {
                        int splitIndex = pathLine.LastIndexOf(":");
                        // 脚本路径
                        string path = pathLine.Substring(0, splitIndex);
                        // 行号
                        line = System.Convert.ToInt32(pathLine.Substring(splitIndex + 1));
                        string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
                        fullPath = fullPath + path;
                        // 跳转到目标代码的特定行
                        UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath.Replace('/', '\\'),
                            line);
                        break;
                    }

                    matches = matches.NextMatch();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取当前日志窗口选中的日志的堆栈信息
        /// </summary>
        /// <returns></returns>
        static string GetStackTrace()
        {
            // 通过反射获取ConsoleWindow类
            var ConsoleWindowType = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
            // 获取窗口实例
            var fieldInfo = ConsoleWindowType.GetField("ms_ConsoleWindow",
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.NonPublic);
            var consoleInstance = fieldInfo.GetValue(null);
            if (consoleInstance != null)
            {
                if ((object) UnityEditor.EditorWindow.focusedWindow == consoleInstance)
                {
                    // 获取m_ActiveText成员
                    fieldInfo = ConsoleWindowType.GetField("m_ActiveText",
                        System.Reflection.BindingFlags.Instance |
                        System.Reflection.BindingFlags.NonPublic);
                    // 获取m_ActiveText的值
                    string activeText = fieldInfo.GetValue(consoleInstance).ToString();
                    return activeText;
                }
            }

            return null;
        }
#endif

        public static void Log(object message, Object context = null)
        {
            if (!debugEnable) return;
            Debug.Log(message, context);
        }

        public static void LogWarning(object message, Object context = null)
        {
            if (!warningEnable) return;
            Debug.LogWarning(message, context);
        }

        public static void LogError(object message, Object context = null)
        {
            if (!errorEnable) return;
            Debug.LogError(message, context);
        }

        public static void Log(object message, DebugColor color)
        {
            if (!debugEnable) return;
            switch (color)
            {
                case DebugColor.Red:
                    Debug.LogFormat("<color=#ff0000>{0}</color>", message);
                    break;
                case DebugColor.Green:
                    Debug.LogFormat("<color=#00ff00>{0}</color>", message);
                    break;
                case DebugColor.Blue:
                    Debug.LogFormat("<color=#0000ff>{0}</color>", message);
                    break;
                case DebugColor.Yellow:
                    Debug.LogFormat("<color=yellow>{0}</color>", message);
                    break;
            }
        }

        public static void LogWarning(object message, DebugColor color)
        {
            if (!warningEnable) return;
            switch (color)
            {
                case DebugColor.Red:
                    Debug.LogWarningFormat("<color=#ff0000>{0}</color>", message);
                    break;
                case DebugColor.Green:
                    Debug.LogWarningFormat("<color=#00ff00>{0}</color>", message);
                    break;
                case DebugColor.Blue:
                    Debug.LogWarningFormat("<color=#0000ff>{0}</color>", message);
                    break;
                case DebugColor.Yellow:
                    Debug.LogWarningFormat("<color=yellow>{0}</color>", message);
                    break;
            }
        }

        public static void LogError(object message, DebugColor color)
        {
            if (!errorEnable) return;
            switch (color)
            {
                case DebugColor.Red:
                    Debug.LogErrorFormat("<color=#ff0000>{0}</color>", message);
                    break;
                case DebugColor.Green:
                    Debug.LogErrorFormat("<color=#00ff00>{0}</color>", message);
                    break;
                case DebugColor.Blue:
                    Debug.LogErrorFormat("<color=#0000ff>{0}</color>", message);
                    break;
                case DebugColor.Yellow:
                    Debug.LogErrorFormat("<color=yellow>{0}</color>", message);
                    break;
            }
        }

        public static void LogFormat(string format, params object[] args)
        {
            if (!debugEnable) return;
            Debug.LogFormat(format, args);
        }

        public static void LogWarningFormat(string format, params object[] args)
        {
            if (!warningEnable) return;
            Debug.LogWarningFormat(format, args);
        }

        public static void LogErrorFormat(string format, params object[] args)
        {
            if (!errorEnable) return;
            Debug.LogErrorFormat(format, args);
        }
    }

    public enum DebugColor
    {
        Red,
        Green,
        Blue,
        Yellow
    }
}