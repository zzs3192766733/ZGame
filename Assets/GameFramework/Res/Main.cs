//========================================================
// 描述:Main游戏入口，需要手动进行挂载的单例
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/24 8:23:30
//========================================================

using System;
using GameFramework.Common;
using UnityEngine;
namespace GameFramework.Res
{
	
	public class Main : MonoBehaviour
	{
		public static Main Instance;
		private async void Awake()
		{
			Instance = this;
			DontDestroyOnLoad(this.gameObject);

			var module = new ModuleConfig
			{
				ModuleName = "AModule",
				ModuleVersion = "1.0.0"
			};

			if (await ModuleManager.Instance.Load(module))
			{
				GameLogger.Log("加载模块完成!!!");
				await AssetLoader.Instance.CloneAsync("AModule", "Assets/GAssets/AModule/Module/Cube.prefab");
				GameLogger.Log("物体异步加载完成");
			}
		}
	}
}
