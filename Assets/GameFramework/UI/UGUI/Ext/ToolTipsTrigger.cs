//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/26 9:29:00
//========================================================

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace GameFramework.UI.UGUI.Ext
{
	
	public class ToolTipsTrigger : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
	{
	    [SerializeField] private string _headerText;
	    [SerializeField][Multiline] private string _infoText;
	    public void OnPointerEnter(PointerEventData eventData)
	    {
	        StartCoroutine("DelayShowTips");
	    }
	
	    private IEnumerator DelayShowTips()
	    {
	        yield return new WaitForSeconds(0.8f);
	        ToolTipsSystem.instance.Show(_infoText, _headerText);
	    }
	
	    public void OnPointerExit(PointerEventData eventData)
	    {
	        StopCoroutine("DelayShowTips");
	        ToolTipsSystem.instance.Hide();
	    }
	
	    private void OnMouseEnter()
	    {
	        StartCoroutine("DelayShowTips");
	    }
	
	    private void OnMouseExit()
	    {
	        StopCoroutine("DelayShowTips");
	        ToolTipsSystem.instance.Hide();
	    }
	}
}
