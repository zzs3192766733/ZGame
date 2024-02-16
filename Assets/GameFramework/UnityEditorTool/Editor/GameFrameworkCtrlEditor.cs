//========================================================
// 描述:GameFramework主控制界面
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/11 9:30:05
//========================================================

using System;
using System.Collections.Generic;
using System.IO;
using GameFramework.Common;
using GameFramework.Res;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameFramework.UnityEditorTool.Editor
{
    public class GameFrameworkCtrlEditor : EditorWindow
    {
        [MenuItem("GameFramework/GameFramework(主控制器面板)", priority = -99)]
        private static void OpenWindow()
        {
            GetWindow<GameFrameworkCtrlEditor>("GameFramework主控制器面板").Show();
        }

        private GUIStyle _tileGUIStyle;
        private const int WindowCount = 3;

        private GUIStyle TileGUIStyle => _tileGUIStyle ?? (_tileGUIStyle = new GUIStyle
        {
            fontSize = 30, fontStyle = FontStyle.Bold,
            normal = new GUIStyleState {textColor = Color.white}
        });

        private Texture _testTex;
        private Texture TestTex => _testTex ? _testTex : GameFrameworkGUIHelper.GetSourceTex("w1.png");

        private int _selectBundleOptionsIndex = 0;
        private int _selectBundleTarget = 0;

        private void OnEnable()
        {
            switch (GameFrameworkGlobalConfig.BuildAssetBundleOptions)
            {
                case BuildAssetBundleOptions.None:
                    _selectBundleOptionsIndex = 0;
                    break;
                case BuildAssetBundleOptions.UncompressedAssetBundle:
                    _selectBundleOptionsIndex = 1;
                    break;
                case BuildAssetBundleOptions.ChunkBasedCompression:
                    _selectBundleOptionsIndex = 2;
                    break;
            }

            switch (GameFrameworkGlobalConfig.BuildAssetBundleTarget)
            {
                case BuildTarget.Android:
                    _selectBundleTarget = 0;
                    break;
                case BuildTarget.StandaloneWindows:
                    _selectBundleTarget = 1;
                    break;
                case BuildTarget.iOS:
                    _selectBundleTarget = 2;
                    break;
            }

            _assetsTreeView = new GAssetsTreeView(new TreeViewState(), GetGAssetsTreeViewItems());
            _searchField = new SearchField();
            _searchField.downOrUpArrowKeyPressed += _assetsTreeView.SetFocusAndEnsureSelectedItem;
        }

        private Vector2 _threeWindowVector2;
        private GAssetsTreeView _assetsTreeView;
        private SearchField _searchField;
        private int _currSelectTreeViewID = -1;
        private string _currSelectTreeViewName = string.Empty;

        private void OnProjectChange()
        {
            _assetsTreeView = new GAssetsTreeView(new TreeViewState(), GetGAssetsTreeViewItems());
        }

        private void OnGUI()
        {
            //标题
            GUILayout.BeginHorizontal("Box");
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(TestTex);
                GUILayout.Label("GameFramework主控制器面板", TileGUIStyle);
                GUILayout.Label(TestTex);
                GUILayout.FlexibleSpace();
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("Box");
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("保存信息", GUILayout.Width(200)))
                {
                    GameFrameworkGlobalConfig.SaveConfig();
                    ShowNotification(new GUIContent("保存成功"));
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                var width = this.position.width / WindowCount;
                //Editor相关
                GUILayout.BeginVertical(GUI.skin.window, GUILayout.MaxWidth(width));
                {
                    DrawWindow("Editor相关", "TerrainInspector.TerrainToolSettings");
                    //是否开启自动创建命名空间
                    GameFrameworkGlobalConfig.IsAutoCreateTitle =
                        GUILayout.Toggle(GameFrameworkGlobalConfig.IsAutoCreateTitle, "是否开启自动创建代码命名空间");
                    GUILayout.Space(10);
                    //是否开启Hierarchy拓展
                    GameFrameworkGlobalConfig.IsHierarchyExpand =
                        GUILayout.Toggle(GameFrameworkGlobalConfig.IsHierarchyExpand, "是否开启Hierarchy拓展");
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUI.skin.window, GUILayout.MaxWidth(width));
                {
                    DrawWindow("资源打包相关", "Prefab Icon");
                    GUILayout.Label("一.打包内部参数显示:");
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        GUILayout.Label("1.BundleMode(当前AB包模式): " +
                                        (ResConfigBase.BundleMode ? "AB包中读取" : "AssetsDataBase中读取"));
                        GUILayout.Label("2.资源模块跟目录: " + GameFrameworkGlobalConfig.GAssetPath);
                        GUILayout.Label("3.AssetBundle输出路径: " + GameFrameworkGlobalConfig.AbOutputPath);
                    }
                    GUILayout.EndVertical();

                    GUILayout.Label("二.打包设置:");
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        #region 选择压缩方式

                        //BuildAssetBundleOptions.None：使用LZMA算法压缩，压缩的包更小，但是加载时间更长
                        //BuildAssetBundleOptions.UncompressedAssetBundle：不压缩，包大，加载快
                        //BuildAssetBundleOptions.ChunkBasedCompression：使用LZ4压缩，压缩率没有LZMA高，但是我们可以加载指定资源而不用解压全部
                        var tmp1 = _selectBundleOptionsIndex;
                        _selectBundleOptionsIndex = EditorGUILayout.Popup("1.选择压缩方式:", _selectBundleOptionsIndex,
                            new string[] {"LZMA压缩", "不压缩", "LZ4压缩"});
                        if (tmp1 != _selectBundleOptionsIndex)
                        {
                            switch (_selectBundleOptionsIndex)
                            {
                                case 0:
                                    GameFrameworkGlobalConfig.BuildAssetBundleOptions = BuildAssetBundleOptions.None;
                                    break;
                                case 1:
                                    GameFrameworkGlobalConfig.BuildAssetBundleOptions =
                                        BuildAssetBundleOptions.UncompressedAssetBundle;
                                    break;
                                default:
                                    GameFrameworkGlobalConfig.BuildAssetBundleOptions =
                                        BuildAssetBundleOptions.ChunkBasedCompression;
                                    break;
                            }
                        }

                        #endregion

                        #region 选择打包平台

                        var tmp2 = _selectBundleTarget;
                        _selectBundleTarget = EditorGUILayout.Popup("2.选择打包平台:", _selectBundleTarget,
                            new string[] {"Android", "StandaloneWindows", "iOS"});
                        if (tmp2 != _selectBundleTarget)
                        {
                            switch (_selectBundleTarget)
                            {
                                case 0:
                                    GameFrameworkGlobalConfig.BuildAssetBundleTarget = BuildTarget.Android;
                                    break;
                                case 1:
                                    GameFrameworkGlobalConfig.BuildAssetBundleTarget = BuildTarget.StandaloneWindows;
                                    break;
                                default:
                                    GameFrameworkGlobalConfig.BuildAssetBundleTarget = BuildTarget.iOS;
                                    break;
                            }
                        }

                        #endregion
                    }
                    GUILayout.EndVertical();
                    GUILayout.Label("三.功能按钮");
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        if (GUILayout.Button("AssetBundleBuild(打包AB包文件)"))
                        {
                            AbEditor.Build();
                        }

                        GUILayout.Space(10);
                        if (GUILayout.Button("打开AB包打包文件夹"))
                        {
                            if (Directory.Exists(Application.streamingAssetsPath))
                                EditorUtility.RevealInFinder(Application.streamingAssetsPath + "/");
                            else
                                ShowNotification(new GUIContent("streamingAssetsPath文件夹不存在"));
                        }

                        GUILayout.Space(10);
                        if (GUILayout.Button("打开AssetBundle预览窗口"))
                        {
                            AssetBundleViewWindow.Open();
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();


                GUILayout.BeginVertical(GUI.skin.window, GUILayout.MaxWidth(width));
                {
                    DrawWindow("GAssets资源结构", "AvatarInspector/DotFrameDotted");
                    GUILayout.BeginHorizontal(EditorStyles.toolbar);
                    {
                        _assetsTreeView.searchString = _searchField.OnToolbarGUI(_assetsTreeView.searchString);
                    }
                    GUILayout.EndHorizontal();
                    var rect = GUILayoutUtility.GetRect(0, 10000, 0, 10000);
                    _assetsTreeView.OnGUI(rect);
                    GUILayout.BeginVertical();
                    {
                        GUILayout.BeginHorizontal("box");
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label("设置");
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.Space(5);
                        GUILayout.BeginVertical("Box");
                        {
                            GUILayout.BeginHorizontal();
                            {
                                if (GUILayout.Button("展开"))
                                    _assetsTreeView.ExpandAll();
                                if (GUILayout.Button("折叠"))
                                    _assetsTreeView.CollapseAll();
                            }
                            GUILayout.EndHorizontal();

                            //现实当前选择资源路径
                            if (_assetsTreeView.HasSelection())
                            {
                                var selectID = _assetsTreeView.GetSelection()[0];
                                if (_currSelectTreeViewID != selectID)
                                {
                                    var selectTreeView = _assetsTreeView.GetItemByID(selectID);
                                    _currSelectTreeViewID = selectID;
                                    var tmp = selectTreeView;
                                    _currSelectTreeViewName = selectTreeView.displayName;
                                    while (tmp.parent != null)
                                    {
                                        tmp = tmp.parent;
                                        _currSelectTreeViewName = tmp.displayName + "/" + _currSelectTreeViewName;
                                    }
                                    _currSelectTreeViewName = _currSelectTreeViewName.Replace("Root/各个模块信息", "Assets/GAssets");
                                    var obj = AssetDatabase.LoadAssetAtPath(_currSelectTreeViewName, typeof(Object));
                                    if(obj==null)
                                        GameLogger.Log("Null");
                                    else
                                    {
                                        EditorGUIUtility.PingObject(obj);
                                        Selection.activeObject = obj;
                                    }
                                }

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("当前选择资源路径:" + _currSelectTreeViewName,GUILayout.MinWidth(200));
                                if (GUILayout.Button("Copy Path"))
                                    GUIUtility.systemCopyBuffer = _currSelectTreeViewName;
                                GUILayout.EndHorizontal();
                            }
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            //显示版权信息
            GameFrameworkGUIHelper.DrawCopyright();
        }

        private void DrawWindow(string tile, string icon = null)
        {
            GUILayout.BeginHorizontal("Box");
            {
                GUILayout.FlexibleSpace();
                if (icon != null)
                    GUILayout.Label(EditorGUIUtility.IconContent(icon), GUILayout.Width(20), GUILayout.Height(20));
                GUILayout.Label(tile);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private List<TreeViewItem> GetGAssetsTreeViewItems()
        {
            var assetsTreeViewList = new List<TreeViewItem>();
            var index = 1;
            assetsTreeViewList.Add(new TreeViewItem(index++, 0, "各个模块信息"));
            var rootPath = GameFrameworkGlobalConfig.GAssetPath;
            var rootDirInfo = new DirectoryInfo(rootPath);
            var moduleDirInfos = rootDirInfo.GetDirectories();
            foreach (var moduleDirInfo in moduleDirInfos)
            {
                assetsTreeViewList.Add(new TreeViewItem(index++, 1, moduleDirInfo.Name));
                GetCurrDir(moduleDirInfo, assetsTreeViewList, 2, ref index);
            }

            return assetsTreeViewList;
        }

        private void GetCurrDir(DirectoryInfo directoryInfo, List<TreeViewItem> lst, int depth, ref int index)
        {
            //处理文件
            var fileInfos = directoryInfo.GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(".meta")) continue;
                lst.Add(new TreeViewItem(index++, depth, fileInfo.Name));
            }

            //处理文件夹(递归)
            var dirInfos = directoryInfo.GetDirectories();
            foreach (var dirInfo in dirInfos)
            {
                lst.Add(new TreeViewItem(index++, depth, dirInfo.Name));
                GetCurrDir(dirInfo, lst, depth + 1, ref index);
            }
        }
    }

    public class GAssetsTreeView : TreeView
    {
        private readonly List<TreeViewItem> _treeViewItems;

        public GAssetsTreeView(TreeViewState state, List<TreeViewItem> item) : base(state)
        {
            _treeViewItems = item;
            Reload();
        }

        public TreeViewItem GetItemByID(int id) => _treeViewItems[id - 1];

        public void AddItem(TreeViewItem item) => _treeViewItems.Add(item);

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem {id = 0, depth = -1, displayName = "Root"};
            SetupParentsAndChildrenFromDepths(root, _treeViewItems);
            return root;
        }
    }
}