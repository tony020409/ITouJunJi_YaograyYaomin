using ccU3DEngine;
using ProtoBuf;
using System;


namespace GameSysc
{
    
    [ProtoContract]             //[Serializable]
    [ProtoInclude(100, typeof(NoAction))]
    [ProtoInclude(101, typeof(PlayerAction))]
    [ProtoInclude(102, typeof(RoleAttackAction))]       //����
    [ProtoInclude(103, typeof(RoleDieAction))]       //����
    [ProtoInclude(104, typeof(RoleWaitAction))]       //�ȴ�
    [ProtoInclude(105, typeof(RoleWalkAction))]       //����
    [ProtoInclude(106, typeof(RoleBirthAction))]       //����
    [ProtoInclude(107, typeof(RoleHpAction))]       //����
    [ProtoInclude(108, typeof(RoleArrowAttackAction))] //���a����
    [ProtoInclude(109, typeof(PlayerPushAction))]       //����
    [ProtoInclude(110, typeof(RoleMulHpAction))]
    [ProtoInclude(111, typeof(RolePureHpAction))]      //�¥[����
    //[ProtoInclude(120, typeof(RoleFlyAction))]
    [ProtoInclude(130, typeof(ServerActionState))]
    [ProtoInclude(140, typeof(RoleSpiralAction))]
    [ProtoInclude(150, typeof(RoleWalk2TargetAction))]
    [ProtoInclude(160, typeof(SetActiveAction))]       //�}������(������)
    [ProtoInclude(170, typeof(RoomAction))]            //�����ж�
    [ProtoInclude(180, typeof(RolePathAction))]        //�����| (�S�Ψ�)
    [ProtoInclude(190, typeof(CreateHitEffectAction))] //�гy�l�u�������S�Ī���
    [ProtoInclude(200, typeof(ChangeAIAction))]        //����AI
    [ProtoInclude(210, typeof(ChangeAnimatorAction))]  //�ܧ�Animator�����Y��Int�Ѽƭ�
    [ProtoInclude(220, typeof(ChangeParameterAction))] //�ܧ��ܼ�
    //[ProtoInclude(230, typeof(RoleBeAttackAction))]  //�P�B����
    [ProtoInclude(240, typeof(CreateResourceAction))]  //�P�B�Ы�Resource��Ƨ��U������
    [ProtoInclude(250, typeof(Action_Shoot))]          //�P�B�Ǫ��g��
    [ProtoInclude(260, typeof(Action_Die))]            //�P�B���`
    [ProtoInclude(270, typeof(RoleWeaponStateAction))] //ǹ֧״̬
    [ProtoInclude(280, typeof(RoleChangePointAction))] //�����I

    [ProtoInclude(290, typeof(RoleThrowAttackAction))] //���Y
    [ProtoInclude(300, typeof(Action_PlayerShoot))]    //���a�g��
    [ProtoInclude(310, typeof(RoleArcherInitAction))]  //ArcherRoleControl - �X�ͮɷ|��������񪺸����I
    [ProtoInclude(320, typeof(Action_Animator))]       //�P�B�ʵe
    [ProtoInclude(330, typeof(Action_GetPickable))]    //�P�B�ߪF��
    [ProtoInclude(340, typeof(Action_PushBullet))]     //�P�B�Y�쪱�a���l�u
    [ProtoInclude(350, typeof(Action_AddGunClip))]     //�P�B�Y�쪱�a�ɼu��
    [ProtoInclude(360, typeof(Action_SetTarget))]      //�P�B�Y������Ǫ������ؼ�
    [ProtoInclude(370, typeof(Action_DelayDie))]       //�P�B�Y���Ǫ����𦺤`
    [ProtoInclude(380, typeof(Action_PlayerChangeGun))]//�P�B�Y���a���j�F
    [ProtoInclude(400, typeof(Action_Path))]           //�P�B�����|
    [ProtoInclude(410, typeof(Action_ChangePlayerPos))]//�P�B�����|

    public abstract class Action
    {
        public Action()
        {
            m_iId = ccMath.f_CreateKeyId();
            m_iType = 0;
        }
        /// <summary>
        /// ΨһId
        /// </summary>
        [ProtoMember(1)]
        public int m_iId;
        /// <summary>
        /// �������Id
        /// </summary>
        [ProtoMember(2)]
        public int m_iUserId;
        /// <summary>
        /// Action���� 0=NoAction 1=��������
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
        //����Ϊ�Զ�����������

            

        //[ProtoMember(5)]
        //public int NetworkAverage { get; set; }
        //[ProtoMember(6)]
        //public int RuntimeAverage { get; set; }



        public abstract void ProcessAction();
    }

}