//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/23 8:11:42
//========================================================

using System;
using GameFramework.ClassExt;
using UnityEditor;
using UnityEngine;

namespace GameFramework.UnityEditorTool.Editor
{
    public class DragAreaGetObject : UnityEditor.Editor
    {
        public static UnityEngine.Object GetObject(Func<string, bool> func, EditorWindow window = null,
            string msg = null)
        {
            var aEvent = Event.current;
            UnityEngine.Object tmpObj = null;
            var dragArea = GUILayoutUtility.GetRect(0f, 35, GUILayout.ExpandWidth(true));
            GUIContent title = null;
            title = msg.IsNullOrEmpty()
                ? new GUIContent("Drag Object here from Project view to get the object")
                : new GUIContent(msg);
            GUI.Box(dragArea, title);

            switch (aEvent.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dragArea.Contains(aEvent.mousePosition))
                        break;
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if (aEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        if (DragAndDrop.objectReferences != null && DragAndDrop.objectReferences.Length > 0)
                        {
                            tmpObj = DragAndDrop.objectReferences[0];
                            if (!func(AssetDatabase.GetAssetPath(tmpObj)))
                            {
                                if (window != null)
                                    window.ShowNotification(new GUIContent("拖拽失败"));
                                return null;
                            }
                        }

                        else
                            break;
                    }

                    Event.current.Use();
                    break;
            }

            return tmpObj;
        }
    }
}