using System.Text;
using GameFramework.Common;
using UnityEngine;
using UnityEditor;

namespace GameFramework.UnityEditorTool.Editor
{
    public static class GameObjectMenuExt
    {
        [MenuItem("GameObject/复制相对路径(不包括根节点,包含叶子节点)", false, -100)]
        private static void CopyPath()
        {
            var selectTrans = Selection.activeTransform;
            if (selectTrans != null)
            {
                var tmpTrans = selectTrans;
                var path = new StringBuilder();
                while (tmpTrans != null)
                {
                    path.Insert(0, tmpTrans.name);
                    tmpTrans = tmpTrans.parent;
                    if (tmpTrans != null)
                        path.Insert(0, "/");
                }
                //将路径复制到剪切板中
                GUIUtility.systemCopyBuffer = path.ToString();
                GameLogger.Log(path.ToString());
            }
        }
    }
}