//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/10 10:53:46
//========================================================

using UnityEngine;
namespace GameFramework.Common
{
	
	public class SingleBase<T> where T:new()
	{
		private static readonly object ObjLock = new object();

		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (ObjLock)
					{
						if (_instance == null)
						{
							_instance = new T();
						}
					}
				}
				return _instance;
			}
		}
	}
}
