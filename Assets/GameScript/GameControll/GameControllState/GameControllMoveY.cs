using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using DG.Tweening;

public class GameControllMoveY : GameControllBaseState
{
    //BaseRoleControllV2 _BaseRoleControl;
    Transform _BaseRoleControl;

    public GameControllMoveY()
        : base((int)EM_GameControllAction.MoveY)
    {
    }


    //？？. 移動角色Y軸（參數1是角色id，參數2是移動速度，參數3是移動高度(座標Y值)，參數4無效）
    public override void f_Enter(object Obj) {
        _CurGameControllDT = (GameControllDT)Obj;  //當前任務
        StartRun();
    }


    protected override void Run(object Obj) {
        base.Run(Obj);

        //參數1 = 指定角色
        //_BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(ccMath.atoi(_CurGameControllDT.szData1));
        //if (_BaseRoleControl == null) {
        //    MessageBox.ASSERT("【任務腳本】步驟" + _CurGameControllDT.iId + "未找到角色 :" + _CurGameControllDT.szData1);
        //    EndRun();
        //    return;
        //}

        _BaseRoleControl = BattleMain.GetInstance().m_oMySelfPlayerTransform.transform;
        if (_BaseRoleControl == null) {
            EndRun();
            return;
        }


        //參數2 = 以什麼速度
        float speed = ccMath.atof(_CurGameControllDT.szData2);

        //參數3 = 向上移動多少
        float endValue = ccMath.atof(_CurGameControllDT.szData3);
        

        //
        if (_CurGameControllDT.iNeedEnd == 1) {
            _BaseRoleControl.transform.DOLocalMoveY(endValue, speed)
                .SetSpeedBased(true)
                .SetEase(Ease.OutQuad)
                .OnComplete(DelayEndRun);
        }
        else {
            _BaseRoleControl.transform.DOLocalMoveY(endValue, speed)
                .SetSpeedBased(true)
                .SetEase(Ease.OutQuad);
            EndRun();
        }

        
    }

    private void DelayEndRun() {
        EndRun();
    }

}
