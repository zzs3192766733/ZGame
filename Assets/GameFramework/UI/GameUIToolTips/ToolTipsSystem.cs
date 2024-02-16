//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/12 12:52:52
//========================================================

using UnityEngine;
namespace GameFramework.UI.GameUIToolTips
{
	
	public class ToolTipsSystem : MonoBehaviour
	{
	    public static ToolTipsSystem instance;
	
	    public ToolTips _Tips;
	    
	    private void Awake()
	    {
	        instance = this;
	    }
	
	    public void Show(string infoText,string headerText = "")
	    {
	        _Tips.gameObject.SetActive(true);
	        _Tips.SetText(headerText,infoText);
	    }
	
	    public void Hide()
	    {
	        _Tips.gameObject.SetActive(false);
	    }
	}
}
