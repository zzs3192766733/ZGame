//========================================================
// 描述:将此类挂载到LoadingScene中
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/12 12:24:30
//========================================================

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameFramework.LoadScene
{
	/// <summary>
	/// 场景加载
	/// </summary>
	public class LoadScene : MonoBehaviour
	{
		private float tarValue;
		private float currValue;
		//===================根据所用UI系统更换=====================
		[SerializeField] private Slider _slider;
		[SerializeField] private Text _text;
		//=========================================================
		[SerializeField] private GameObject _uiRoot;//UI根部
	    private void Start()
	    {
		    StartCoroutine(LoadAsyncSceneIEnumerator());
	    }
	    
	    private IEnumerator LoadAsyncSceneIEnumerator()
	    {
		    SceneManager.Instance.UIObject = _uiRoot;
		    var loadSceneAsync = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneManager.Instance.CurrSceneName,LoadSceneMode.Additive);
		    loadSceneAsync.allowSceneActivation = false;
		    while (loadSceneAsync.progress<0.9f)
		    {
			    tarValue = loadSceneAsync.progress * 100f;

			    while (currValue<tarValue)
			    {
				    currValue++;
				    _slider.value = currValue / 100f;
				    _text.text = $"当前加载进度:{currValue}%";
				    yield return null;
			    }

			    yield return null;
		    }

		    tarValue = 100f;
		    while (currValue<tarValue)
		    {
			    currValue++;
			    _slider.value = currValue / 100f;
			    _text.text = $"当前加载进度:{currValue}%";
			    yield return null;
		    }
		    loadSceneAsync.allowSceneActivation = true;
	    }
	}
}
