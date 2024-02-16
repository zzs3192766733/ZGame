//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/30 14:55:42
//========================================================

using UnityEngine;
namespace GameFramework.ClassExt
{
	
	public static class GameObjectExt
	{
		public static T AddOrGetComponent<T>(this GameObject obj)where T: Component
		{
			var com = obj.GetComponent<T>();
			if (com == null)
				com = obj.AddComponent<T>();
			return com;
		}
	}
}
