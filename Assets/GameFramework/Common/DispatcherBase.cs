//========================================================
// 描述:消息派发基类
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/11 9:49:03
//========================================================

using UnityEngine;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

namespace GameFramework.Common
{
    public class DispatcherBase<TP, TX> : SingleBase<DispatcherBase<TP, TX>> where TP : class
    {
        public delegate void OnActionHandler(TP p);
        private Dictionary<TX,List<OnActionHandler>> key2List = new Dictionary<TX, List<OnActionHandler>>();
        public void AddEventListener(TX key, OnActionHandler handler,MonoBehaviour mono = null)
        {
            if (key2List.ContainsKey(key))
                key2List[key].Add(handler);
            else
            {
                var lst = new List<OnActionHandler> {handler};
                key2List.Add(key,lst);
            }

            if (mono != null)
                mono.OnDestroyAsObservable().Subscribe(_ => RemoveEventListener(key, handler));
        }
        public void RemoveEventListener(TX key, OnActionHandler handler)
        {
            if (key2List.ContainsKey(key))
            {
                key2List[key].Remove(handler);
                if (key2List[key].Count <= 0)
                    key2List.Remove(key);
            }
        }
        public void Dispatch(TX key, TP val)
        {
            if (key2List.ContainsKey(key))
            {
                var lst = key2List[key];
                if (lst != null && lst.Count > 0)
                {
                    foreach (var t in lst)
                        t?.Invoke(val);
                }
            }
        }
        public void Clear()
        {
            foreach (var lst in key2List.Values)
                lst.Clear();
            key2List.Clear();
        }
    }
}