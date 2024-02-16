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
	using System;
	using UnityEngine.UI;
	
	public abstract class UIViewBase : MonoBehaviour,IDisposable
	{
	    private readonly Dictionary<string, Component> allComponents = new Dictionary<string, Component>();
	    protected abstract void OnStart();
	    protected abstract void OnAwake();
	    protected abstract void OnUpdate();
	    protected abstract void RefreshUI();
	    protected abstract void InitComponent();
	
	    protected virtual Action<Transform> howToShowUI { get; set; }
	    protected virtual Action<Transform> howToCloseUI { get; set; }
	
	
	    private Canvas _selfCanvas;
	    protected RectTransform selfRectTrans;
	
	    private void Start()
	    {
	        OnStart();
	    }
	    private void Awake()
	    {
	        FindRectTrans();
	        InitComponent();
	        OnAwake();
	    }
	
	    private void Update()
	    {
	        OnUpdate();
	    }
	
	    private void FindRectTrans() => selfRectTrans = GetOrCreateComponent<RectTransform>();
	
	    private T FindComponent<T>(string componentPath) where T : Component
	    {
	        return transform.Find(componentPath).GetComponent<T>();
	    }
	
	    public void SetSortOrder(int order = -1)
	    {
	        if (_selfCanvas == null)
	        {
	            _selfCanvas = GetOrCreateComponent<Canvas>();
	            GetOrCreateComponent<GraphicRaycaster>();
	        }
	        _selfCanvas.overrideSorting = true;
	        _selfCanvas.sortingOrder = order == -1 ? UIManager.instance.SortOrder : order;
	    }
	    
	
	    public virtual void ShowUI()
	    {
	        RefreshUI();
	        UIManager.instance.PushUI(this);
	        SetSortOrder();
	        if(howToShowUI==null)
	            this.gameObject.SetActive(true);
	        else howToShowUI?.Invoke(transform);
	    }
	
	    public virtual void CloseUI()
	    {
	        UIManager.instance.PopUI();
	        SetSortOrder(0);
	        if (howToCloseUI == null)
	            this.gameObject.SetActive(false);
	        else howToCloseUI.Invoke(transform);
	    }
	    
	
	    protected T CacheComponent<T>(string componentPath) where T : Component
	    {
	        if (!allComponents.ContainsKey(componentPath))
	        {
	            var t = FindComponent<T>(componentPath);
	            if(t!=null)
	                allComponents.Add(componentPath,FindComponent<T>(componentPath));
	        }
	
	        return allComponents[componentPath] as T;
	    }
	
	    protected T GetCacheComponent<T>(string componentPath)where T : UnityEngine.Component
	    {
	        if (allComponents.ContainsKey(componentPath))
	        {
	            return allComponents[componentPath] as T;
	        }
	        return default;
	    }
	
	    protected T GetOrCreateComponent<T>() where T : Component
	    {
	        return GetComponent<T>() == null ? this.gameObject.AddComponent<T>() : GetComponent<T>();
	    }
	
	    private void OnDestroy()
	    {
	        howToShowUI = null;
	        howToCloseUI = null;
	    }
	
	    public void Dispose()
	    {
	        allComponents.Clear();
	    }
	}
}
