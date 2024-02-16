//========================================================
// 描述:UI触碰事件监听者
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/17 9:57:42
//========================================================

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
namespace GameFramework.UI.UGUI
{
	public class UIListener : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerDownHandler,IPointerUpHandler
	{
		public delegate void PointerEventDataHandler(PointerEventData eventData);
		public event PointerEventDataHandler ONClick;
		public event PointerEventDataHandler ONEnter;
		public event PointerEventDataHandler ONExit;
		public event PointerEventDataHandler BeginDrag;
		public event PointerEventDataHandler Drag;
		public event PointerEventDataHandler EndDrag;
		public event PointerEventDataHandler ONDown;
		public event PointerEventDataHandler ONUp;
		private GameObject _self;
		
		public void OnPointerClick(PointerEventData eventData)
		{
			ONClick?.Invoke(eventData);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			ONEnter?.Invoke(eventData);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			ONExit?.Invoke(eventData);
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			BeginDrag?.Invoke(eventData);
		}

		public void OnDrag(PointerEventData eventData)
		{
			Drag?.Invoke(eventData);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			EndDrag?.Invoke(eventData);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			ONDown?.Invoke(eventData);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			ONUp?.Invoke(eventData);
		}

		/// <summary>
		/// 只能绑定一次
		/// </summary>
		/// <param name="go"></param>
		public void BindGameObject(GameObject go)
		{
			if (_self != null) return;
			_self = go;
			go.OnDestroyAsObservable().Subscribe(_ =>
			{
				if(ONClick!=null)
					foreach (var del in ONClick.GetInvocationList())
						ONClick -= del as PointerEventDataHandler;
				if(ONEnter!=null)
					foreach (var del in ONEnter.GetInvocationList())
						ONEnter -= del as PointerEventDataHandler;
				if(ONExit!=null)
					foreach (var del in ONExit.GetInvocationList())
						ONExit -= del as PointerEventDataHandler;
				if(BeginDrag!=null)
					foreach (var del in BeginDrag.GetInvocationList())
						BeginDrag -= del as PointerEventDataHandler;
				if(Drag!=null)
					foreach (var del in Drag.GetInvocationList())
						Drag -= del as PointerEventDataHandler;
				if(EndDrag!=null)
					foreach (var del in EndDrag.GetInvocationList())
						EndDrag -= del as PointerEventDataHandler;
				if(ONDown!=null)
					foreach (var del in ONDown.GetInvocationList())
						ONDown -= del as PointerEventDataHandler;
				if(ONUp!=null)
					foreach (var del in ONUp.GetInvocationList())
						ONUp -= del as PointerEventDataHandler;
				ONClick = null;
				ONEnter = null;
				ONExit = null;
				BeginDrag = null;
				Drag = null;
				EndDrag = null;
				ONDown = null;
				ONUp = null;
			});
		}
	}
}
