//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/12 12:52:52
//========================================================

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFramework.UI.GameUIToolTips
{
	
	public class ToolTipsTrigger : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
	{
	    [SerializeField] private string _headerText;
	    [SerializeField][Multiline] private string _infoText;
	    public void OnPointerEnter(PointerEventData eventData)
	    {
	        StartCoroutine(nameof(DelayShowTips));
	    }
	
	    private IEnumerator DelayShowTips()
	    {
	        yield return new WaitForSeconds(0.8f);
	        ToolTipsSystem.instance.Show(_infoText, _headerText);
	    }
	
	    public void OnPointerExit(PointerEventData eventData)
	    {
	        StopCoroutine(nameof(DelayShowTips));
	        ToolTipsSystem.instance.Hide();
	    }
	
	    private void OnMouseEnter()
	    {
	        StartCoroutine(nameof(DelayShowTips));
	    }
	
	    private void OnMouseExit()
	    {
	        StopCoroutine(nameof(DelayShowTips));
	        ToolTipsSystem.instance.Hide();
	    }
	}
}
