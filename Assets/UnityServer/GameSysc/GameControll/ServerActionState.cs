using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ProtoContract]
public class ServerActionState : GameSysc.Action
{
    [ProtoMember(13001)]
    public int m_iGameControllDTId;

    [ProtoMember(13002)]
    public int m_iConditionId;



    public ServerActionState()
            : base()
    {
        m_iType = (int)GameEM.EM_RoleAction.ServerActionState;
    }

    /// <summary>
    /// 创建角色动作
    /// </summary>
    /// <param name="iId">角色Id</param>
    /// <param name="tTeamType">角色所属队伍</param>
    /// <param name="iCharacterDTId">角色模板Id</param>
    /// <param name="iTileNodeId">角色所在位置</param>
    /// 
    /// <summary>
    /// BasePlayerControll tBasePlayerControll, CharacterDT tCharacterDT, TileNode tTileNode
    /// </summary>
    public void f_Save(int iConditionId, int iGameControllDTId)
    {
        m_iConditionId = iConditionId;
        m_iGameControllDTId = iGameControllDTId;
    }

    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction()
    {
#if UNITY_EDITOR
        //MessageBox.DEBUG("服务器脚本任务指令 " + m_iGameControllDTId);
#endif
        BattleMain.GetInstance().f_RunServerActionState(m_iConditionId, m_iGameControllDTId);
    }

    


}
