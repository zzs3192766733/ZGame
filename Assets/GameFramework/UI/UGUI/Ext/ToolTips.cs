//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/26 9:29:00
//========================================================

using UnityEngine;
namespace GameFramework.UI.UGUI.Ext
{
	using System;
	using TMPro;
	using UnityEngine.UI;
	
	public class ToolTips : MonoBehaviour
	{
	    public TextMeshProUGUI _headerText;
	    public TextMeshProUGUI _infoText;
	    [SerializeField] private int _maxLength;
	    public LayoutElement _layoutElement;
	    private RectTransform _rectTransform;
	
	    private void Start()
	    {
	        _rectTransform = GetComponent<RectTransform>();
	    }
	
	    public void SetText(string headerStr, string infoText)
	    {
	        if (string.IsNullOrEmpty(headerStr))
	        {
	            _headerText.gameObject.SetActive(false);
	        }
	        else
	        {
	            _headerText.gameObject.SetActive(true);
	            _headerText.text = headerStr;
	        }
	
	        _infoText.text = infoText;
	        
	        var headerLength = _headerText.text.Length;
	        var infoLength = _infoText.text.Length;
	        _layoutElement.enabled = headerLength > _maxLength || infoLength > _maxLength;
	    }
	
	    private void Update()
	    {
	        transform.position = Input.mousePosition;
	        
	        var v = new Vector2(Input.mousePosition.x/Screen.width,Input.mousePosition.y/Screen.height);
	
	        _rectTransform.pivot = v;
	    }
	}
}
