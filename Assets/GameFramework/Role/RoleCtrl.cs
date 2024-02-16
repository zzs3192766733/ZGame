//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/30 14:24:24
//========================================================

using System;
using GameFramework.Role.AI;
using GameFramework.Role.FSM;
using GameFramework.Role.FSM.State;
using GameFramework.Role.Info;
using UnityEngine;
namespace GameFramework.Role
{
	
	public class RoleCtrl : MonoBehaviour
	{
		private RoleType _currRoleType;
		private RoleFSMMgr _currRoleFsmMgr;
		public RoleFSMMgr CurrRoleFsmMgr
		{
			get
			{
				if (_currRoleFsmMgr == null)
				{
					_currRoleFsmMgr = new RoleFSMMgr(this);
				}

				return _currRoleFsmMgr;
			}
		}
		private RoleInfoBase _currRoleInfo;
		private IRoleAI _currRoleAI;
		private Animator _selfAnimator;
		public Animator SelfAnimator
		{
			get
			{
				if (_selfAnimator == null)
				{
					_selfAnimator = GetComponent<Animator>();
				}
				return _selfAnimator;
			}
		}

		private void Start()
		{
			Idle();
		}

		public void InitRole(RoleType roleType,RoleInfoBase roleInfo,IRoleAI roleAI)
		{
			_currRoleType = roleType;
			_currRoleInfo = roleInfo;
			_currRoleAI = roleAI;
		}

		private void Update()
		{
			_currRoleAI?.DoAI();
			
			CurrRoleFsmMgr.OnUpdate();

			if (Input.GetKeyDown(KeyCode.A))
			{
				Idle();
			}

			if (Input.GetKeyDown(KeyCode.S))
			{
				Run();
			}

			if (Input.GetKeyDown(KeyCode.D))
			{
				Jump();
			}
		}

		private void Idle()
		{
			CurrRoleFsmMgr.ChangeState(RoleStateEnum.Idle);
		}

		private void Run()
		{
			CurrRoleFsmMgr.ChangeState(RoleStateEnum.Run);
		}

		private void Jump()
		{
			_currRoleFsmMgr.ChangeState(RoleStateEnum.Jump);
		}
	}
}
