using ccU3DEngine;
using GameControllAction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class BattleMain : UIFramwork, IPlayerMono
{
    private DateTime m_tStartTimeTime;
    public BulletPool m_BulletPool = new BulletPool();

    /// <summary>
    /// 保存游戏角色Pool
    /// </summary>
    public BattleRolePool m_BattleRolePool = new BattleRolePool();

    public GameMission _GameMission = new GameMission();
    private PositionManager _PositionManager = null;

    [Header("(測試用) 按O攻擊目標1")]
    public int Attackild_01 = 2000;
    public int AttackiDamage_01 = 10;

    [Header("(測試用) 按P攻擊目標2")]
    public int Attackild_02 = 3000;
    public int AttackiDamage_02 = 20;

    [Header("---------------------")]
    public MapNav m_MapNav;

    private GameObject[] m_aPlayerPos;
    public GameObject m_oMySelfPlayer;
    public GameObject m_oMySelfPlayerTransform;
    public GameObject m_oRole;

    [Header("UI項目---------------")]
    public GameObject[] ui_Win;
    public GameObject[] ui_Lost;
    public GameObject[] ui_Other;

    [Header("UI：場景淡入淡出用")]
    [Rename("淡入淡出時的音樂")] public AudioClip _FadeClip;
    [Rename("淡入淡出用的黑圖")] public Image UI_SceneFade;

    [Header("躲藏点---------")]
    public Transform[] HidePos;


    [Header("門清單---------")]
    public GameObject[] Door;
    /// <summary>
    /// 門字典
    /// </summary>
    private Dictionary<string, DoorAnimation> _dicDoor = new Dictionary<string, DoorAnimation>();



    [Header("網路斷線遮罩-------")]
    public GameObject Canvas_Internet_WARING;
    [Header("伺服器斷線遮罩-------")]
    public GameObject Canvas_Server_WARING;
    public bool _bGameSocketEor = false;

    private AudioSource _audioSource;
    private BattleAI _BattleAI = new BattleAI();
    private GameControllV2 _GameControllV2 = new GameControllV2();
    private static BattleMain _Instance = null;
    public static BattleMain GetInstance() {
        return _Instance;
    }



    ////////////////////////////////////////////////////////////////////////////////////

    #region 内部消息

    void Start()
    {
        _Instance = this;

        InitComponent();
        InitPlayerPos();
        InitGame();
        
        InitGameRes();
        Data_Pool.m_PlayerPool.f_RequestPlayerList();
        //ccTimeEvent.GetInstance().f_RegEvent(3, false, null, ReadyStartGame);

        glo_Main.GetInstance().m_GameSyscManager.f_AddSyscObject(this);

        ccTimeEvent.GetInstance().f_RegEvent(1, false, null, InitPlayerPool);

        _audioSource = this.GetComponent<AudioSource>();
    }




    void Update() {
        if (Input.GetKeyDown(KeyCode.O) && StaticValue.m_bIsMaster) {
            BaseRoleControllV2 tBaseRoleControl = f_GetRoleControl2(Attackild_01); //目標1
            if (tBaseRoleControl != null) {
                RoleHpAction tRoleHpAction = new RoleHpAction();
                tRoleHpAction.f_Hp(0, tBaseRoleControl.m_iId, AttackiDamage_01, Vector3.zero);
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleHpAction);          //同步攻擊執行   
                Debug.LogWarning("代號(" + Attackild_01 + ")受到 " + AttackiDamage_01 + "單位的傷害! 剩餘血量為:" + tBaseRoleControl.f_GetHp());
            }
            else Debug.LogWarning("代號(" + Attackild_01 + ")不存在!");
        }

        if (Input.GetKeyDown(KeyCode.P) && StaticValue.m_bIsMaster) {
            BaseRoleControllV2 tBaseRoleControl = f_GetRoleControl2(Attackild_02); //目標2
            if (tBaseRoleControl != null) {
                RoleHpAction tRoleHpAction = new RoleHpAction();
                tRoleHpAction.f_Hp(0, tBaseRoleControl.m_iId, AttackiDamage_02, Vector3.zero);
                glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleHpAction);          //同步攻擊執行   
                Debug.LogWarning("代號(" + Attackild_02 + ")受到 " + AttackiDamage_02 + "單位的傷害! 剩餘血量為:" + tBaseRoleControl.f_GetHp());
            }
            else Debug.LogWarning("代號(" + Attackild_02 + ")不存在!");
        }










        if (Input.GetKeyDown(KeyCode.F1)) {
            f_GetGameObj("1F").SetActive( !f_GetGameObj("1F").activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            f_GetGameObj("2F").SetActive(!f_GetGameObj("2F").activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            f_GetGameObj("3F").SetActive(!f_GetGameObj("3F").activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            f_GetGameObj("4F").SetActive(!f_GetGameObj("4F").activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            f_GetGameObj("5F").SetActive(!f_GetGameObj("5F").activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            f_GetGameObj("6F").SetActive(!f_GetGameObj("6F").activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            f_GetGameObj("7F").SetActive(!f_GetGameObj("7F").activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            f_GetGameObj("8F").SetActive(!f_GetGameObj("8F").activeInHierarchy);
        }

        if (Input.GetKeyDown(KeyCode.F9)) {
            if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming)
            {
                glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEOVER, EM_GameResult.Win);
            }
        }


       
    }


    private void InitGameRes()
    {
        List<NBaseSCDT> aData = glo_Main.GetInstance().m_SC_Pool.m_GameControllSC.f_GetAll();
        for (int i = 0; i < aData.Count; i++)
        {
            GameControllDT tGameControllDT = (GameControllDT)aData[i];
            EM_GameControllAction tEM_GameControllAction = (EM_GameControllAction)tGameControllDT.iStartAction;
            if (tEM_GameControllAction == EM_GameControllAction.RoleCreate)
            {
                CharacterDT tCharacterDT = (CharacterDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterSC.f_GetSC(ccMath.atoi(tGameControllDT.szData2));
                glo_Main.GetInstance().m_ResourceManager.f_CreateRole2(tCharacterDT, false);
            }
        }

        _GameControllV2.f_Init();
    }

    private void InitComponent()
    {
        //位置管理器
        GameObject tObj = f_GetObject("PosManager");
        if (tObj == null) {
            MessageBox.ASSERT("PosManager 组件未找到");
        }
        _PositionManager = tObj.GetComponent<PositionManager>();


        //門管理
        SaveDoor();

    }

    private void InitGame()
    {
        glo_Main.GetInstance().f_Reset();

        if (GloData.glo_iGameModel == 1)
        {
            GraphicRaycaster tGraphicRaycaster = transform.GetChild(0).GetComponent<GraphicRaycaster>();
            tGraphicRaycaster.enabled = true;
        }
        else
        {
            //f_GetObject("Master").SetActive(false);
        }

        //f_GetObject("Win").SetActive(false);
    }

    private void InitPlayerPos()
    {
        //
        GameObject PlayerPos = f_GetObject("PlayerPos");
        m_aPlayerPos = new GameObject[PlayerPos.transform.childCount];
        for (int i = 0; i < PlayerPos.transform.childCount; i++){
            m_aPlayerPos[i] = PlayerPos.transform.GetChild(i).gameObject;
        }
    }

    private void InitPlayerPool(object Obj = null)
    {
        if (StaticValue.m_UserDataUnit.m_PlayerDT == null)
        {
            ccTimeEvent.GetInstance().f_RegEvent(1, false, null, InitPlayerPool);
        }
        else
        {
            Data_Pool.m_PlayerPool.f_InitPlayerPool();
        }
    }


    private int _iDDD = 0;
    //void FixedUpdate()
    public void f_Update(int iDeltaTime)
    {

        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Waiting)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                //UI_BtnStartGame();
            }
        }
        else if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming)
        {
            TestKey();
            _iDDD--;
            if (StaticValue.m_bIsMaster)
            {
                _BattleAI.f_Update();
                //_GameMission.f_CheckAllPlayerIsDie();
            }
            if (_GameControllV2 != null)
            {
                _GameControllV2.f_Update();
            }          
        }
      
    }

    #endregion


    #region 同步相关

    private bool _bIsComplete;
    public bool m_bIsComplete
    {
        get
        {
            return _bIsComplete;
        }

        set
        {
            _bIsComplete = value;
        }
    }

    #endregion


    #region 消息相关

    protected override void f_InitMessage()
    {
        base.f_InitMessage();

        //f_RegClickEvent("BtnLeaveBattle", UI_BtnLeaveBattle);
        f_RegClickEvent("BtnStartGame", UI_BtnStartGame);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.DOORDIE, OnDoorDie, null);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.PlayerJionGame, On_BattleUIUpdate);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.PlayerLeaveGame, On_BattleUIUpdate);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.GAMEOVER, Callback_GameOver, this);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.SOCKETERROR, On_SocketError, this);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.GAMESOCKETERO, On_GameSocketError, this);
    }

    private void DoDestory()
    {
        glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.DOORDIE, OnDoorDie, null);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.PlayerJionGame, On_BattleUIUpdate);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.PlayerLeaveGame, On_BattleUIUpdate);
        glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.GAMEOVER, Callback_GameOver, this);
        glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.SOCKETERROR,On_SocketError,this);
        glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.GAMESOCKETERO, On_GameSocketError, this);

        glo_Main.GetInstance().m_EM_GameStatic = EM_GameStatic.Waiting;
        StaticValue.m_UserDataUnit.m_PlayerDT = null;
        StaticValue.m_PlayerSelTeam = GameEM.TeamType.Ero;
        StaticValue.m_PlayerSelJob = GameEM.PlayerJob.Ero;
        m_BattleRolePool.f_Clear();
        _BattleAI.f_Clear();
        Data_Pool.m_TeamPool.f_Destory();
    }

    private void On_SocketError(object obj)
    {
        MessageBox.DEBUG("與網路連接錯誤");
        //跳出斷網UI
        String strErrorInfor = (String)obj;
        Canvas_Internet_WARING.SetActive(true);
    }

    private void On_GameSocketError(object obj)
    {
        if (_bGameSocketEor)
        {
            return;
        }
        MessageBox.DEBUG("與遊戲伺服器連接錯誤");
        _bGameSocketEor = true;
        string strErrorInfor = (string)obj;
        //跳出斷伺服器UI
        Canvas_Server_WARING.SetActive(true);
    }


    private void On_BattleUIUpdate(object Obj)
    {
        return;

        List<PlayerDT> aData = Data_Pool.m_PlayerPool.f_GetAll();

        GameObject oPlayerList = f_GetObject("PlayerList");
        Transform tItem = oPlayerList.transform.GetChild(0);
        int iNum = aData.Count - oPlayerList.transform.childCount;
        if (oPlayerList.transform.childCount < aData.Count)
        {
            for (int i = 0; i < iNum; i++)
            {
                GameObject oNewItem = Instantiate(tItem.gameObject);
                oNewItem.transform.parent = oPlayerList.transform;
            }
        }
        for (int i = 0; i < oPlayerList.transform.childCount; i++)
        {
            oPlayerList.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < aData.Count; i++)
        {
            PlayerDT tPlayerDT = (PlayerDT)aData[i];
            tItem = oPlayerList.transform.GetChild(i);
            tItem.gameObject.SetActive(true);
            Vector3 Pos = tItem.gameObject.transform.localPosition;
            Pos.z = 0;
            tItem.gameObject.transform.localPosition = Pos;
            tItem.transform.localScale = new Vector3(1, 1, 1);
            UpdateItem(tPlayerDT, tItem);
        }

        f_GetObject("PlayerList").SetActive(false);
        f_GetObject("PlayerList").SetActive(true);
    }

    private void UpdateItem(PlayerDT tPlayerDT, Transform tItem)
    {
        UITools.f_SetText(tItem.gameObject, "PlayerId:" + tPlayerDT.m_iId + " Pos:" + tPlayerDT.f_GetPos());
    }

    private void OnDoorDie(object Obj)
    {
        MessageBox.DEBUG("OnDoorDie");

        return;

        if (StaticValue.m_bIsMaster)
        {
            GameSocket.GetInstance().f_GameOver();
        }

        GameEM.TeamType emTeamType = (GameEM.TeamType)Obj;
        if (StaticValue.m_UserDataUnit.m_PlayerDT.f_GetTeamType() == emTeamType)
        {
            MessageBox.DEBUG("Lose");
            f_GetObject("Lose").SetActive(true);
            glo_Main.GetInstance().m_EM_GameStatic = EM_GameStatic.Lost;
        }
        else
        {
            MessageBox.DEBUG("Win");
            SoundPool.GetInstance().f_PlaySound("Sound/勝利");
            f_GetObject("Win").SetActive(true);
            glo_Main.GetInstance().m_EM_GameStatic = EM_GameStatic.Win;
        }
        f_GetObject("BtnLeaveBattle").SetActive(true);
        //}

        glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.DOORDIE, OnDoorDie, null);
    }

    #endregion



    #region 游戏控制(鍵盤測試區) ================================================================
    private int _iDoKeyDown = -99;
    private int iTestBulletId;

    /// <summary>
    /// 创建测试怪物鍵盤
    /// </summary>
    private void TestKey()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //GameObject obj = f_GetGameObj("tmpPos (22)");
            //int iBulletId = 4;
            //BulletDT tBulletDT = (BulletDT)glo_Main.GetInstance().m_SC_Pool.m_BulletSC.f_GetSC(iBulletId);
            //BaseRoleControllV2 tBaseRoleControllV2 = f_GetRoleControl2(0);
            //tBulletDT.fSpeed = 0.01f;
            //BaseBullet tBaseBullet = glo_Main.GetInstance().m_ResourceManager.f_CreateBullet(tBulletDT);
            //tBaseBullet.gameObject.transform.position = obj.transform.position;
            //tBaseBullet.transform.LookAt(tBaseRoleControllV2.transform);
            //iTestBulletId = ccMath.f_CreateKeyId();
            //tBaseBullet.f_Fired(iTestBulletId, iBulletId, GameEM.TeamType.B, 0);
            //return;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            //BaseBullet tBaseBullet = m_BulletPool.f_Get(iTestBulletId);
            //tBaseBullet.f_BeAttack(2);

            //CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
            //tCreateSocketBuf.f_Add(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId);
            //tCreateSocketBuf.f_Add(1);
            //tCreateSocketBuf.f_Add(1);
            //tCreateSocketBuf.f_Add(2);
            //
            //for (int i = 0; i < 16; i++)
            //{
            //    //public int m_iShot;
            //    //public int m_iShotHit;
            //    //public int m_iHeadShot;
            //    //public int m_iHeadShotDie;
            //    //public int m_iShotDie;
            //
            //    tCreateSocketBuf.f_Add(i * 10 + 0);
            //    tCreateSocketBuf.f_Add(i*10 + 1);
            //    tCreateSocketBuf.f_Add(i * 10 + 2);
            //    tCreateSocketBuf.f_Add(i * 10 + 3);
            //    tCreateSocketBuf.f_Add(i * 10 + 4);
            //    tCreateSocketBuf.f_Add(i * 10 + 5);
            //    tCreateSocketBuf.f_Add(i * 10 + 6);
            //}          
            //向验证服务器提交一份游戏结果
            //ccVerifyMain2.GetInstance().f_SubmitGameOverData(tCreateSocketBuf.f_GetBuf());
        }


        if (_iDoKeyDown == -99)
        {
            KeyCode tKeyCode = KeyCode.None;
            if (Input.GetKeyDown(KeyCode.A))
            { //A
                tKeyCode = KeyCode.A;
            }
            if (Input.GetKeyDown(KeyCode.B))
            { //B
                tKeyCode = KeyCode.B;
            }
            if (Input.GetKeyDown(KeyCode.C))
            { //C
                tKeyCode = KeyCode.C;
            }
            if (Input.GetKeyDown(KeyCode.D))
            { //D
                tKeyCode = KeyCode.D;
            }
            if (Input.GetKeyDown(KeyCode.E))
            { //E
                tKeyCode = KeyCode.E;
            }
            if (Input.GetKeyDown(KeyCode.I))
            { //I
                tKeyCode = KeyCode.I;
            }
            if (Input.GetKeyDown(KeyCode.J))
            { //J
                tKeyCode = KeyCode.J;
            }
            if (Input.GetKeyDown(KeyCode.M))
            { //M
                tKeyCode = KeyCode.M;
            }
            if (Input.GetKeyDown(KeyCode.N))
            { //N
                tKeyCode = KeyCode.N;
            }
            if (Input.GetKeyDown(KeyCode.O))
            { //O
                tKeyCode = KeyCode.O;
            }
            if (Input.GetKeyDown(KeyCode.P))
            { //P
                tKeyCode = KeyCode.P;
            }
            if (Input.GetKeyDown(KeyCode.S))
            { //S
                tKeyCode = KeyCode.S;
            }
            if (Input.GetKeyDown(KeyCode.T))
            { //T
                tKeyCode = KeyCode.T;
            }
            if (Input.GetKeyDown(KeyCode.X))
            { //X
                tKeyCode = KeyCode.X;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            { //Z
                tKeyCode = KeyCode.Z;
            }
            if (Input.GetKeyDown(KeyCode.K))
            { //K
                tKeyCode = KeyCode.K;
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            { //右邊數字1
                tKeyCode = KeyCode.Keypad1;
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            { //右邊數字2
                tKeyCode = KeyCode.Keypad2;
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            { //右邊數字3
                tKeyCode = KeyCode.Keypad3;
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            { //右邊數字4
                tKeyCode = KeyCode.Keypad4;
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            { //右邊數字5
                tKeyCode = KeyCode.Keypad5;
            }
            if (Input.GetKeyDown(KeyCode.Keypad6))
            { //右邊數字6
                tKeyCode = KeyCode.Keypad6;
            }
            if (Input.GetKeyDown(KeyCode.Keypad7))
            { //右邊數字7
                tKeyCode = KeyCode.Keypad7;
            }
            if (Input.GetKeyDown(KeyCode.Keypad8))
            { //右邊數字8
                tKeyCode = KeyCode.Keypad8;
            }
            if (Input.GetKeyDown(KeyCode.Keypad9))
            { //右邊數字9
                tKeyCode = KeyCode.Keypad9;
            }
            if (tKeyCode != KeyCode.None)
            {
                _iDoKeyDown = ccTimeEvent.GetInstance().f_RegEvent(0.5f, true, tKeyCode, CallBack_TestKey);
            }
        }
    }

    private void CallBack_TestKey(object Obj)
    {
        KeyCode tKeyCode = (KeyCode)Obj;
        //攻擊測試 -------------------------------------------------------------------------
        if (tKeyCode == KeyCode.O)
        {
           
        }
        else if(tKeyCode == KeyCode.Z)
        {
           
        }
        else if (tKeyCode == KeyCode.C)
        {

        }
        else if (tKeyCode == KeyCode.X)
        {
           

        }
        ccTimeEvent.GetInstance().f_UnRegEvent(_iDoKeyDown);
        _iDoKeyDown = -99;
    }


    /// <summary>
    /// 管理员开始游戏
    /// </summary>
    /// <param name="Obj"></param>
    private void UI_BtnStartGame()
    {
        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Waiting)
        {
            f_StartGame();
        }
        else
        {
            //f_EndGame();
        }
    }

    /// <summary>
    /// 开始游戏命令
    /// </summary>
    public void f_StartGame()
    {        
        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Waiting)
        {
            CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
            tCreateSocketBuf.f_Add(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId);
            GameSocket.GetInstance().f_SendBuf2Force((int)SocketCommand.CTS_ReadyStartGame, tCreateSocketBuf.f_GetBuf());

            UITools.f_SetText(f_GetObject("BtnStartGame"), "StopGame");
        }
        else
        {
            MessageBox.ASSERT("游戏状态错误不能执行开始游戏");
        }
    }

    /// <summary>
    /// 结束游戏命令
    /// </summary>
    private void Callback_GameOver(object Obj)
    {
        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming)
        {
            if (StaticValue.m_bIsMaster)
            {
                //Data_Pool.m_PlayerPool.f_GameOver();
                int iGameTime = (int)(System.DateTime.Now - m_tStartTimeTime).TotalSeconds;
                int tEM_GameResult = (int)Obj;
                _GameMission.f_Reset();
                //CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
                //tCreateSocketBuf.f_Add(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId);
                //tCreateSocketBuf.f_Add(tEM_GameResult);
                //GameSocket.GetInstance().f_SendBuf2Force((int)SocketCommand.CTS_GameOver, tCreateSocketBuf.f_GetBuf());
                glo_Main.GetInstance().m_EM_GameStatic = EM_GameStatic.Waiting;
                List<PlayerDT> aPlayer = Data_Pool.m_PlayerPool.f_GetAll();
                //public int m_iUserId;
                //public int m_iGameResult;
                //public int m_iTime;
                //public int m_iPlayerNum;
                //public stScore[] m_aScore;

                CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
                tCreateSocketBuf.f_Add(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId);
                tCreateSocketBuf.f_Add(tEM_GameResult);
                tCreateSocketBuf.f_Add(iGameTime);
                tCreateSocketBuf.f_Add(aPlayer.Count);
                
                for(int i = 0; i < aPlayer.Count; i++)
                {
                    //public int m_iShot;
                    //public int m_iShotHit;
                    //public int m_iHeadShot;
                    //public int m_iHeadShotDie;
                    //public int m_iShotDie;
                    PlayerScorePool tPlayerScorePool = aPlayer[i].m_PlayerScorePool;
                    tCreateSocketBuf.f_Add(aPlayer[i].m_iId);
                    tCreateSocketBuf.f_Add(tPlayerScorePool.m_iShot);
                    tCreateSocketBuf.f_Add(tPlayerScorePool.m_iShotHit);
                    tCreateSocketBuf.f_Add(tPlayerScorePool.m_iHeadShot);
                    tCreateSocketBuf.f_Add(tPlayerScorePool.m_iHeadShotDie);
                    tCreateSocketBuf.f_Add(tPlayerScorePool.m_iShotDie);
                    tCreateSocketBuf.f_Add(tPlayerScorePool.m_iDie);
                }
                for (int i = aPlayer.Count; i < 16; i++)
                {
                    tCreateSocketBuf.f_Add(0);
                    tCreateSocketBuf.f_Add(0);
                    tCreateSocketBuf.f_Add(0);
                    tCreateSocketBuf.f_Add(0);
                    tCreateSocketBuf.f_Add(0);
                    tCreateSocketBuf.f_Add(0);
                    tCreateSocketBuf.f_Add(0);
                }

                GameSocket.GetInstance().f_SendBuf2Force((int)SocketCommand.CTS_GameOver, tCreateSocketBuf.f_GetBuf());

                //向验证服务器提交一份游戏结果
                //ccVerifyMain2.GetInstance().f_SubmitGameOverData(tCreateSocketBuf.f_GetBuf());


            }

            MissionLog.f_Out();

        }
        //else
        //{
        //    MessageBox.ASSERT("游戏状态错误不能执行结束游戏");  //←死光後跑到這行
        //}

    }



    /// <summary>
    /// 正式开始游戏 (按下 3v3GameServer 開始按鈕後執行的地方)
    /// </summary>
    /// <param name="tCTS_StartGame"></param>
    public void On_Start(CTS_StartGame tCTS_StartGame)
    {

        //驗證：開始遊戲
#if Verify
        VerityTools.f_StartGame();
        MessageBox.DEBUG("驗證開始");
#endif

        if (glo_Main.GetInstance().m_EM_GameStatic == EM_GameStatic.Gaming)
        {
            return;
        }       
        glo_Main.GetInstance().m_EM_GameStatic = EM_GameStatic.Gaming;
        _BattleAI.f_Start();
        _GameControllV2.f_Start();

        MessageBox.DEBUG("戰鬥開始");
        //SoundPool.GetInstance().f_PlaySound("Sound/戰鬥開始2");

        m_tStartTimeTime = System.DateTime.Now;
    }


    private EM_GameResult _EM_GameResult = EM_GameResult.Default;
    /// <summary>
    /// 游戏结束返回大厅
    /// </summary>
    /// <param name="tCTS_GameOver"></param>
    public void On_GameOver(CTS_GameOver tCTS_GameOver)
    {
        if (_EM_GameResult != EM_GameResult.Default)
        {
            return;
        }

        //驗證：遊戲結束
#if Verify
        VerityTools.f_StopGame();
        MessageBox.DEBUG("驗證結束");
#endif

        glo_Main.GetInstance().m_EM_GameStatic = EM_GameStatic.Waiting;
        _BattleAI.f_Stop();
        _GameControllV2.f_Stop();
        _GameMission.f_Reset();

        //GameSocket.GetInstance().f_StopAndClose();
        //ControllSocket.GetInstance().f_StopAndClose();

        _EM_GameResult = (EM_GameResult) tCTS_GameOver.m_iGameResult;
        if (_EM_GameResult == EM_GameResult.Win)
        {
            OpenUI_Win();
        }
        else if (_EM_GameResult == EM_GameResult.Lost)
        {
            MySelfPlayerControll2.staticMySelfPlayerControll2._Lutify.Blend = 1;
            MySelfPlayerControll2.staticMySelfPlayerControll2.DeathreciprocalGameObject.gameObject.SetActive(false);
            OpenUI_Lost();
        }

        //排行榜 (只在勝利時打開)
        if (_EM_GameResult == EM_GameResult.Win) {
            ScoreRank.GetInstance().Receive_GameOverData(tCTS_GameOver);
        }
        


        #if Verify
        //驗證：遊戲結束
        //ccVerifyMain2.GetInstance().On_StopGameBtn();
        #endif

        //tCTS_GameOver.m_aScore;
        MessageBox.DEBUG("游戏结束， " + _EM_GameResult.ToString());
        MessageBox.DEBUG("游戏结束返回大厅");

       

        //ccTimeEvent.GetInstance().f_RegEvent(15, false, null, QuitGame);
    }

    void QuitGame(object Obj)
    {
        MessageBox.DEBUG("强制结束游戏");
        glo_Main.GetInstance().f_Destroy();
    }


    
    /// <summary>
    /// 显示胜利UI
    /// </summary>
    public void OpenUI_Win()
    {
        if (ui_Win.Length > 0)
        {
            for (int i = 0; i < ui_Win.Length; i++)
            {
                ui_Win[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// 显示失败UI
    /// </summary>
    public void OpenUI_Lost()
    {
        if (ui_Lost.Length > 0)
        {
            for (int i = 0; i < ui_Lost.Length; i++)
            {
                ui_Lost[i].SetActive(true);
            }
        }
        //ui_Other[0].SetActive(false);
    }



    public void f_RegRoleDieMission(BaseRoleControllV2 tBaseRoleControl, EM_GameResult tEM_GameResult)
    {
        _GameMission.f_RegRole(tBaseRoleControl, tEM_GameResult);
    }

    private void UI_BtnLeaveBattle()
    {
        MessageBox.DEBUG("游戏结束返回大厅");
        DoDestory();
        SceneManager.LoadScene(GameEM.GameScene.GameMainV2.ToString());
    }
#endregion


    #region 玩家控制 ============================================================================
    public GameObject f_GetPlayerPos(PlayerDT tPlayerDT)
    {
        int iPos = tPlayerDT.f_GetPos();
        if (iPos > m_aPlayerPos.Length)
        {
            MessageBox.DEBUG("场景里没有空余位置。");
            return null;
        }
        return m_aPlayerPos[iPos];
    }
    #endregion



    #region 创建玩家及其它角色 ==================================================================
    public void f_CreateMyselfPlayer(PlayerDT tPlayerDT)
    {
        GameObject Obj = BattleMain.GetInstance().f_GetPlayerPos(tPlayerDT);
        TileNode tTileNode = BattleMain.GetInstance().m_MapNav.f_GetTileNodeForPosition(Obj.transform.position);
        MySelfPlayerControll2 tMySelfPlayerControll2 = m_oMySelfPlayer.GetComponent<MySelfPlayerControll2>();
        tPlayerDT.f_SetPlayerInfor(tMySelfPlayerControll2, null);
        CharacterDT tCharacterDT = (CharacterDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterSC.f_GetSC(1000);
        tMySelfPlayerControll2.f_Init(tPlayerDT.m_iId, new PlayerActionController(tMySelfPlayerControll2), tPlayerDT.f_GetTeamType(), tCharacterDT, tTileNode, tPlayerDT.f_GetHeight());
        tMySelfPlayerControll2.f_SetPos(new Vector3(Obj.transform.position.x, GloData.glo_fPlayerDefaultY, Obj.transform.position.z));
        m_BattleRolePool.f_Save(tMySelfPlayerControll2);
    }

    public void f_CreateOtherPlayer(PlayerDT tPlayerDT)
    {
        if (f_GetRoleControl2(tPlayerDT.m_iId) != null)
        {
            return;
        }
        GameObject Obj = BattleMain.GetInstance().f_GetPlayerPos(tPlayerDT);
        TileNode tTileNode = BattleMain.GetInstance().m_MapNav.f_GetTileNodeForPosition(Obj.transform.position);
        OtherPlayerControll2 tOtherPlayerControll2 = glo_Main.GetInstance().m_ResourceManager.f_CreateOtherPlayer(tPlayerDT.f_GetHeight());
        tPlayerDT.f_SetPlayerInfor(tOtherPlayerControll2, null);
        CharacterDT tCharacterDT = (CharacterDT)glo_Main.GetInstance().m_SC_Pool.m_CharacterSC.f_GetSC(1000);

        tOtherPlayerControll2.f_Init(tPlayerDT.m_iId, new PlayerActionController(tOtherPlayerControll2), tPlayerDT.f_GetTeamType(), tCharacterDT, tTileNode, tPlayerDT.f_GetHeight());
        tOtherPlayerControll2.transform.position = new Vector3(Obj.transform.localPosition.x, GloData.glo_fPlayerDefaultY, Obj.transform.localPosition.z);
        m_BattleRolePool.f_Save(tOtherPlayerControll2);
    }
    
    public void f_SaveRole(BaseRoleControllV2 tRoleControl)
    {
        if (tRoleControl != null)
        {
            m_BattleRolePool.f_Save(tRoleControl);
        }
        else
        {
            MessageBox.ASSERT("f_SaveRole == null");
        }
    }
    #endregion


    #region 销毁玩家及其它角色
    public void f_DestoryOtherPlayer(PlayerDT tPlayerDT)
    {

    }
    #endregion


    #region 老版本功能
  
    public BaseRoleControllV2 f_GetRoleControl2(int iRoleKeyId)
    {
        return m_BattleRolePool.f_Get(iRoleKeyId);
    }
    

    public void f_RoleDie2(BaseRoleControllV2 tRoleControl)
    {
        MessageBox.DEBUG("RoleDie " + tRoleControl.m_iId);
        m_BattleRolePool.f_Die(tRoleControl);

        if (StaticValue.m_bIsMaster)
        {
            _GameMission.f_RoleDie(tRoleControl);
        }

    }
    #endregion


    #region 资源接口----------------------------------------------------------
    private void CreateRole(int iId)
    {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.A, iId, 1126);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    private void CreatePlayerMoster(int iId)
    {
        GameObject TTT = f_GetGameObj("TTT");
        TileNode tTileNode = BattleMain.GetInstance().m_MapNav.f_GetTileNodeForPosition(TTT.transform.position);
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.A, iId, tTileNode.idx);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    private void CreatePlayerRaptor()
    {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.A, 1006, 1026);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    private void CreateBarrel()
    {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.A, 1059, 1026);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    private void CreateTrex()
    {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.Computer, 1005, 1028);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    private void CreatePter()
    {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.Computer, 1009, 1030);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    private void CreateSam()
    {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.A, 1010, 1051);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    private void CreateComputerMonter(int iId)
    {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.Computer, iId, 396);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    public void CreateComputerMonter2(int iId)
    {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.Computer, iId, 425);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    private void CreatePlayerMonter(int iId)
    {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.A, iId, 396);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }

    public void f_CreateMonster(int iUserId, int iCharacterDTId, int iTileNodeId)
    {
        RoleBirthAction tRoleBirthAction = new RoleBirthAction();
        tRoleBirthAction.f_Birth(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId * 100000 + ccMath.f_CreateKeyId(), GameEM.TeamType.Computer, iCharacterDTId, iTileNodeId);
        glo_Main.GetInstance().m_GameSyscManager.f_AddMyAction(tRoleBirthAction);
    }


    /// <summary>
    /// 玩家自己创建怪物
    /// </summary>
    /// <param name="tCharacterDT"></param>
    /// <param name="Pos"></param>
    public void f_CreateRole(int iUserId, CharacterDT tCharacterDT, Vector3 Pos)
    {
        f_PlaySomke(Pos);
        TileNode tTileNode = BattleMain.GetInstance().m_MapNav.f_GetTileNodeForPosition(Pos);
        //f_CreateRole(StaticValue.m_UserDataUnit.m_PlayerDT.m_iId, StaticValue.m_UserDataUnit.m_MySelfPlayerControll, tCharacterDT, tTileNode);
        f_CreateMonster(iUserId, tCharacterDT.iId, tTileNode.idx);
    }
    #endregion



    #region 房間門項目 --------------------------------------------------

    /// <summary>
    /// 門字典取得門資訊
    /// </summary>
    private void SaveDoor() {
        for (int i = 0; i < Door.Length; i++) {
            if(Door[i] == null) {
                continue;
            }
            if (_dicDoor.ContainsKey(Door[i].name)) {
                MessageBox.ASSERT("BattleMain 的門清單裡有相同名字的門：" + Door[i].name);
            }
            else {
                _dicDoor.Add(Door[i].name, Door[i].GetComponent<DoorAnimation>());
            }
        }
    }


    /// <summary>
    /// 獲取指定名稱的門
    /// </summary>
    /// <param name="index"> 要取得的物品名稱 </param>
    public GameObject GetDoor (string tmpName){
        for (int i = 0; i < Door.Length; i++) {
            if (Door[i].name == tmpName){
                return Door[i].gameObject;
            }
        }
        return null;
    }


    /// <summary>
    /// 獲取指定名稱的門 (用字典取得)
    /// </summary>
    /// <param name="tmpName"> 要取得的門的名稱 </param>
    /// <returns></returns>
    public DoorAnimation GetDoor2(string tmpName) {
        if (_dicDoor.ContainsKey(tmpName)) {
            return _dicDoor[tmpName];
        }
        return null;
    }
    #endregion




    //---------------------------------------------------------
    public void f_PlaySomke(Vector3 Pos)
    {
        //Instantiate(m_SummonSmoke, Pos, Quaternion.identity);
    }
    
    public void f_UpdateFps(string ppSQL)
    {
        //MessageBox.DEBUG(ppSQL);
        Text tText = f_GetObject("FpsText").GetComponent<Text>();
        tText.text = ppSQL;         /// "FPS:" + iData + " R:" + tGameSyscStatic.m_iRecvAction + " S:" + tGameSyscStatic.m_iSendAction + " C:" + tGameSyscStatic.m_fCurFrameTime + " N:" + tGameSyscStatic.m_iNetWorkTime;
        Text tText2 = f_GetObject("FpsText2").GetComponent<Text>();
        tText2.text = ppSQL;
    }

    public void f_UpdateAction(string ppSQL)
    {
        Text tText = f_GetObject("ActionText").GetComponent<Text>();
        tText.text = ppSQL;
        MessageBox.DEBUG(ppSQL);
    }


    /// <summary>
    /// 其它玩家的坐标信息更新动作
    /// </summary>
    /// <param name="tPlayerTransformAction"></param>
    public void f_UpdatePlayerAction(PlayerTransformAction tPlayerTransformAction)
    {
        if (tPlayerTransformAction.m_iUserId == StaticValue.m_UserDataUnit.m_PlayerDT.m_iId)
        {
            return;
        }
        PlayerDT tPlayerDT = Data_Pool.m_PlayerPool.f_GetPlayer(tPlayerTransformAction.m_iUserId);
        if (tPlayerDT != null)
        {
            ((OtherPlayerControll2)tPlayerDT.m_PlayerControll).f_UpdatePlayerTransform(tPlayerTransformAction);
        }
    }


    #region UI相关

    /// <summary>
    /// 显示UI文字
    /// </summary>
    /// <param name="aPlayer"></param>
    /// <param name="strText"></param>
    /// <param name="fTime"></param>
    public void f_ShowUIText(string strText, float fTime) {
        GameObject tUIText = f_GetObject("UIText");
        tUIText.SetActive(true);
        Text tText = tUIText.GetComponent<Text>();
        tText.text = strText;

        ccTimeEvent.GetInstance().f_RegEvent(fTime, false, tUIText, Callback_CloseUIText);
    }

    private void Callback_CloseUIText(object Obj){
        GameObject tUIText = (GameObject)Obj;
        tUIText.SetActive(false);
    }

    /// <summary>
    /// 打开显示UI组件
    /// </summary>
    /// <param name="aPlayer"></param>
    /// <param name="strText"></param>
    /// <param name="fTime"></param>
    public void f_UIActionShow(string strUIAction, float fTime) {
        GameObject tUIAction = f_GetObject(strUIAction);
        tUIAction.SetActive(true);
        ccTimeEvent.GetInstance().f_RegEvent(fTime, false, tUIAction, Callback_CloseUIActionShow);
    }

    private void Callback_CloseUIActionShow(object Obj) {
        GameObject tUIAction = (GameObject)(Obj);
        tUIAction.SetActive(false);
    }



    /// <summary>
    /// 場景淡入淡出
    /// </summary>
    /// <param name="value"> 透明度 </param>
    /// <param name="time" > 淡入淡出花費的時間 </param>
    public void SceneFadeTo(int value, float time = 0.5f) {
        UI_SceneFade.DOFade(value, time);
        if (_audioSource == null) {
            return;
        }
        if (_FadeClip != null && value == 1) {
            _audioSource.PlayOneShot(_FadeClip, 1);
        }
    }

    #endregion


    #region 任务公共相关接口
    public void f_RunServerActionState(int iConditionId, int iId) {
        _GameControllV2.f_RunServerActionState(iConditionId, iId);
    }


    public string f_GetParamentData(string strParament){
        return _GameControllV2.f_GetParamentData(strParament);
    }


    public void f_SetParamentData(string strParament, string strData){
        _GameControllV2.f_SetParamentData(strParament, strData);
    }
    #endregion


    #region BattleMain场景物件相关

    public GameObject f_GetGameObj(string strName){
        GameObject oGameObj = f_GetObject(strName);
        if (oGameObj == null) {
            oGameObj = _PositionManager.f_GetPosManagerObject(strName);
            if (oGameObj != null) {
                return oGameObj;
            } else {
                Debug.LogWarning("【警告】游戏场景里的对象未找到！" + strName);
            }
        }
        return oGameObj;
    }
       
    public void f_SetChildForGameObj(GameObject Obj)
    {
        Transform oGameObj = f_GetObject("PosManager").transform;
        Obj.transform.parent = oGameObj;
    }

    #endregion
}

