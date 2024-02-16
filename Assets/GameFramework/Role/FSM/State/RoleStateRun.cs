//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/30 14:24:59
//========================================================

using GameFramework.Common;
using UnityEngine;
namespace GameFramework.Role.FSM.State
{
	
	public class RoleStateRun : RoleStateAbstract
	{
	    public RoleStateRun(RoleFSMMgr fsmMgr) : base(fsmMgr)
	    {
	    }

	    public override void OnEnter()
	    {
		    CurrAnimator.SetBool("Run",true);
	    }

	    public override void OnUpdate()
	    {
		    GameLogger.Log("更新Run",DebugColor.Red);
	    }

	    public override void OnExit()
	    {
		    CurrAnimator.SetBool("Run",false);
	    }
	}
}
