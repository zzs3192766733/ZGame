//========================================================
// 描述:GameFrameworkGlobalConfig 编辑器全局配置
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/12 12:50:06
//========================================================

using System;
using UnityEngine;
using System.Xml;
using GameFramework.ClassExt;
using System.IO;
using System.Reflection;
using UnityEditor;

namespace GameFramework.UnityEditorTool.Editor
{
	
	public static class GameFrameworkGlobalConfig
	{
		public static string GameFrameworkVersion = "1.0.1";

		public static string GameFrameworkInfo = @"
			用于游戏的游戏框架
		";

		public static string GameFrameworkName = "周忠帅";
		public static string GameFrameworkEmail = "3192766733@qq.com";

		public static bool IsAutoCreateTitle = false;
		public static bool IsHierarchyExpand = false;
		public static string EditorSourcePath = "Assets/GameFramework/Source/";
		public static string GAssetPath = Application.dataPath + "/GAssets";
		public static string AbOutputPath = Application.streamingAssetsPath;
		public static BuildAssetBundleOptions BuildAssetBundleOptions = BuildAssetBundleOptions.None;
		public static BuildTarget BuildAssetBundleTarget = EditorUserBuildSettings.activeBuildTarget;


		static GameFrameworkGlobalConfig()
		{
			//读Xml配置信息,初始化整体信息
			//1.先对本地Xml进行读取，若本地没有Xml配置文件则创建默认的Xml
			//2.若含有Xml，则直接进行读取并赋值到各个变量当中
			//3.再编辑器环境下，需要通过用户手动保存才能进行保存Xml
			//4.每次增加字段需要增加下面的
			try
			{
				ReadConfig();
			}
			catch (Exception)
			{
				SaveConfig();
			}
		}
		private static void CreateNode(XmlDocument xmlDoc,XmlNode root,string name,string val)
		{
			var node = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
			node.InnerText = val;
			root.AppendChild(node);
		}
		public static void ReadConfig()
		{
			var configXmlPath = Application.dataPath + "/GameFramework/UnityEditorTool/Editor/Config/Config.xml";
			var xmlDocument = new XmlDocument();
			xmlDocument.Load(configXmlPath);
			var rootNode = xmlDocument.SelectSingleNode("Config");
			if (rootNode != null)
			{
				//从Xml文件中读取配置信息
				GameFrameworkVersion = rootNode[nameof(GameFrameworkVersion)]?.InnerText;
				GameFrameworkInfo = rootNode[nameof(GameFrameworkInfo)]?.InnerText;
				GameFrameworkName = rootNode[nameof(GameFrameworkName)]?.InnerText;
				GameFrameworkEmail = rootNode[nameof(GameFrameworkEmail)]?.InnerText;
				IsAutoCreateTitle = (rootNode[nameof(IsAutoCreateTitle)]?.InnerText).ToBool();
				IsHierarchyExpand = (rootNode[nameof(IsHierarchyExpand)]?.InnerText).ToBool();
				EditorSourcePath = rootNode[nameof(EditorSourcePath)]?.InnerText;
				GAssetPath = rootNode[nameof(GAssetPath)]?.InnerText;
				AbOutputPath = rootNode[nameof(AbOutputPath)]?.InnerText;
				BuildAssetBundleOptions =
					(rootNode[nameof(BuildAssetBundleOptions)]?.InnerText).ToEnum<BuildAssetBundleOptions>();
				BuildAssetBundleTarget =
					(rootNode[nameof(BuildAssetBundleTarget)]?.InnerText).ToEnum<BuildTarget>();
			}
		}
		public static void SaveConfig()
		{
			var path = Application.dataPath + "/GameFramework/UnityEditorTool/Editor/Config/Config.xml";
			var xmlDocument = new XmlDocument();
			if(File.Exists(path))
				File.Delete(path);
			//本地无Xml文件,进行创建
			var xmlNode = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "");
			xmlDocument.AppendChild(xmlNode);
			var root = xmlDocument.CreateElement("Config");
			xmlDocument.AppendChild(root);
			var fieldInfos = typeof(GameFrameworkGlobalConfig).GetFields(BindingFlags.Static|BindingFlags.Instance|BindingFlags.Public);
			foreach (var fieldInfo in fieldInfos)
				CreateNode(xmlDocument,root,fieldInfo.Name,fieldInfo.GetValue(null).ToString());
			xmlDocument.Save(path);
			AssetDatabase.Refresh();
		}
	}

	public class GameFrameworkConfigSettingWindow : EditorWindow
	{
		[MenuItem("GameFramework/ConfigSettingWindow(图形化界面修改GameFramework配置信息)")]
		private static void OpenConfigSettingWindow()
		{
			GetWindow<GameFrameworkConfigSettingWindow>("图形化界面修改GameFramework配置信息").Show();
		}

		private Vector2 _ve2;
		private void OnGUI()
		{
			GUILayout.BeginHorizontal(GUI.skin.box);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label("图形化界面修改GameFramework配置信息");
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			
			GUILayout.Space(5);
			GUILayout.BeginHorizontal(GUI.skin.box);
			{
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("更新Editor文件Config信息"))
				{
					GameFrameworkGlobalConfig.ReadConfig();
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10);
			
			_ve2 = GUILayout.BeginScrollView(_ve2);
			{
				
			}
			GUILayout.EndScrollView();
		}
	}
}
