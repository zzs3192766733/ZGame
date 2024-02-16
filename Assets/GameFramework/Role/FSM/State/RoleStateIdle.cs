//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/30 14:24:45
//========================================================

using GameFramework.Common;
using UnityEngine;
namespace GameFramework.Role.FSM.State
{
	
	public class RoleStateIdle : RoleStateAbstract
	{
		public RoleStateIdle(RoleFSMMgr fsmMgr) : base(fsmMgr)
		{
		}

		public override void OnEnter()
		{
			CurrAnimator.SetBool("Idle",true);
		}

		public override void OnUpdate()
		{
			CurrAnimatorStateInfo = CurrAnimator.GetCurrentAnimatorStateInfo(0);
		}

		public override void OnExit()
		{
			CurrAnimator.SetBool("Idle",false);
		}
	}
}
