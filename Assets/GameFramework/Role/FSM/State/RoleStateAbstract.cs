//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/30 14:23:36
//========================================================

using UnityEngine;
namespace GameFramework.Role.FSM.State
{
	
	public abstract class RoleStateAbstract
	{
		protected RoleFSMMgr RoleFsmMgr;
		protected AnimatorStateInfo CurrAnimatorStateInfo;
		protected Animator CurrAnimator;
		protected float CurrAnimationLastTime
		{
			get
			{
				var time = CurrAnimatorStateInfo.length - CurrAnimatorStateInfo.normalizedTime;
				if (time < 0) return 0;
				else
					return time;
			}
		}

		protected RoleStateAbstract(RoleFSMMgr fsmMgr)
		{
			RoleFsmMgr = fsmMgr;
			CurrAnimator = RoleFsmMgr.RoleCtrl.SelfAnimator;
		}
		public abstract void OnEnter();
		public abstract void OnUpdate();
		public abstract void OnExit();
	}
}
