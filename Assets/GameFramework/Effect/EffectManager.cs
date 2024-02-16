//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/12 18:55:33
//========================================================

using System;
using GameFramework.Common;
using UnityEngine;
using System.Collections.Generic;
using GameFramework.ObjectPool;
using GameFramework.Time;

namespace GameFramework.Effect
{
	
	public class EffectManager : SingleBase<EffectManager>
	{
		private Dictionary<GameObject, PoolObject> _dir;
		public EffectManager()
		{
			_dir = new Dictionary<GameObject, PoolObject>();
		}
		public void PlayEffect(GameObject prefab,Transform trans,float delayTime) => PlayEffect(prefab,trans.position,delayTime);
		public void PlayEffect(GameObject prefab,Vector3 pos,float delayTime)
		{
			if (_dir.ContainsKey(prefab))
			{
				var pool = _dir[prefab] == null
					? PoolManager.Instance.CreateObjectPool(prefab.name, prefab)
					: _dir[prefab];
				_dir[prefab] = pool;
				var go = pool.GetObject();
				go.transform.position = pos;
				TimeManager.Instance.Delay(TimeSpan.FromSeconds(delayTime),()=>pool.SetObject(go),pool.gameObject);
			}
			else
			{
				var pool = PoolManager.Instance.CreateObjectPool(prefab.name,prefab);
				_dir[prefab] = pool;
				PlayEffect(prefab,pos,delayTime);
			}
		}
		public void PlayEffect(GameObject prefab,Vector3 pos,float delayTime,out GameObject effectObj)
		{
			if (_dir.ContainsKey(prefab))
			{
				var pool = _dir[prefab] == null
					? PoolManager.Instance.CreateObjectPool(prefab.name, prefab)
					: _dir[prefab];
				_dir[prefab] = pool;
				var go = pool.GetObject();
				effectObj = go;
				go.transform.position = pos;
				TimeManager.Instance.Delay(TimeSpan.FromSeconds(delayTime),()=>pool.SetObject(go),pool.gameObject);
			}
			else
			{
				var pool = PoolManager.Instance.CreateObjectPool(prefab.name,prefab);
				_dir[prefab] = pool;
				PlayEffect(prefab,pos,delayTime,out effectObj);
			}
		}

		public void ClearAllEffect()
		{
			foreach (var pool in _dir.Values)
				PoolManager.Instance.ClearPool(pool.PoolName);
			_dir.Clear();
		}

		public void ClearEffect(GameObject prefab)
		{
			if (!_dir.ContainsKey(prefab)) return;
			PoolManager.Instance.ClearPool(_dir[prefab].PoolName);
			_dir.Remove(prefab);
		}
	}
}
