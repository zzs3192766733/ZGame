//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/30 14:22:47
//========================================================

using GameFramework.Role.FSM.State;
using UnityEngine;
using System.Collections.Generic;
namespace GameFramework.Role.FSM
{
	public class RoleFSMMgr
	{
		public RoleCtrl RoleCtrl;
		private RoleStateAbstract _currRoleState = null;
		private RoleStateEnum _currRoleStateEnum = RoleStateEnum.None;
		private Dictionary<RoleStateEnum, RoleStateAbstract> _enum2State;

		public RoleFSMMgr(RoleCtrl roleCtrl)
		{
			_enum2State = new Dictionary<RoleStateEnum, RoleStateAbstract>();
			RoleCtrl = roleCtrl;
			InitAllState();
		}

		private void InitAllState()
		{
			_enum2State.Add(RoleStateEnum.Idle,new RoleStateIdle(this));
			_enum2State.Add(RoleStateEnum.Run,new RoleStateRun(this));
			_enum2State.Add(RoleStateEnum.Jump,new RoleStateJump(this));
		}

		public void OnUpdate()
		{
			_currRoleState?.OnUpdate();
		}

		public void ChangeState(RoleStateEnum newState)
		{
			if (_currRoleStateEnum == newState) return;
			_currRoleState?.OnExit();
			_currRoleStateEnum = newState;
			_currRoleState = _enum2State[_currRoleStateEnum];
			_currRoleState.OnEnter();
		}
	}
}
