//========================================================
// 描述:GUI帮助类
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/11 9:30:05
//========================================================

using UnityEditor;
using UnityEngine;

namespace GameFramework.UnityEditorTool.Editor
{
    public static class GameFrameworkGUIHelper
    {
        public static GUIStyle CloneGUIStyle(GUIStyle source)
        {
            var clone = new GUIStyle
            {
                normal = source.normal,
                hover = source.hover,
                active = source.active,
                onNormal = source.onNormal,
                onHover = source.onHover,
                onActive = source.onActive,
                focused = source.focused,
                onFocused = source.onFocused,
                border = source.border,
                margin = source.margin,
                padding = source.padding,
                overflow = source.overflow,
                font = source.font,
                imagePosition = source.imagePosition,
                alignment = source.alignment,
                wordWrap = source.wordWrap,
                clipping = source.clipping,
                contentOffset = source.contentOffset,
                fixedWidth = source.fixedWidth,
                fixedHeight = source.fixedHeight,
                stretchWidth = source.stretchWidth,
                stretchHeight = source.stretchHeight
            };
            return clone;
        }
        public static void DrawCopyright()
        {
            GUILayout.BeginHorizontal(GUI.skin.box);
            {
                GUILayout.Label(
                    $"当前版本:{GameFrameworkGlobalConfig.GameFrameworkVersion}\t{GameFrameworkGlobalConfig.GameFrameworkName}\t{GameFrameworkGlobalConfig.GameFrameworkEmail}");
                GUILayout.FlexibleSpace();
                GUILayout.Label("学海无涯，苦作舟，还请默默努力");
            }
            GUILayout.EndHorizontal();
        }
        public static Texture GetSourceTex(string texName) => AssetDatabase.LoadAssetAtPath<Texture>(GameFrameworkGlobalConfig.EditorSourcePath + texName);
        public static Rect GetRect(Rect oldRect, int index)
        {
            var newRect = new Rect(oldRect);
            newRect.x += newRect.width - 20 * index;
            newRect.width = 18;
            return newRect;
        }
        public static void DrawRectIcon<T>(Rect oldRect, ref int index)where T : Component
        {
            var newRect = GetRect(oldRect,++index);
            GUI.Label(newRect,EditorGUIUtility.ObjectContent(null,typeof(T)).image);
        }
        public static void DrawRectIcon<T>(Rect oldRect, ref int index,GameObject go)where T : Component
        {
            if (go.GetComponent<T>() == null) return;
            var newRect = GetRect(oldRect,++index);
            GUI.Label(newRect,EditorGUIUtility.ObjectContent(null,typeof(T)).image);
        }
    }
}
