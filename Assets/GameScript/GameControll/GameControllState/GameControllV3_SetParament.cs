using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class GameControllV3_SetParament : GameControllBaseState
{    
    public GameControllV3_SetParament() :
    base((int)EM_GameControllAction.V3_SetParament)
    { }


    public override void f_Enter(object Obj){
        _CurGameControllDT = (GameControllDT)Obj;
        //3001.设置变量的值（参数1为变量名, 参数2为变量值，参数3无效）    
        StartRun();
    }

    protected override void Run(object Obj) {
        base.Run(Obj);
        BattleMain.GetInstance().f_SetParamentData(_CurGameControllDT.szData1, _CurGameControllDT.szData2);
        EndRun();
    }


    
}


