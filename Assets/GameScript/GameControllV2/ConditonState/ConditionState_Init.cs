using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;


public class GameControllV3_Init : ConditionState_Base
{
    public GameControllV3_Init(int iId, GameControllPara tGameControllPara) : base(iId, tGameControllPara)
    {

    }
    
    public override void f_Init(string szParament, string szParamentData, string szData1, string szData2, string szData3, string szData4)
    {
        base.f_Init(szParament, szParamentData, szData1, szData2, szData3, szData4);

    }

    //3000.系统初始化指令,只有szParament参数有效（参数1无效,参数2无效，参数3无效，参数4无效）
    public override bool f_Check()
    {
        return base.f_Check();
    }




}
