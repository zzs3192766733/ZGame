//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/12 13:00:25
//========================================================

using System.Collections.Generic;
using UnityEngine;
namespace GameFramework.UI.UGUI
{
	using System.Linq;
	
	public class UIManager : MonoBehaviour
	{
	    public static UIManager instance;
	    private readonly Stack<UIViewBase> _stackAllUIViewBases = new Stack<UIViewBase>();
	    public int SortOrder => _stackAllUIViewBases.Count;
	
	    private void Awake()
	    {
	        instance = this;
	    }
	    
	
	    public void PushUI(UIViewBase uiView)
	    {
	        if (_stackAllUIViewBases.Any(item => item.Equals(uiView)))
	        {
	            var viewArr = _stackAllUIViewBases.ToArray();
	            var length = SortOrder;
	            _stackAllUIViewBases.Clear();
	            var tmp = viewArr.Length - 1;
	            for (var i = viewArr.Length - 1; i >= 0; i--)
	            {
	                if (!viewArr[i].Equals(uiView))
	                {
	                    _stackAllUIViewBases.Push(viewArr[i]);
	                    viewArr[i].SetSortOrder(length-tmp--);
	                }
	            }
	            _stackAllUIViewBases.Push(uiView);
	        }
	        else
	            _stackAllUIViewBases.Push(uiView);
	    }
	
	    public void PopUI()
	    {
	        if (_stackAllUIViewBases.Count>0)
	            _stackAllUIViewBases.Pop();
	    }
	
	    public UIViewBase GetCurrUIViewBase()
	    {
	        return _stackAllUIViewBases.Count > 0 ? _stackAllUIViewBases.Peek() : null;
	    }
	}
}
