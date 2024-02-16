//========================================================
// 描述:Hierarchy面板拓展
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/20 16:33:20
//========================================================

using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace GameFramework.UnityEditorTool.Editor
{
    [InitializeOnLoad]
    public class HierarchyIconManager
    {
        static HierarchyIconManager()
        {
            EditorApplication.hierarchyWindowItemOnGUI = HierarchyWindowItemOnGUI;
        }

        private static void HierarchyWindowItemOnGUI(int instanceid, Rect selectionrect)
        {
            if (!GameFrameworkGlobalConfig.IsHierarchyExpand) return;
            var newRect = new Rect(selectionrect);
            newRect.x += newRect.width;
            newRect.width = 18;
            var go = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
            if (go == null) return;
            go.SetActive(GUI.Toggle(newRect, go.activeSelf, string.Empty));

            var index = 0;
            if (go.isStatic)
            {
                index++;
                GUI.Label(GameFrameworkGUIHelper.GetRect(selectionrect, index), "S");
            }

            //模型相关
            GameFrameworkGUIHelper.DrawRectIcon<MeshRenderer>(selectionrect, ref index, go);
            GameFrameworkGUIHelper.DrawRectIcon<MeshFilter>(selectionrect, ref index, go);
            GameFrameworkGUIHelper.DrawRectIcon<BoxCollider>(selectionrect, ref index, go);
            GameFrameworkGUIHelper.DrawRectIcon<CapsuleCollider>(selectionrect, ref index, go);
            GameFrameworkGUIHelper.DrawRectIcon<SphereCollider>(selectionrect, ref index, go);
            //UI相关
            GameFrameworkGUIHelper.DrawRectIcon<Image>(selectionrect, ref index, go);
            GameFrameworkGUIHelper.DrawRectIcon<Text>(selectionrect, ref index, go);
            GameFrameworkGUIHelper.DrawRectIcon<Button>(selectionrect, ref index, go);
            //动画相关
            GameFrameworkGUIHelper.DrawRectIcon<Animation>(selectionrect, ref index, go);
            GameFrameworkGUIHelper.DrawRectIcon<Animator>(selectionrect, ref index, go);
            //声音相关
            GameFrameworkGUIHelper.DrawRectIcon<AudioSource>(selectionrect, ref index, go);
            //相机
            GameFrameworkGUIHelper.DrawRectIcon<Camera>(selectionrect, ref index, go);
        }
    }
}