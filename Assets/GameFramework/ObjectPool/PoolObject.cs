//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/10 14:53:08
//========================================================

using UnityEngine;
using System.Collections.Generic;

namespace GameFramework.ObjectPool
{
    public class PoolObject : MonoBehaviour
    {
        private Queue<GameObject> _queueFreePool = new Queue<GameObject>();
        private int defCount;
        private GameObject _selfPrefab;
        private string poolName;
        public string PoolName => poolName;
        private bool _isAutoCheck = false;

        public void InitPool(string poolName, int defCount, GameObject prefab, bool isAutoCheck)
        {
            this.poolName = poolName;
            this.defCount = defCount;
            this._selfPrefab = prefab;
            _isAutoCheck = isAutoCheck;
            CreateItem(defCount);
        }

        private void CreateItem(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var go = Instantiate(_selfPrefab, this.transform, true);
                go.SetActive(false);
                _queueFreePool.Enqueue(go);
            }
        }

        public GameObject GetObject(Transform targetTrans)
        {
            if (_queueFreePool.Count <= 0)
                CreateItem(defCount);
            var go = _queueFreePool.Dequeue();
            go.SetActive(true);
            go.transform.position = targetTrans.position;
            return go;
        }

        public GameObject GetObject()
        {
            if (_queueFreePool.Count <= 0)
                CreateItem(defCount);
            var go = _queueFreePool.Dequeue();
            go.SetActive(true);
            return go;
        }

        public void SetObject(GameObject obj)
        {
            obj.SetActive(false);
            _queueFreePool.Enqueue(obj);
            if (!_isAutoCheck) return;
            CheckQueue();
        }

        private void CheckQueue()
        {
            if (_queueFreePool.Count <= defCount * 5) return;
            while (_queueFreePool.Count > defCount)
            {
                var go = _queueFreePool.Dequeue();
                DestroyImmediate(go);//采用这个销毁避免发生Bug隐患
            }
        }

        public void Clear()
        {
            if (_queueFreePool != null)
            {
                _queueFreePool.Clear();
                Destroy(this.gameObject);
            }
        }
    }
}