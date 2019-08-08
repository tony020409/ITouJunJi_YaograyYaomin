
using UnityEngine;

public class GameControl_RolePath : GameControllBaseState
{

    public GameControl_RolePath() :
        base((int)EM_GameControllAction.RolePath)
    { }


    public override void f_Enter(object Obj)
    {
        _CurGameControllDT = (GameControllDT)Obj; //當前腳本
        StartRun();
    }


    //29. 角色進入路徑 (參數1是目標座標, 參數2是跳多高, 參數3是跳多久)
    protected override void Run(object Obj)
    {
        base.Run(Obj);

        //獲取要執行的角色--------------------------------------------------------------------------------------------------------------
        int m_iRoleId = ccMath.atoi(_CurGameControllDT.szData1);
        BaseRoleControllV2 tRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_iRoleId);
        if (tRoleControl == null) {
            MessageBox.DEBUG("- 【警告】任務[" + _CurGameControllDT.iId + "] 未找到指定要走路徑的的角色: " + _CurGameControllDT.szData1);
            EndRun();
            return;
        }

        //獲取其他資訊------------------------------------------------------------------------------------------------------------------
        string iPathId = _CurGameControllDT.szData2;    //獲取要進入的路徑
        string iEndAction = _CurGameControllDT.szData3; //獲取走到路徑終點後要做的動作

        //執行動作----------------------------------------------------------------------------------------------------------------------
        if (StaticValue.m_bIsMaster) {
            Action_Path tAction = new Action_Path();
            tAction.f_Path(m_iRoleId, iPathId, iEndAction);
            glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tAction);
        }


        EndRun();

    }



}