//========================================================
// 描述:Bundle依赖树
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/25 9:28:00
//========================================================

using System.Collections.Generic;
using UnityEngine;
namespace GameFramework.Res
{
	
	public class BundleRef
	{
		public BundleInfo SelfBundleInfo;
		public AssetBundle SelfAssetBundle;
		public List<AssetRef> AssetRefs;

		public BundleRef(BundleInfo info)
		{
			SelfBundleInfo = info;
		}
	}
}
