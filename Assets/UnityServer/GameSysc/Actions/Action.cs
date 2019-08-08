using ccU3DEngine;
using ProtoBuf;
using System;


namespace GameSysc
{
    
    [ProtoContract]             //[Serializable]
    [ProtoInclude(100, typeof(NoAction))]
    [ProtoInclude(101, typeof(PlayerAction))]
    [ProtoInclude(102, typeof(RoleAttackAction))]       //攻击
    [ProtoInclude(103, typeof(RoleDieAction))]       //死亡
    [ProtoInclude(104, typeof(RoleWaitAction))]       //等待
    [ProtoInclude(105, typeof(RoleWalkAction))]       //行走
    [ProtoInclude(106, typeof(RoleBirthAction))]       //出生
    [ProtoInclude(107, typeof(RoleHpAction))]       //出生
    [ProtoInclude(108, typeof(RoleArrowAttackAction))] //產ю阑
    [ProtoInclude(109, typeof(PlayerPushAction))]       //出生
    [ProtoInclude(110, typeof(RoleMulHpAction))]
    [ProtoInclude(111, typeof(RolePureHpAction))]      //Ι﹀
    //[ProtoInclude(120, typeof(RoleFlyAction))]
    [ProtoInclude(130, typeof(ServerActionState))]
    [ProtoInclude(140, typeof(RoleSpiralAction))]
    [ProtoInclude(150, typeof(RoleWalk2TargetAction))]
    [ProtoInclude(160, typeof(SetActiveAction))]       //秨闽ン(ゼЧΘ)
    [ProtoInclude(170, typeof(RoomAction))]            //ち传┬丁
    [ProtoInclude(180, typeof(RolePathAction))]        //ǐ隔畖 (⊿ノ)
    [ProtoInclude(190, typeof(CreateHitEffectAction))] //承硑紆阑い疭ン
    [ProtoInclude(200, typeof(ChangeAIAction))]        //ち传AI
    [ProtoInclude(210, typeof(ChangeAnimatorAction))]  //跑Animatorい琘Int把计
    [ProtoInclude(220, typeof(ChangeParameterAction))] //跑跑计
    //[ProtoInclude(230, typeof(RoleBeAttackAction))]  //˙ю阑
    [ProtoInclude(240, typeof(CreateResourceAction))]  //˙承Resource戈Жン
    [ProtoInclude(250, typeof(Action_Shoot))]          //˙┣甮阑
    [ProtoInclude(260, typeof(Action_Die))]            //˙
    [ProtoInclude(270, typeof(RoleWeaponStateAction))] //枪支状态
    [ProtoInclude(280, typeof(RoleChangePointAction))] //跟磷翴

    [ProtoInclude(290, typeof(RoleThrowAttackAction))] //щ耏
    [ProtoInclude(300, typeof(Action_PlayerShoot))]    //產甮阑
    [ProtoInclude(310, typeof(RoleArcherInitAction))]  //ArcherRoleControl - ネ穦ǐ跟旅翴
    [ProtoInclude(320, typeof(Action_Animator))]       //˙笆礶
    [ProtoInclude(330, typeof(Action_GetPickable))]    //˙具狥﹁
    [ProtoInclude(340, typeof(Action_PushBullet))]     //˙琘產传紆
    [ProtoInclude(350, typeof(Action_AddGunClip))]     //˙琘產干紆Ж
    [ProtoInclude(360, typeof(Action_SetTarget))]      //˙琘唉环ю┣ю阑ヘ夹
    [ProtoInclude(370, typeof(Action_DelayDie))]       //˙琘唉┣┑筐
    [ProtoInclude(380, typeof(Action_PlayerChangeGun))]//˙琘產传簀
    [ProtoInclude(400, typeof(Action_Path))]           //˙ǐ隔畖
    [ProtoInclude(410, typeof(Action_ChangePlayerPos))]//˙ǐ隔畖

    public abstract class Action
    {
        public Action()
        {
            m_iId = ccMath.f_CreateKeyId();
            m_iType = 0;
        }
        /// <summary>
        /// 唯一Id
        /// </summary>
        [ProtoMember(1)]
        public int m_iId;
        /// <summary>
        /// 所属玩家Id
        /// </summary>
        [ProtoMember(2)]
        public int m_iUserId;
        /// <summary>
        /// Action类型 0=NoAction 1=其它类型
        /// </summary>
        [ProtoMember(3)]
        public short m_iType;

        [ProtoMember(10)]
        public int m_iRoleId;

        public void f_Save(int iUserId)
        {
            m_iUserId = iUserId;
        }

        ////////////////////////////////////////////////////////////////////
        //以下为自定义数据内容

            

        //[ProtoMember(5)]
        //public int NetworkAverage { get; set; }
        //[ProtoMember(6)]
        //public int RuntimeAverage { get; set; }



        public abstract void ProcessAction();
    }

}