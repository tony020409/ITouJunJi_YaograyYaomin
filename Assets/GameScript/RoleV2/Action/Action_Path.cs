using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProtoBuf;
using PathTool;
using DG.Tweening;

//記得到Action.cs加
[ProtoContract]
public class Action_Path : BaseActionV2
{
    public Action_Path()
        : base(GameEM.EM_RoleAction.Path)
    { }

    [ProtoMember(40001)]
    public int m_RoleId;

    [ProtoMember(40002)]
    public string m_PathName;

    [ProtoMember(40003)]
    public string m_EndAction;



    private Vector3[] points;    //路徑航點
    private string endAction;    //結束行為
    private Path_Stantard tPath; //路徑
    BaseRoleControllV2 _BaseRoleControl;  //怪物


    /// <summary>
    /// 設定路徑
    /// </summary>
    /// <param name="tRoleId"   > 要走路徑的角色 </param>
    /// <param name="tPathName" > 路徑的名稱 </param>
    /// <param name="tEndAction"> 走到終點的動作 </param>
    public void f_Path(int tRoleId, string tPathName, string tEndAction) {
        m_RoleId = tRoleId;
        m_PathName = tPathName;
        m_EndAction = tEndAction;
    }



    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction()
    {
        _BaseRoleControl = BattleMain.GetInstance().f_GetRoleControl2(m_RoleId);
        if (_BaseRoleControl != null)  {
            //tmpRole.f_RunAIState(AI_EM.EM_AIState.Path, this);


            f_SetPath(m_PathName, m_EndAction);


            if (tPath != null) {
                _BaseRoleControl.transform.position = points[0];
                AutoMove();
            }

        }
        else  {
            //MessageBox.ASSERT("Die 未找到目标 " + m_RoleId);
        }
    }


    /// <summary>
    /// 設定路徑資訊
    /// </summary>
    /// <param name="iPathId"   > 路徑名稱 </param>
    /// <param name="iEndAction"> 到終點後的行為 </param>
    public void f_SetPath(string iPathId, string iEndAction) {

        //搜尋路徑名單內的路徑 
        for (int i = 0; i < PathTool_Manager.inst.PathList.Length; i++) {

            //先用路徑名稱去找，找到名稱相符的路徑就設置路徑
            if (PathTool_Manager.inst.PathList[i].name == iPathId) {
                tPath = PathTool_Manager.inst.PathList[i];
                break;
            }

            //如果所有名單內的路徑都找過了，還找不到名稱相符的
            else if (i == PathTool_Manager.inst.PathList.Length - 1 && tPath == null)  {
                tPath = PathTool_Manager.inst.PathList[int.Parse(iPathId)]; //就改用編號去找路徑
                if (tPath == null) {                                        //如果還是找不到路徑,就回報找不到的訊息
                    MessageBox.ASSERT(" - Action_Path.cs找不到 " + m_RoleId + " 要走的路徑，\n"
                        + "看看是不是腳本打錯 或 路徑沒有放到 BattleMain場景裡的路徑名單裡？\n"
                        + "(中文路徑找不到的情況下，可能造成「数据转换时出错,转换数据：xxx」的訊息出現)");
                }
            }
        }


        if (tPath != null) {
            points = tPath.GetPathPoints(false); //獲取路徑航點
            endAction = iEndAction;              //獲取結束行為
        }

    }




     /// <summary>
    /// 執行移動
    /// </summary>
    /// <param name="isClose"> 路徑是否閉合 </param>
    private void AutoMove(bool isClose = false) {
        float pathLength = PathTool_Manager.GetPathLength(points);       //計算路徑距離
        float moveTime = pathLength / _BaseRoleControl.f_GetWalkSpeed(); //計算移動所需時間 = 距離 / 怪物的移動速度
        TweenParams parms = new TweenParams();                           //???
        _BaseRoleControl.transform.DOLocalPath(points, moveTime, PathType.CatmullRom, PathMode.Full3D) // 路点数组 / 周期时间 / path type / path mode
                 .SetAs(parms)             //??
                 .SetEase(Ease.Linear)     //速度變化效果 (參考：http://dotween.demigiant.com/documentation.php)
                 .SetOptions(isClose)      //路徑是否閉合
                 .SetSpeedBased(true)      //基於速度來移動 (false 則表示基於時間(走完路徑要花多久時間))
                 .SetLookAt(0.001f)        //(0~1) 數字越小，移動轉向越自然，1表示不轉向
                 .OnComplete(ReachedEnd);  //如果循環的，每循環完成調用一次。不是循環的則完成執行
    }


    
    /// <summary>
    /// 移動結束事件
    /// </summary>
    private void ReachedEnd() {
        //如果設定成 Loop，則回到起點重複跑
        if (endAction == "Loop") {
            _BaseRoleControl.transform.position = points[0]; //直接移動到第一個航點
            AutoMove();                                      //重複路徑
        }

        //如果設定成 Kill，則死亡
        else if (endAction == "Kill") {
            _BaseRoleControl.f_Die();
        }

        //如果設定成 End，則結束AI
        else if (endAction == "End") {

        }

        //其它擴充
        //....



        _BaseRoleControl = null;

    }



}
