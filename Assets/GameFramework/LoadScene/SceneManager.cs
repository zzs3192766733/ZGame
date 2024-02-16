//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/11 15:35:20
//========================================================

using System;
using GameFramework.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace GameFramework.LoadScene
{
	
	public class SceneManager : SingleBase<SceneManager>
	{
		private string _currSceneName;
		public string CurrSceneName => _currSceneName;
		public GameObject UIObject { get; set; }

		private string loadingSceneName => "loadingSceneName";
		public void LoadScene(string sceneName)
		{
			if (_currSceneName == sceneName) return;
			UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
			_currSceneName = sceneName;
		}

		public void LoadSceneAsync(string sceneName)
		{
			//先加载到登陆场景，由登陆场景去管理异步加载的场景
			if (_currSceneName == sceneName) return;
			_currSceneName = sceneName;
			try
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(loadingSceneName);
			}
			catch (Exception)
			{
				GameLogger.LogError($"{loadingSceneName}场景不存在，请创建该场景!",DebugColor.Red);
			}
		}

		public void CloseLoadingUI() => UIObject.SetActive(false);
	}
}
