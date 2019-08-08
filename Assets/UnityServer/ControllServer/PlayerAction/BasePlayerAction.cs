using ccU3DEngine;
using ProtoBuf;
using System;


namespace GameControllAction
{

    /// <summary>
    /// ����ƽ����
    /// Ϊ����һ���ͻ����������ͻ��˷���ƽ������Action�ӿ��޸�Ϊһ���������ֶεĳ�����
    /// </summary>
    [ProtoContract]             //[Serializable]
    [ProtoInclude(200, typeof(PlayerTransformAction))]
    [ProtoInclude(201, typeof(PlayerRotateAction))]
    public abstract class BasePlayerAction
    {
        public BasePlayerAction()
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
        /// ��ǰ��Ϸ֡
        /// </summary>
        //[ProtoMember(3)]
        //public int m_iCurGameSyscFrame;
        /// <summary>
        /// Action���� 0=NoAction 1=��������
        /// </summary>
        [ProtoMember(4)]
        public int m_iType;

        public void f_Save(int iUserId, int iCurGameSyscFrame)
        {
            m_iUserId = iUserId;
            //m_iCurGameSyscFrame = iCurGameSyscFrame;
        }

        ////////////////////////////////////////////////////////////////////
        //����Ϊ�Զ�����������
                
        public abstract void ProcessAction();


    }

}