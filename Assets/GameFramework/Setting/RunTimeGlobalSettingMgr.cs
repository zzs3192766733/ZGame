//========================================================
// 描述:游戏运行全局设置
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2022/1/17 7:41:10
//========================================================
using GameFramework.Common;
using UnityEngine;
namespace GameFramework.Setting
{
	public class RunTimeGlobalSettingMgr : SingleBase<RunTimeGlobalSettingMgr>
	{
		public void SetScreenResolution(Resolution resolution)
		{
			Screen.SetResolution(resolution.width,resolution.height,true,resolution.refreshRate);
		}
		public Resolution GetScreenResolution() => Screen.currentResolution;

		public void SetFullScreen()
		{
			Screen.fullScreen = true;
		}

		public void SetWindowScreen()
		{
			Screen.fullScreen = false;
		}
	}
}
