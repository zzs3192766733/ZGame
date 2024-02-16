using System.IO;
using System.Text;
using System;

namespace GameFramework.UnityEditorTool.Editor
{
    public class SpriteTitleChangeEditor : UnityEditor.AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            if (!GameFrameworkGlobalConfig.IsAutoCreateTitle) return;
            path = path.Replace(".meta", "");
            if (!path.EndsWith(".cs")) return;
            var str = new StringBuilder();
            var isFramework = path.StartsWith("Assets/GameFramework");

            using (var sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    var oneStr = sr.ReadLine();
                    if (oneStr == null) continue;
                    if (oneStr.StartsWith("using System.Collections;") ||
                        oneStr.StartsWith("using System.Collections.Generic;") ||
                        oneStr.StartsWith("using UnityEngine;")) continue;
                    str.AppendLine(isFramework?"\t"+oneStr:oneStr);
                }
            }
            var titleText = new StringBuilder();
            titleText.AppendLine("//========================================================");
            titleText.AppendLine("// 描述:");
            titleText.AppendLine("// 创建者:周忠帅");
            titleText.AppendLine("// 联系方式:QQ3192766733");
            titleText.AppendLine($"// 创建时间:{DateTime.Now:G}");
            titleText.AppendLine("//========================================================");
            titleText.AppendLine("");
            titleText.AppendLine("using UnityEngine;");
            if (isFramework)
            {
                //当创建的脚本在GameFramework中时，自动添加命名空间
                var namespaceStr = "namespace GameFramework";
                var arr = path.Split('/');
                for (var i = 2; i < arr.Length - 1; i++)
                {
                    namespaceStr += "." + arr[i];
                }

                titleText.AppendLine(namespaceStr);
                titleText.AppendLine("{");
                titleText.Append(str);
                titleText.AppendLine("}");
            }
            else
                titleText.Append(str);
            File.WriteAllText(path, titleText.ToString());
        }
    }
}