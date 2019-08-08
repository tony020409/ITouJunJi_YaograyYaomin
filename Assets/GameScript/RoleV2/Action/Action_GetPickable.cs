using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

//記得到Action.cs加
[ProtoContract]
public class Action_GetPickable : BaseActionV2
{

    public Action_GetPickable() 
        : base(GameEM.EM_RoleAction.GetPickable)
    { }

    [ProtoMember(33001)]
    public int m_RoleId;  //被撿到的物品

    [ProtoMember(33002)]
    public int m_OwnerId; //撿到的人



    /// <summary>
    /// 設定使用者 (物件自己的ID, 撿到的人的ID)
    /// </summary>
    /// <param name="newID"     > 物件自己的ID </param>
    /// <param name="newOwnerID"> 撿到的人的ID </param>
    public void f_SetOwner(int newID, int newOwnerID)  {
        m_RoleId = newID;
        m_OwnerId = newOwnerID;
    }



    /// <summary>
    /// 动作处理方法
    /// 用来处理服务器下发的动作
    /// </summary>
    public override void ProcessAction()
    {
        BaseRoleControllV2 tmpRole = BattleMain.GetInstance().m_BattleRolePool.f_Get(m_RoleId);
        BaseRoleControllV2 tmpOwner = BattleMain.GetInstance().m_BattleRolePool.f_Get(m_OwnerId);

        //如果物件不存在
        if (tmpRole == null) {
            return;
        }

        //如果物件的使用次數用盡
        if (tmpRole.f_IsDie()) {
            return;
        }

        //設定使用者、觸發效果
        PickableRoleControl _PickableRoleControl = tmpRole.GetComponent<PickableRoleControl>();
        if (_PickableRoleControl != null) {
            if (!_PickableRoleControl.canUSE) {
                return;
            }
            _PickableRoleControl._Owner = tmpOwner;    //設定使用者
            _PickableRoleControl.f_PickUp();           //發動物件效果
        }

    }





}
