//========================================================
// 描述:Asset依赖树
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/25 9:28:19
//========================================================

using System.Collections.Generic;
using UnityEngine;
namespace GameFramework.Res
{
	
	public class AssetRef
	{
		public AssetInfo SelfAssetInfo;
		public BundleRef SelfBundleRef;
		public BundleRef[] Dependencies;
		public Object Asset;
		public bool IsGameObject;
		public List<GameObject> Children;

		public AssetRef(AssetInfo info)
		{
			SelfAssetInfo = info;
		}
	}
}
