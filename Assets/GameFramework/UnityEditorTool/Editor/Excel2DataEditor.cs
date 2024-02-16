//========================================================
// 描述:操作Excel文档生成Data文件
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/12 13:06:24
//========================================================

using System;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using GameFramework.Common;
using OfficeOpenXml;

namespace GameFramework.UnityEditorTool.Editor
{
    public class Excel2DataEditor : EditorWindow
    {
        private static string _excelPath;
        private static string _dataPath;
        private static string _entityPath;
        private static string _modelPath;
        private static Dictionary<string, Excel> _name2Excel;
        private static List<string> _names;
        private int _currSelectExcelIndex = 1;
        private int _currSelectSheetIndex = 1;
        private bool _isLock = false;

        private void OnEnable()
        {
            _excelPath = Application.dataPath + "/GameFramework/Data/Excel";
            _dataPath = Application.dataPath + "/GameFramework/Data/CreateData";
            _entityPath = Application.dataPath + "/GameFramework/Data/Entity";
            _modelPath = Application.dataPath + "/GameFramework/Data/Model";
            _name2Excel = new Dictionary<string, Excel>();
            _names = new List<string>();
            RefreshData();
        }

        [MenuItem("GameFramework/Excel2Data(读取Excel表格)", priority = -100)]
        private static void OpenExcel2DataWindow()
        {
            GetWindow<Excel2DataEditor>($"Excel表格工具({GameFrameworkGlobalConfig.GameFrameworkVersion})").Show();
            RefreshData();
        }
        
        private Vector2 _pos;
        private Vector2 _windowPos;
        private GUIContent _selectExcelGUIContent;
        private GUIContent _selectSheetGUIContent;

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            if (_name2Excel != null && _name2Excel.Count > 0 && _currSelectExcelIndex > 0)
            {
                if (_currSelectExcelIndex > _name2Excel.Count) return;
                var excel = _name2Excel[_names[_currSelectExcelIndex - 1]];

                #region 最上面的下拉列表和按钮

                GUILayout.Space(10);
                GUILayout.BeginHorizontal(GUI.skin.box);
                {
                    if (_selectExcelGUIContent == null)
                        _selectExcelGUIContent = new GUIContent("Select Excel");
                    _selectExcelGUIContent.text = _names[_currSelectExcelIndex - 1];
                    var r1 = GUILayoutUtility.GetRect(_selectExcelGUIContent, EditorStyles.toolbarDropDown,
                        GUILayout.Width(200));
                    if (EditorGUI.DropdownButton(r1, _selectExcelGUIContent, FocusType.Passive,
                        EditorStyles.toolbarDropDown))
                    {
                        var menu = new GenericMenu();
                        for (var i = 1; i <= _name2Excel.Count; i++)
                        {
                            menu.AddItem(new GUIContent(_names[i - 1]), i == _currSelectExcelIndex,
                                (index) =>
                                {
                                    _currSelectExcelIndex = (int) index;
                                    _currSelectSheetIndex = 1;
                                }, i);
                        }

                        menu.DropDown(r1);
                    }

                    if (_selectSheetGUIContent == null)
                        _selectSheetGUIContent = new GUIContent($"当前选择Sheet:{_currSelectSheetIndex}");
                    _selectSheetGUIContent.text = $"当前选择Sheet:{_currSelectSheetIndex}";
                    var r2 = GUILayoutUtility.GetRect(_selectSheetGUIContent, EditorStyles.toolbarDropDown,
                        GUILayout.Width(200));
                    if (EditorGUI.DropdownButton(r2, _selectSheetGUIContent, FocusType.Passive,
                        EditorStyles.toolbarDropDown))
                    {
                        var menu = new GenericMenu();
                        var currExcel = _name2Excel[_names[_currSelectExcelIndex - 1]];
                        for (var i = 1; i <= currExcel.Cells.Count; i++)
                        {
                            menu.AddItem(new GUIContent($"Sheet{i}"), i == _currSelectSheetIndex,
                                (index) => _currSelectSheetIndex = (int) index, i);
                        }

                        menu.DropDown(r2);
                    }

                    GUILayout.FlexibleSpace();
                    
                    if(GUILayout.Button("打开Data数据读取窗口"))
                        GetWindow<DataFileDragWindow>("读取Data数据面板").Show();

                    if (GUILayout.Button("导出新的Excel文件"))
                    {
                        var excelOutPath = _excelPath + "/" + _names[_currSelectExcelIndex - 1].Replace(".xlsx", "") +
                                           "_new.xlsx";
                        ExportNewExcel(excelOutPath);
                        AssetDatabase.Refresh();
                        ShowNotification(new GUIContent("导出新的Excel成功!"));
                    }

                    if (GUILayout.Button("导出Data文件"))
                    {
                        ExportData();
                    }

                    if (GUILayout.Button("打开Excel文件"))
                    {
                        OpenExcel(_excelPath + "/" + _names[_currSelectExcelIndex - 1]);
                    }

                    if (GUILayout.Button("刷新数据"))
                    {
                        ShowNotification(RefreshData() ? new GUIContent("刷新成功!") : new GUIContent("刷新失败!"));
                    }

                    if (GUILayout.Button("删除Excel"))
                    {
                        if (EditorUtility.DisplayDialog("提示", $"是否删除{_names[_currSelectExcelIndex - 1]}Excel文件", "确定",
                            "取消"))
                        {
                            var deletePath = _excelPath + "/" + _names[_currSelectExcelIndex - 1];
                            DeleteExcel(deletePath);
                            AssetDatabase.Refresh();
                            RefreshData();
                            ShowNotification(new GUIContent("删除Excel成功!"));
                            if (_name2Excel == null || _name2Excel.Count <= 0)
                                _currSelectExcelIndex = 0;
                            else
                                _currSelectExcelIndex = 1;
                        }
                    }

                    _isLock = GUILayout.Toggle(_isLock, "锁定");
                }
                GUILayout.EndHorizontal();

                #endregion

                #region 绘制标题

                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(_names[_currSelectExcelIndex - 1]);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                #endregion

                GUILayout.BeginHorizontal("Box");

                #region 绘制每一行信息(滚动条视图)

                _pos = GUILayout.BeginScrollView(_pos);
                {
                    //绘制每一行信息(i从3开始，是为了排除前3行)
                    for (var i = 3; i < excel.Cells[_currSelectSheetIndex - 1].GetLength(0); i++)
                    {
                        GUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            for (var j = 0; j < excel.Cells[_currSelectSheetIndex - 1].GetLength(1); j++)
                            {
                                var label = excel.Cells[_currSelectSheetIndex - 1][i, j];
                                var valName = excel.Cells[_currSelectSheetIndex - 1][0, j];
                                if (!_isLock)
                                {
                                    GUILayout.BeginHorizontal();
                                    {
                                        GUILayout.Label(valName + ":", GUILayout.Width(50));
                                        excel.Cells[_currSelectSheetIndex - 1][i, j] =
                                            GUILayout.TextField(label, GUILayout.Width(200));
                                    }
                                    GUILayout.EndHorizontal();
                                }
                                else
                                {
                                    GUILayout.BeginHorizontal();
                                    {
                                        GUILayout.Label(valName + ":", GUILayout.Width(50));
                                        GUILayout.Label(label, GUILayout.Width(200));
                                    }
                                    GUILayout.EndHorizontal();
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();

                #endregion

                #region 导出Data数据配置信息

                GUILayout.BeginVertical("Box");
                {
                    _windowPos = GUILayout.BeginScrollView(_windowPos, GUI.skin.window, GUILayout.MinWidth(400));
                    {
                        //标题
                        GUILayout.BeginHorizontal("Box");
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("Data数据配置信息");
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                        //变量信息
                        GUILayout.BeginVertical("Box");
                        {
                            GUILayout.Label($"一共包含Sheet:{excel.Cells.Count}页");
                            GUILayout.Space(10);
                            for (var i = 0; i < excel.Cells[_currSelectSheetIndex - 1].GetLength(1); i++)
                            {
                                var valName = excel.Cells[_currSelectSheetIndex - 1][0, i];
                                var valType = excel.Cells[_currSelectSheetIndex - 1][1, i];
                                var valInfo = excel.Cells[_currSelectSheetIndex - 1][2, i];
                                GUILayout.BeginHorizontal("Box");
                                GUILayout.Label($"变量名称:{valName}\t变量类型:{valType}\t变量信息:{valInfo}");
                                GUILayout.EndHorizontal();
                            }
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndScrollView();
                    //右侧下方提示
                    GUILayout.Label("使用方法");
                    GUILayout.Label("Excel文件，对应的前三行分别代表的含义:");
                    GUILayout.Label("1.变量的名称");
                    GUILayout.Label("2.变量的类型");
                    GUILayout.Label("3.变量的介绍(注释)");
                }
                GUILayout.EndVertical();

                #endregion

                GUILayout.EndHorizontal();

                #region 最下面信息条

                //绘制版权信息
                GameFrameworkGUIHelper.DrawCopyright();

                #endregion
            }
        }

        private void DeleteExcel(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        private void ExportNewExcel(string path)
        {
            using (var excelPack = new ExcelPackage(new FileInfo(path)))
            {
                var excel = _name2Excel[_names[_currSelectExcelIndex - 1]];
                for (var i = 0; i < excel.Cells.Count; i++)
                {
                    var workSheet = excelPack.Workbook.Worksheets.Add($"Sheet{i + 1}");
                    for (var j = 0; j < excel.Cells[i].GetLength(0); j++)
                    {
                        for (var k = 0; k < excel.Cells[i].GetLength(1); k++)
                        {
                            workSheet.Cells[j + 1, k + 1].Value = excel.Cells[i][j, k];
                        }
                    }
                }

                excelPack.Save();
            }
        }

        /// <summary>
        /// 根据Excel表格导出Data数据
        /// </summary>
        private void ExportData()
        {
            //先刷新数据
            RefreshData();
            string[,] dataArr = null;
            string fileName = null;
            //导出Data数据
            using (var ms = new GameMemoryStream())
            {
                var excel = _name2Excel[_names[_currSelectExcelIndex - 1]];
                var currExcelSheet = excel.Cells[_currSelectSheetIndex - 1];
                var row = currExcelSheet.GetLength(0);
                var col = currExcelSheet.GetLength(1);
                ms.WriteInt(row);
                ms.WriteInt(col);
                dataArr = new string[col, 3];
                for (var i = 0; i < row; i++)
                {
                    for (var j = 0; j < col; j++)
                    {
                        if (i < 3)
                            dataArr[j, i] = currExcelSheet[i, j];
                        ms.WriteString(currExcelSheet[i, j]);
                    }
                }

                var buffer = ms.ToArray();
                fileName = _names[_currSelectExcelIndex - 1].Replace(".xlsx", "") + "_" +
                           (_currSelectSheetIndex - 1);
                var path = _dataPath + "/" + fileName + ".data";

                if (File.Exists(path))
                    File.Delete(path);
                using (var fs =
                    new FileStream(path
                        , FileMode.Create))
                {
                    fs.Write(buffer, 0, buffer.Length);
                }

                AssetDatabase.Refresh();
                ShowNotification(new GUIContent("导出Data数据成功!!!"));
            }

            //创建脚本前先关闭自动创建脚本的编辑器脚本
            //1.创建实体类(Entity)
            CreateEntityClass(fileName,dataArr);
            AssetDatabase.Refresh();
            //2.创建数据模型类(Model)
            CreateModelClass(fileName,dataArr);
            AssetDatabase.Refresh();
        }

        private void CreateEntityClass(string fileName, string[,] dataArr)
        {
            var sbr = new StringBuilder();
            if (!Directory.Exists(_entityPath))
                Directory.CreateDirectory(_entityPath);
            
            sbr.Append("\r\n");
            sbr.Append("//====================================================\r\n");
            sbr.Append("//该脚本为实体(Entity)脚本，由工具生成，切勿更改\r\n");
            sbr.Append("//该脚本为实体(Entity)脚本，由工具生成，切勿更改\r\n");
            sbr.Append("//该脚本为实体(Entity)脚本，由工具生成，切勿更改\r\n");
            sbr.Append("//====================================================\r\n");
            sbr.Append("\r\n");
            sbr.Append("/// <summary>\r\n");
            sbr.Append($"/// {fileName}实体类\r\n");
            sbr.Append("/// </summary>\r\n");
            sbr.Append($"public partial class {fileName}Entity : AbstractEntity\r\n");
            sbr.Append("{\r\n");
            
            //循环比例所有属性
            for (var i = 0; i < dataArr.GetLength(0); i++)
            {
                if (i == 0) continue;
                sbr.Append("    /// <summary>\r\n");
                sbr.Append($"    /// {dataArr[i,2]}\r\n");
                sbr.Append("    /// </summary>\r\n");
                sbr.Append($"    public {dataArr[i,1]} {dataArr[i,0]} {{ get; set; }}\r\n");
                sbr.Append("\r\n");
            }
            
            sbr.Append("}\r\n");
            var path = _entityPath+$"/{fileName}Entity.cs";
            if (File.Exists(path))
            {
                File.Delete(path);
                File.Delete(path+".meta");
            }
            using (var sw = new StreamWriter(path))
            {
                sw.Write(sbr.ToString());
            }
        }

        private void CreateModelClass(string fileName, string[,] dataArr)
        {
            var sbr = new StringBuilder();
            if (!Directory.Exists(_modelPath))
                Directory.CreateDirectory(_modelPath);
            sbr.Append("using GameFramework.ClassExt;\r\n");
            sbr.Append("using GameFramework.Data.Entity;\r\n");
            sbr.Append("//====================================================\r\n");
            sbr.Append("//该脚本为数据模型(DBModel)脚本，由工具生成，切勿更改\r\n");
            sbr.Append("//该脚本为数据模型(DBModel)脚本，由工具生成，切勿更改\r\n");
            sbr.Append("//该脚本为数据模型(DBModel)脚本，由工具生成，切勿更改\r\n");
            sbr.Append("//====================================================\r\n");
            sbr.Append("\r\n");
            sbr.Append("/// <summary>\r\n");
            sbr.Append($"/// {fileName}DBModel类\r\n");
            sbr.Append("/// </summary>\r\n");
            sbr.Append($"public partial class {fileName}DBModel : AbstractDBModel<{fileName}DBModel,{fileName}Entity>\r\n");
            sbr.Append("{\r\n");
            sbr.Append($"    protected override string FileName => \"{fileName}.data\";\r\n");
            sbr.Append($"    protected override {fileName}Entity MakeData(GameDataTableParser parser)\r\n");
            sbr.Append("    {\r\n");
            sbr.Append($"        var entity = new {fileName}Entity();\r\n");
            for (var i = 0; i < dataArr.GetLength(0); i++)
            {
                sbr.Append($"        entity.{dataArr[i,0]} = parser.GetFieldValue(\"{dataArr[i,0]}\"){StringToType(dataArr[i,1])};\r\n");
            }
            sbr.Append("        return entity;\r\n");
            sbr.Append("    }\r\n");
            sbr.Append("}\r\n");
            var path = _modelPath + $"/{fileName}DBModel.cs";
            if (File.Exists(path))
            {
                File.Delete(path);
                File.Delete(path+".meta");
            }
            using (var sw = new StreamWriter(path))
            {
                sw.Write(sbr.ToString());
            }
        }

        private string StringToType(string str)
        {
            var typeStr = string.Empty;
            switch (str.ToLower())
            {
                case "int":
                    typeStr = ".ToInt()";
                    break;
                case "float":
                    typeStr = ".ToFloat()";
                    break;
                case "bool":
                    typeStr = ".ToBool()";
                    break;
                case "short":
                    typeStr = ".ToShort()";
                    break;
                case "long":
                    typeStr = ".ToLong()";
                    break;
            }
            return typeStr;
        }

        private void OpenExcel(string path) => Application.OpenURL(path);

        private static bool RefreshData()
        {
            _name2Excel.Clear();
            _names.Clear();
            var fileInfos = new DirectoryInfo(_excelPath).GetFiles();
            if (fileInfos.Length <= 0) return false;
            foreach (var fileInfo in fileInfos)
            {
                if (!fileInfo.FullName.EndsWith(".xlsx") || fileInfo.FullName.Contains("$")) continue;
                //依次打开Excel文件，对文件内容进行保存
                try
                {
                    var excelPack = new ExcelPackage(fileInfo);
                    var sheetCount = excelPack.Workbook.Worksheets.Count;
                    var excel = new Excel();

                    for (var k = 1; k <= sheetCount; k++)
                    {
                        var sheet = excelPack.Workbook.Worksheets[k];
                        var row = sheet.Dimension.End.Row;
                        var col = sheet.Dimension.End.Column;

                        var arr = new string[row, col];
                        for (var i = 1; i <= row; i++)
                        {
                            for (var j = 1; j <= col; j++)
                            {
                                arr[i - 1, j - 1] = sheet.Cells[i, j].Value.ToString();
                            }
                        }

                        excel.Cells.Add(arr);
                    }

                    _names.Add(fileInfo.Name);
                    _name2Excel.Add(fileInfo.Name, excel);
                    excelPack.Dispose();
                }
                catch (Exception)
                {
                    GameLogger.LogError($"由于Excel已经打开，导致无法读写或Excel文件出现空行现象({fileInfo.FullName})，请修改后尝试", DebugColor.Red);
                    return false;
                }
            }

            return true;
        }

        private class Excel
        {
            public readonly List<string[,]> Cells;

            public Excel()
            {
                Cells = new List<string[,]>();
            }
        }
    }
}