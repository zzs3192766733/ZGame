//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/12 12:46:15
//========================================================

using System;
using System.IO;
using GameFramework.ClassExt;
using UnityEditor;
using UnityEngine;

namespace GameFramework.UnityEditorTool.Editor
{
	
	public class GameFrameworkMenuEditor : EditorWindow
	{
		[MenuItem("GameFramework/Version信息",priority = 1)]
		private static void OpenMenu()
		{
			GetWindow<GameFrameworkMenuEditor>(true,"更新信息窗口").Show();
		}

		[MenuItem("GameFramework/更新Editor文件Config信息(手动修改配置文件后使用)",priority = 2)]
		private static void UpdateEditorConfigInfo()
		{
			GameFrameworkGlobalConfig.ReadConfig();
		}
		
		private GUIStyle _tileGUIStyle;
		private GUIStyle TileGUIStyle => _tileGUIStyle ?? (_tileGUIStyle = new GUIStyle
		{
			fontSize = 30, fontStyle = FontStyle.Bold,
			normal = new GUIStyleState {textColor = Color.white}
		});
		
		private GUIStyle _infoGUIStyle;
		private GUIStyle InfoGUIStyle => _infoGUIStyle ?? (_infoGUIStyle = new GUIStyle
		{
			fontSize = 20
		});

		private string _versionInfo;

		private void OnEnable()
		{
			var path = Application.dataPath+"/GameFramework/UnityEditorTool/Editor/Version/version.txt";
			_versionInfo = ReadVersionText(path);
		}

		private Vector2 _ve2;
		private void OnGUI()
		{
			GUILayout.BeginHorizontal(GUI.skin.box);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label("框架版本信息",TileGUIStyle);
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			
			GUILayout.Space(20);
			GUILayout.BeginVertical(GUI.skin.window);
			{
				_ve2 = GUILayout.BeginScrollView(_ve2);
				{
					if (!_versionInfo.IsNullOrEmpty())
						GUILayout.Label(_versionInfo,InfoGUIStyle);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndScrollView();
			}
			GUILayout.EndVertical();
			GameFrameworkGUIHelper.DrawCopyright();
		}

		private string ReadVersionText(string path)
		{
			try
			{
				using (var sr = new StreamReader(path))
				{
					return sr.ReadToEnd();
				}
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
