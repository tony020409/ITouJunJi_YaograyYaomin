using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ChangePonit_noLookAt : AI_RunBaseStateV2
{

    private BaseRoleControllV2 _ReadyAttackTarget;
    private Vector3 tmpLookAtPos;

    public AI_ChangePonit_noLookAt()
        : base(AI_EM.EM_AIState.AI_ChangePoint) { }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        //Debug.LogWarning("進入AI_ChangePoint_NoLookAt");

        if (_CurAction != null)
        {
            RoleChangePointAction tRoleChangePointAction = (RoleChangePointAction)_CurAction;

            _ReadyAttackTarget = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetEnemy2(_BaseRoleControl, _BaseRoleControl.f_GetViewSize());
            _BaseRoleControl.GetComponent<ArcherRoleControl>().CurHidePos = tRoleChangePointAction.m_Index;

            List<Vector3> path = new List<Vector3>();
            path.Add(transform.position);
            path.Add(_BaseRoleControl.GetComponent<ArcherRoleControl>().HidePos[tRoleChangePointAction.m_Index].position);

            AutoMove(path.ToArray());
        }
    }

    public override void f_Execute()
    {
        base.f_Execute();

        if (_ReadyAttackTarget != null)
        {
            tmpLookAtPos = _ReadyAttackTarget.transform.position; //設定面向的位置
            tmpLookAtPos.y = transform.position.y;                //忽略面向的位置的Y軸
            transform.LookAt(tmpLookAtPos);                       //朝向玩家
        }
    }

    public override void f_Exit()
    {
        base.f_Exit();
        //Debug.LogWarning("離開AI_ChangePoint_NoLookAt");
    }

    private void AutoMove(Vector3[] aArray)
    {
        Hashtable args = new Hashtable();

        args.Add("path", aArray);
        args.Add("easeType", iTween.EaseType.linear);

        float fSingleTime = 3 / _BaseRoleControl.f_GetWalkSpeed() - StaticValue.m_fNetAverage / 1000;

        MessageBox.DEBUG(_BaseRoleControl.m_iId + " AutoMove " + fSingleTime + " " + StaticValue.m_fNetAverage / 1000);

        args.Add("time", aArray.Length * fSingleTime);
        args.Add("oncomplete", "ccCallBackMoveComplete");
        args.Add("oncompleteparams", "end");
        args.Add("oncompletetarget", _BaseRoleControl.gameObject);

        //移动中调用，参数和上面类似
        args.Add("onupdate", "ccCallBackMoveUpdate");
        args.Add("onupdatetarget", _BaseRoleControl.gameObject);
        args.Add("onupdateparams", true);
        iTween tiTween = iTween.MoveTo(_BaseRoleControl.gameObject, args);
        tiTween.m_ccCallBackMoveUpdate = ccCallBackMoveUpdate;
        tiTween.m_ccCallBackMoveComplete = ccCallBackMoveComplete;
    }

    protected virtual void ccCallBackMoveUpdate(object Obj)
    {

    }

    protected virtual void ccCallBackMoveComplete(object Obj)
    {
        //MessageBox.DEBUG("更换躲避点完成");
        f_RunStateComplete();
    }

    private void StopWalk()
    {
        iTween.f_Stop(_BaseRoleControl.gameObject);
        _BaseRoleControl.f_StopWalk();
    }
}
