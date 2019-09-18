using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> [指令] 漂浮 </summary>
public class GameControllV3_Boat : GameControllBaseState {

    private GameObject tmpObj;
    private Boat m_Boat;


    public GameControllV3_Boat()
        : base((int)EM_GameControllAction.Boat)
    { }


    public override void f_Enter(object Obj) {
        _CurGameControllDT = (GameControllDT)Obj;

        //取得要播放動畫的物件 (可能是角色，也可能是 BattainMain裡存放的物件)
        try {
            tmpObj = BattleMain.GetInstance().f_GetRoleControl2(int.Parse(_CurGameControllDT.szData1)).gameObject;
        }
        catch {
            tmpObj = BattleMain.GetInstance().f_GetGameObj(_CurGameControllDT.szData1);
        }

        //添加漂浮程式
        if (tmpObj != null) {

            if (tmpObj.GetComponent<MySelfPlayerControll2>() != null) {
                tmpObj = BattleMain.GetInstance().m_oMySelfPlayerTransform;
            }


            m_Boat = tmpObj.AddComponent<Boat>();
            m_Boat.moveDis = ccMath.atof(_CurGameControllDT.szData2);
            m_Boat.LeftRight = ccMath.atof(_CurGameControllDT.szData3);
            ccTimeEvent.GetInstance().f_RegEvent(ccMath.atof(_CurGameControllDT.szData4), false, Obj, RemoveBoat);
        }
        StartRun();
    }



    protected override void Run(object Obj) {
        if (tmpObj == null) {
            Debug.LogWarning("【警告】腳本[" + _CurGameControllDT.iId + "] 未找到指定角色或物件:" + _CurGameControllDT.szData1 + "   // " + _CurGameControllDT.szName);
            EndRun();
            return;
        }

        if (m_Boat == null) {
            EndRun();
            return;
        }


        EndRun();
    }



    /// <summary> 移除漂浮程式 </summary>
    /// <param name="obj"></param>
    private void RemoveBoat(object obj) {
        if (tmpObj != null)  {
            if (m_Boat != null) {
                MonoBehaviour.Destroy(m_Boat);
            }
        }
    }




}