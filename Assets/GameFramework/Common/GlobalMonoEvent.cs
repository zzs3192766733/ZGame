//========================================================
// 描述:当某个类没有继承Mono时，使用GlobalMonoEvent进行Mono生命周期的注册
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/11 9:30:05
//========================================================

using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
namespace GameFramework.Common
{
	
	public class GlobalMonoEvent : SingleMonoBase<GlobalMonoEvent>
	{
		private event Action _globalAwakeEvent;
		private event Action _globalStartEvent;
		private event Action _globalUpdateEvent;
		private void Awake()
		{
			_globalAwakeEvent?.Invoke();
		}

		private void Start()
	    {
		    _globalStartEvent?.Invoke();
	    }
	
	    private void Update()
	    {
		    _globalUpdateEvent?.Invoke();
	    }
	    

	    public void AddAwakeEvent(Action action)
	    {
		    _globalAwakeEvent += action;
	    }
	    public void AddStartEvent(Action action)
	    {
		    _globalStartEvent += action;
	    }
	    public void AddUpdateEvent(Action action)
	    {
		    _globalUpdateEvent += action;
	    }

	    public void ClearAwakeEvent()
	    {
		    if (_globalAwakeEvent == null) return;
		    var arr = _globalAwakeEvent.GetInvocationList();
		    foreach (var del in arr)
			    _globalAwakeEvent -= del as Action;
	    }

	    public void ClearStartEvent()
	    {
		    if (_globalStartEvent == null) return;
		    var arr = _globalStartEvent.GetInvocationList();
		    foreach (var del in arr)
			    _globalStartEvent -= del as Action;
	    }

	    public void ClearUpdateEvent()
	    {
		    if (_globalUpdateEvent == null) return;
		    var arr = _globalUpdateEvent.GetInvocationList();
		    foreach (var del in arr)
			    _globalUpdateEvent -= del as Action;
	    }

	    public void ClearAllEvent()
	    {
		    ClearAwakeEvent();
		    ClearStartEvent();
		    ClearUpdateEvent();
	    }
	}
}
