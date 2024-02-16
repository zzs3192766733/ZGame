//========================================================
// 描述:资源打包预览窗口
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/25 13:52:04
//========================================================

using System;
using UnityEngine;
using UnityEditor;
namespace GameFramework.UnityEditorTool.Editor
{
	public class AssetBundleViewWindow : EditorWindow
	{
		public static void Open()
		{
			GetWindow<AssetBundleViewWindow>().Show();
		}

		private void OnEnable()
		{
			ReadFoldTree();
		}

		private void OnGUI()
		{
			
		}

		private void OnProjectChange()
		{
			ReadFoldTree();
		}

		private void ReadFoldTree()
		{
			
		}
	}
}
