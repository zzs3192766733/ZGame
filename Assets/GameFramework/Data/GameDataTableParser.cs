//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/23 14:44:22
//========================================================

using System;
using System.Collections.Generic;
using System.IO;
using GameFramework.Common;

namespace GameFramework.Data
{
	
	public class GameDataTableParser : IDisposable
	{
		private Dictionary<string, int> _fieldNameDic;
		private int m_Row;
		private int m_Column;
		private string[,] m_GameData;
		private string[] m_FieldName;
		private int m_CurRowNo = 3;
		public bool Eof => m_CurRowNo == m_Row;

		public GameDataTableParser(string path)
		{
			_fieldNameDic = new Dictionary<string, int>();
			byte[] buffer = null;

			using (FileStream fs = new FileStream(path, FileMode.Open))
			{
				buffer = new byte[fs.Length];
				fs.Read(buffer, 0, buffer.Length);
			}
			
			using (var ms = new GameMemoryStream(buffer))
			{
				m_Row = ms.ReadInt();
				m_Column = ms.ReadInt();

				m_GameData = new string[m_Row, m_Column];
				m_FieldName = new string[m_Column];

				for (var i = 0; i < m_Row; i++)
				{
					for (var j = 0; j < m_Column; j++)
					{
						var str = ms.ReadString();

						if (i == 0)
						{ 
							//表示读取的是字段
							m_FieldName[j] = str;
							_fieldNameDic[str] = j;

						}
						else if (i > 2)
						{
							//表示读取的是内容
							m_GameData[i, j] = str;
						}
					}
				}
			}
		}
		
		public void Next()
		{
			if (Eof) return;
			m_CurRowNo++;
		}
		
		public string GetFieldValue(string fieldName)
		{
			try
			{
				if (m_CurRowNo < 3 || m_CurRowNo >= m_Row) return null;
				return m_GameData[m_CurRowNo, _fieldNameDic[fieldName]];
			}
			catch { return null; }
		}
		
		public void Dispose()
		{
			_fieldNameDic.Clear();
			_fieldNameDic = null;

			m_FieldName = null;
			m_GameData = null;
		}
	}
}
