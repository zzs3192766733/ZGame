//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/11 14:53:59
//========================================================

using System;
using GameFramework.Audio;
using GameFramework.Effect;
using GameFramework.ObjectPool;
using UnityEngine;
namespace GameFramework.Common
{
	
	public class Test01 : MonoBehaviour
	{
		[SerializeField] private AudioClip _clip;
		[SerializeField] private GameObject _effect;
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				AudioManager.Instance.PlayClip(_clip);
			}

			if (Input.GetKeyDown(KeyCode.B))
			{
				EffectManager.Instance.PlayEffect(_effect,transform,2f);
			}

			if (Input.GetKeyDown(KeyCode.D))
			{
				EffectManager.Instance.ClearEffect(_effect);
			}
		}
	}
}
