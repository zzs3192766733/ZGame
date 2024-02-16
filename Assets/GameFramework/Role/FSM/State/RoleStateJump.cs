//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/31 8:08:28
//========================================================

using GameFramework.Common;
using UnityEngine;

namespace GameFramework.Role.FSM.State
{
    public class RoleStateJump : RoleStateAbstract
    {
        public RoleStateJump(RoleFSMMgr fsmMgr) : base(fsmMgr)
        {
        }

        public override void OnEnter()
        {
            CurrAnimator.SetBool("Jump", true);
        }

        public override void OnUpdate()
        {
            CurrAnimatorStateInfo = CurrAnimator.GetCurrentAnimatorStateInfo(0);
            if (CurrAnimatorStateInfo.IsName("BasicMotions@Jump01") && CurrAnimationLastTime <= 0.1f)
            {
                RoleFsmMgr.ChangeState(RoleStateEnum.Idle);
            }
        }

        public override void OnExit()
        {
            CurrAnimator.SetBool("Jump", false);
        }
    }
}