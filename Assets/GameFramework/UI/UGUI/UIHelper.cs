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
	public class UIHelper : MonoBehaviour
	{
	    public static UIHelper instance;
	    private Transform _canvasTrans;
	    private readonly Dictionary<UIViewType, UIViewBase> _allUIViewBases = new Dictionary<UIViewType, UIViewBase>();
	    private Dictionary<UIViewType, string> allPath = new Dictionary<UIViewType, string>();
	
	    private void SetAllPath()
	    {
	        allPath.Add(UIViewType.Start,"UI/Start");
	        allPath.Add(UIViewType.Game,"UI/Game");
	        allPath.Add(UIViewType.End,"UI/End");
	    }
	    private void Awake()
	    {
	        instance = this;
	        SetAllPath();
	        _canvasTrans = GameObject.Find("Canvas").transform;
	    }
	    public UIViewBase LoadUIView(UIViewType type)
	    {
	        if (!_allUIViewBases.ContainsKey(type))
	        {
	            var path = allPath[type];
	            var g = Resources.Load<GameObject>(path);
	            if (g == null) return null;
	            var view = Instantiate(g,_canvasTrans);
	            view.transform.localPosition = Vector3.zero;
	            var viewBase = view.GetComponent<UIViewBase>();
	            if (viewBase == null) return null;
	            _allUIViewBases[type] = viewBase;
	            viewBase.ShowUI();
	            return viewBase;
	        }
	        else
	        {
	            _allUIViewBases[type].ShowUI();
	            return _allUIViewBases[type];
	        }
	    }
	
	    public void CloseFirstUIView()
	    {
	        var currViewBase = UIManager.instance.GetCurrUIViewBase();
	        if(currViewBase!=null)
	            currViewBase.CloseUI();
	    }
	    public enum UIViewType
	    {
	        Start,
	        Game,
	        End
	    }
	}
}
