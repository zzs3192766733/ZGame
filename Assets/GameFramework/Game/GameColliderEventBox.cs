//========================================================
// 描述:碰撞事件发生器，大规模使用请利用MVC进行消息的派发
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2022/1/1 9:31:47
//========================================================

using System;
using System.Collections.Generic;
using GameFramework.ClassExt;
using UnityEngine;
using UnityEngine.Events;

namespace GameFramework.Game
{
    public class GameColliderEventBox : MonoBehaviour
    {
        public EventBox allEnterEvents;
        public EventBox allExitEvents;
        public BoxCollider boxCollider;
        public Vector3 center = Vector3.zero;
        public Vector3 size = Vector3.one;
#if UNITY_EDITOR
        public bool isShow = true;
        public Color showColor = Color.white;
#endif


        private void Start()
        {
            boxCollider = gameObject.AddOrGetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.center = center;
            boxCollider.size = size;
            var component = gameObject.AddOrGetComponent<Rigidbody>();
            component.isKinematic = true;
            component.useGravity = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            allEnterEvents.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            allExitEvents.Invoke(other);
        }

        public void AddEnterEvent(UnityAction<Collider> action) => allEnterEvents.AddListener(action);
        public void AddExitEvent(UnityAction<Collider> action) => allExitEvents.AddListener(action);
        public void ClearEnterEvent() => allEnterEvents.RemoveAllListeners();
        public void ClearExitEvent() => allExitEvents.RemoveAllListeners();

        public void ClearAllEvent()
        {
            ClearEnterEvent();
            ClearExitEvent();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!isShow) return;
            Gizmos.color = showColor;
            Gizmos.DrawWireCube(transform.TransformPoint(center), size);
        }
#endif
    }

    [Serializable]
    public class EventBox : UnityEvent<Collider>
    {
    }
}