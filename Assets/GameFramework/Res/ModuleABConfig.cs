//========================================================
// 描述:LitJson序列化出去的类
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/25 8:16:13
//========================================================

using System.Collections.Generic;
using UnityEngine;
namespace GameFramework.Res
{
	public class BundleInfo
	{
		public string BundleName;
		public string CRC;
		public List<string> Assets;
	}

	public class AssetInfo
	{
		public string AssetPath;
		public string BundleName;
		public List<string> Dependencies;
	}
	
	
	public class ModuleABConfig
	{
		public Dictionary<string, BundleInfo> BundleArray;
		public AssetInfo[] AssetArray;

		public ModuleABConfig(){}

		public ModuleABConfig(int assetCount)
		{
			this.AssetArray = new AssetInfo[assetCount];
			BundleArray = new Dictionary<string, BundleInfo>();
		}

		public void AddAsset(int index, AssetInfo assetInfo) => AssetArray[index] = assetInfo;
		public void AddBundle(string bundleName, BundleInfo bundleInfo) => BundleArray[bundleName] = bundleInfo;
	}
}
