//========================================================
// 描述:Res资源总配置
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/24 8:03:47
//========================================================

using UnityEngine;
namespace GameFramework.Res
{
	public static class ResConfigBase
	{
		public static bool BundleMode { get; }

		static ResConfigBase()
		{
			BundleMode = true;
		}
	}

	public class ModuleConfig
	{
		public string ModuleName;
		public string ModuleVersion;
	}
}
