//========================================================
// 描述:对象池管理器
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/10 14:53:08
//========================================================

using UnityEngine;
using System.Collections.Generic;
using GameFramework.Common;
using UniRx;
using UniRx.Triggers;

namespace GameFramework.ObjectPool
{
    public class PoolManager : SingleMonoBase<PoolManager>
    {
        private readonly Dictionary<string, PoolObject> _allPool = new Dictionary<string, PoolObject>();

        /// <summary>
        /// 同一个名称的对象池只会创建一次,再次创建返回空
        /// </summary>
        /// <param name="poolName"></param>
        /// <param name="prefab"></param>
        /// <param name="defCount"></param>
        /// <param name="isAutoCheck"></param>
        /// <param name="parentTrans"></param>
        /// <returns></returns>
        public PoolObject CreateObjectPool(string poolName, GameObject prefab, int defCount = 5,
            bool isAutoCheck = true,Transform parentTrans = null)
        {
            if (_allPool.ContainsKey(poolName) || prefab == null) return null;
            var go = new GameObject(poolName);
            go.transform.SetParent(parentTrans);
            var poolObject = go.AddComponent<PoolObject>();
            poolObject.InitPool(poolName, defCount, prefab, isAutoCheck);
            _allPool[poolName] = poolObject;
            //如果对象池销毁了,那么需要从字典中进行移除
            go.OnDestroyAsObservable().Subscribe(_ =>
            {
                if (_allPool.ContainsKey(poolName))
                    _allPool.Remove(poolName);
            });
            return poolObject;
        }

        public PoolObject GetObjectPool(string poolName)
        {
            return !_allPool.ContainsKey(poolName) ? null : _allPool[poolName];
        }

        /// <summary>
        /// 慎用!会清除所有模块引用对象池的资源,慎用!
        /// </summary>
        public void ClearAllPool()
        {
            foreach (var pool in _allPool.Values)
            {
                if (pool != null)
                {
                    pool.Clear();
                }
            }
            _allPool?.Clear();
        }

        /// <summary>
        /// 清除对象池推荐下面这个
        /// </summary>
        /// <param name="poolName"></param>
        public void ClearPool(string poolName)
        {
            if (!_allPool.ContainsKey(poolName)) return;
            //销毁对象池物体
            Destroy(_allPool[poolName].gameObject);
            _allPool[poolName].Clear();
            _allPool.Remove(poolName);
        }
    }
}