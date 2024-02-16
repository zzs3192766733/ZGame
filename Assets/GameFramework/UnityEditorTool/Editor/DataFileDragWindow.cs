//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/23 8:24:48
//========================================================

using System;
using System.IO;
using GameFramework.Common;
using UnityEngine;
using UnityEditor;

namespace GameFramework.UnityEditorTool.Editor
{
    public class DataFileDragWindow : EditorWindow
    {
        [MenuItem("GameFramework/ReadDataFile(读取Data文件)",priority = -99)]
        private static void Open()
        {
            GetWindow<DataFileDragWindow>("读取Data数据面板").Show();
        }

        private string[,] _excelStrArr = null;
        private Vector2 _vector2;

        private void OnGUI()
        {
            var tmpObj = DragAreaGetObject.GetObject((path) => path.EndsWith(".data"), this, "拖拽Data文件到这里");
            if (tmpObj != null)
            {
                try
                {
                    var path = Application.dataPath.Replace("Assets","") + AssetDatabase.GetAssetPath(tmpObj);
                    ReadData(path);
                }
                catch (Exception)
                {
                    ShowNotification(new GUIContent("读取Data文件失败"));
                    return;
                }
                ShowNotification(new GUIContent("读取Data文件成功"));
            }
            DrawExcel();
            GUILayout.FlexibleSpace();
            GameFrameworkGUIHelper.DrawCopyright();
        }

        private void OnInspectorUpdate()
        {
            this.Repaint();
        }

        private void ReadData(string path)
        {
            var fs = new FileStream(path, FileMode.Open);
            var buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            using (var ms = new GameMemoryStream(buffer))
            {
                var row = ms.ReadInt();
                var col = ms.ReadInt();
                _excelStrArr = new string[row,col];
                for (var i = 0; i < row; i++)
                {
                    for (var j = 0; j < col; j++)
                    {
                        _excelStrArr[i, j] = ms.ReadString();
                    }
                }
            }
        }
        private void DrawExcel()
        {
            if (_excelStrArr == null) return;
            _vector2 = GUILayout.BeginScrollView(_vector2);
            {
                for (var i = 0; i < _excelStrArr.GetLength(0); i++)
                {
                    GUILayout.BeginHorizontal("Box");
                    {
                        for (var j = 0; j < _excelStrArr.GetLength(1); j++)
                        {
                            GUILayout.Label(_excelStrArr[i,j],GUILayout.Width(150));
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
        }
    }
}