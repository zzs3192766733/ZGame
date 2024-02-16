//========================================================
// 描述:工具函数
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/17 10:31:25
//========================================================

using System.IO;
using Newtonsoft.Json;

namespace GameFramework.Tool
{
    public static class ToolUtility
    {
        /// <summary>
        /// 格式化json
        /// </summary>
        /// <param name="str">输入json字符串</param>
        /// <returns>返回格式化后的字符串</returns>
        public static string ConvertJsonString(string str)
        {
            JsonSerializer serializer = new JsonSerializer();

            TextReader tr = new StringReader(str);

            JsonTextReader jtr = new JsonTextReader(tr);

            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();

                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,

                    Indentation = 4,

                    IndentChar = ' '
                };

                serializer.Serialize(jsonWriter, obj);

                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }
    }
}