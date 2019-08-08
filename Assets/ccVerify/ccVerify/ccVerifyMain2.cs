using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ccVerifySDK;

/*
//协议操作结果
    public enum eVerifyMsgOperateResult
    {

        /// <summary>
        /// 操作成功
        /// </summary>
        OR_Succeed = 0, // 成功

        OR_LoginEro = 1,
        /// <summary>
        /// 帐号失效
        /// </summary>
        OR_AccountDisable,

        /// <summary>
        /// 帐号错误
        /// </summary>
        OR_AccountEro,

        /// <summary>
        /// 登陆DLL失败
        /// </summary>
        OR_LoginDLLEro,

        /// <summary>
        /// 未登陆
        /// </summary>
        OR_NoLogin = 5,

        /// <summary>
        /// 电脑硬件信息错误
        /// </summary>
        OR_PcError,

        /// <summary>
        /// 电脑信息已经注册
        /// </summary>
        OR_PcIsRegedited,

        /// <summary>
        /// 未登陆注册电脑信息失败
        /// </summary>
        OR_LoginEroPcError,

        /// <summary>
        /// 数据验证失败
        /// </summary>
        OR_DataVerifyFail,

        /// <summary>
        /// 电脑信息未注册，注册电脑信息
        /// </summary>
        OR_NeedRegPcInfor = 10,

        /// <summary>
        /// 游戏次数已使用完
        /// </summary>
        OR_GameTimesIsLimit,

        /// <summary>
        /// 未找到游戏信息
        /// </summary>
        OR_NoFindGameInfor = 12,

        /// <summary>
        /// 未找到电脑信息
        /// </summary>
        OR_NoFindPcInfor,

        /// <summary>
        /// 授权电脑已满
        /// </summary>
        OR_RegeditPcFull = 14,

        /// <summary>
        /// 授权模式错误
        /// </summary>
        OR_GameTimesNoName,

        /// <summary>
        /// 与授权游戏不一致
        /// </summary>
        OR_GameVerifyGameIdEro,

        //客户端专用提示
        OR_Error_WIFIConnectTimeOut = 993, //WIFI网络未开
        OR_Error_ConnectTimeOut = 994, //连接超时
        OR_Error_CreateAccountTimeOut = 995, //注册超时
        OR_Error_LoginTimeOut = 996, //登陆超时
        OR_Error_ExitGame = 997, //游戏出错，强制玩家离开
        OR_Error_ServerOffLine = 998, //服务器未开启
        OR_Error_Disconnect = 999, //游戏断开连接
        OR_Error_Default = 10000, //操作失败

    }
*/

public class ccVerifyMain2 : UIFramwork
{
    Text _StaticText;
    [Rename("遊戲id")]     public int _iGameId;
    [Rename("帳號輸入欄")] public InputField Account;
    [Rename("密碼輸入欄")] public InputField Password;
    [Rename("下方小圓點")] public Text _GloMainStaticText;
    [Rename("自動登入")]   public bool useAutologin;


    private static ccVerifyMain2 _Instance = null;
    public static ccVerifyMain2 GetInstance() {
        return _Instance;
    }


    protected override void f_CustomAwake()  {
        base.f_CustomAwake();
        
        ccVerityService.GetInstance();
        _Instance = this;
        MessageBox.f_OpenLog();

        //取得紀錄的帳密
        Account.text  = PlayerPrefs.GetString("Account");
        Password.text = PlayerPrefs.GetString("Password");
    }

   

    #region UI消息
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        
        _StaticText = f_GetObject("StaticText").GetComponent<Text>();
        f_GetObject("VerifyPcGameBtn").SetActive(false);
        f_GetObject("RegeditPcGameBtn").SetActive(false);
        f_GetObject("StarGameBtn").SetActive(false);
        f_GetObject("StopGameBtn").SetActive(false);
        //f_GetObject("GameTimeGameBtn").SetActive(false);
        //f_GetObject("GetGamesBtn").SetActive(false);
        //f_GetObject("CheckCanStartGameBtn").SetActive(false);
        //f_GetObject("GetPcName").SetActive(false);

        f_RegClickEvent("LoginBtn", On_LoginBtn);
        //f_RegClickEvent("VerifyPcGameBtn", On_VerifyPcGameBtn);
        f_RegClickEvent("RegeditPcGameBtn", On_RegeditPcGameBtn);
        f_RegClickEvent("StarGameBtn", On_StarGameBtn);
        f_RegClickEvent("StopGameBtn", On_StopGameBtn);
        //f_RegClickEvent("GameTimeGameBtn", On_GameTimeGameBtn);
        f_RegClickEvent("GetGamesBtn", On_GetGamesBtn);
        f_RegClickEvent("CheckCanStartGameBtn", On_CheckCanStartGameBtn);
        f_RegClickEvent("GetPcName", On_GetPcName);
        Account.text = PlayerPrefs.GetString("Account");
        Password.text = PlayerPrefs.GetString("Password");

        if (useAutologin) {
            StartCoroutine("Autologin");
        }
        
    }
    #endregion


    /// <summary>
    /// 自動登入
    /// </summary>
    IEnumerator Autologin() {
        yield return new WaitForSeconds(5);
        On_LoginBtn();
    }


    /// <summary>
    /// 登入
    /// </summary>
    public void On_LoginBtn() {
        Log("Login");
        if (CheckAndUdpateGameId()) {
            Text LoginName = f_GetObject("LoginName").transform.Find("Text").GetComponent<Text>();
            Text LoginPwd  = f_GetObject("LoginPwd").transform.Find("Text").GetComponent<Text>();
            ccVerityService.GetInstance().f_Login(GloData.glo_iGamePlayerTime, Account.text, Password.text, GloData.glo_iGameId, Callback_LoginRet);
        }
    }


    /// <summary>
    /// 登陆认证系统回调参考实例,
    /// </summary>
    /// <param name="Obj"></param>
    private void Callback_LoginRet(eVerifyMsgOperateResult teMsgOperateResult)
    {
        if (teMsgOperateResult == eVerifyMsgOperateResult.OR_Succeed) {
            Log("可以开始游戏");

            f_GetObject("LoginPanel").SetActive(false);
            f_GetObject("StarGameBtn").SetActive(true);
            f_GetObject("GetGamesBtn").SetActive(true);
            f_GetObject("CheckCanStartGameBtn").SetActive(true);
            f_GetObject("GetPcName").SetActive(true);
            f_GetObject("StopGameBtn").SetActive(true);
            f_GetObject("VerifyPcGameBtn").SetActive(false);
            f_GetObject("RegeditPcGameBtn").SetActive(false);
            PlayerPrefs.SetString("Account", Account.text);   //紀錄輸入的帳密
            PlayerPrefs.SetString("Password", Password.text); //紀錄輸入的帳密
            On_CheckCanStartGameBtn();


        }
        else if (teMsgOperateResult == eVerifyMsgOperateResult.OR_NoFindPcInfor 
            ||  teMsgOperateResult == eVerifyMsgOperateResult.OR_NoFindGameInfor) {
            /// 未找到游戏信息   OR_NoFindGameInfor = 12,
            /// 未找到电脑信息   OR_NoFindPcInfor = 13,
            //这二种错误码说明电脑的硬件未注册，遇到调用注册接口进行注册
            f_GetObject("LoginPanel").SetActive(false);
            f_GetObject("RegeditPcGameBtn").SetActive(true);
            f_GetObject("VerifyPcGameBtn").SetActive(false);
        }
        else {
            Log("登陆认证系统失败 " + teMsgOperateResult.ToString());
            ccVerityService.GetInstance().f_Close();
        }
    }



    public void On_GetPcName() {
        Log("On_GetPcName");
        f_GetPcName(Callback_VerifyPcInfor);
    }

    public void On_RegeditPcGameBtn() {
        Log("On_RegeditPcGameBtn");
        ccVerityService.GetInstance().f_RegPcInfor(Callback_LoginRet);
    }

    public void On_CheckCanStartGameBtn() {
        Log("On_CheckCanStartGameBtn");
        ccVerityService.GetInstance().f_CheckCanStartGame(Callback_CheckCanStartGame);
    }
    private void Callback_CheckCanStartGame(eVerifyMsgOperateResult teMsgOperateResult) {
        if (teMsgOperateResult == eVerifyMsgOperateResult.OR_Succeed) {
            MessageBox.DEBUG("可以开始游戏");
            Log("載入遊戲場景...");
            Connect();
        }
        else {
            Log("不能结束游戏" + teMsgOperateResult.ToString());
        }
    }



    #region 登入流程 (原本寫在 Launch.cs)

    public void Connect() {
        MessageBox.DEBUG("Connect");
        GameSocket.GetInstance().f_Login(Callback_LoginSuc);
        ControllSocket.GetInstance().f_Login(Callback_GameControllLoginSuc);
    }

    private void Callback_GameControllLoginSuc(object Obj) {
        MessageBox.DEBUG("GameControll登陆");
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult) Obj;
        if (teMsgOperateResult == (int)eMsgOperateResult.OR_Succeed) {
            MessageBox.DEBUG("GameControll登陆成功");
        }
        else {
            MessageBox.DEBUG("登陆失败 " + teMsgOperateResult.ToString());
        }

    }


    private void Callback_LoginSuc(object Obj) {
        MessageBox.DEBUG("GameStep登陆");
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)Obj;
        if (teMsgOperateResult == (int)eMsgOperateResult.OR_Succeed) {
            MessageBox.DEBUG("GameStep登陆成功");
            if (GloData.glo_iGameModel == 1) {
                StaticValue.m_bIsMaster = true;
            }
            else {
                StaticValue.m_bIsMaster = false;
            }

            //進入 BattleMain 場景
            SceneManager.LoadScene(GameEM.GameScene.BattleMain.ToString());
        }
        else {
            MessageBox.DEBUG("登陆失败 " + teMsgOperateResult.ToString());
        }
    }



    /// <summary>
    /// glo_Main 讀取完畢的提示 (給 glo_Main.cs 的 Callback_LoadResSuc 調用 )
    /// </summary>
    public void f_Show_GloMainStatic() {
        _GloMainStaticText.color = Color.green;
    }

    #endregion


    void Update() {
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.Alpha0)) {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.DeleteAll();
            Account.text = "";
            Password.text = "";
            Log("帳密記憶已清除");
        }
    }


    private void Log(string strText) {
        _StaticText.text = strText;
        MessageBox.DEBUG(strText);
    }



    #region 測試區
    [HideInInspector]
    public int testa = 0;

    /// <summary>
    /// 測試按鍵：開始遊戲
    /// </summary>
    public void On_StarGameBtn() {
        Log("On_StarGameBtn");
        testa = Random.Range(0, 999999);
        ccVerityService.GetInstance().f_StartGame(testa.ToString("000000"));
    }


    /// <summary>
    /// 測試按鍵：結束遊戲
    /// </summary>
    public void On_StopGameBtn() {
        ccVerityService.GetInstance().f_StopGame(testa.ToString("000000"));
    }


    /// <summary>
    /// 測試按鍵：計次(?)
    /// </summary>
    public void On_GameTimeGameBtn() {
        ccVerityService.GetInstance().f_SubmitData(testa.ToString("000000"), 5, Callback_Submit);
    }

    private void Callback_Submit(eVerifyMsgOperateResult teMsgOperateResult, string strKey = "") {
        //服务器确认数据后返回的提交数据的KEY
        if (teMsgOperateResult != eVerifyMsgOperateResult.OR_Succeed) {
            Log("SubmitData 游戏开始数据提交失败 " + ccVerityService.GetInstance().f_GetErroCode(teMsgOperateResult));
        }
        else {
            Log("数据提交成功 " + ccVerityService.GetInstance().f_GetSubmitDataCount());
            //ccVerityService.GetInstance().f_StopGame(Callback_StopReturn);
        }        
    }
          

    public void On_GetGamesBtn() {
        Log("On_GetGamesBtn");
        ccVerityService.GetInstance().f_GetGamePlayTimes(Callback_GetGamePlayTimes);
    }

    private void Callback_GetGamePlayTimes(object Obj) {
        int iTimes = (int)Obj;
        Log("电脑可玩次数 " + iTimes);
    }



    /// <summary>
    /// 測試按鍵：開始+計次+結束
    /// </summary>
    public void test_verify_button() {
        StartCoroutine(MyCoroutineFunction2());
    }
    IEnumerator MyCoroutineFunction2() {
        for (var i = 0; i < 1000; i++) {
            On_StarGameBtn();
            Debug.Log("開始");
            yield return new WaitForSeconds(0.5f);
            On_GameTimeGameBtn();
            Debug.Log("計次");
            yield return new WaitForSeconds(0.5f);
            On_StopGameBtn();
            Debug.Log("結束");
            yield return new WaitForSeconds(0.5f);
        }

    }
    #endregion


    #region 更新GameId
    /// <summary>
    /// 取得遊戲id
    /// </summary>
    /// <returns></returns>
    private bool CheckAndUdpateGameId() {
        Text GameIdText = f_GetObject("GameId").transform.Find("Text").GetComponent<Text>();
        //int iGameId = ccU3DEngine.ccMath.atoi(GameIdText.text);
        GameIdText.text = _iGameId.ToString();
        if (_iGameId > 0) {
            GloData.glo_iGameId = _iGameId;
            return true;
        }
        MessageBox.ASSERT("GameId 错误" + GameIdText.text);
        return false;
    }    
    #endregion


    #region 获得机器名

    public void f_GetPcName(ccCallback tccCallback)
    {
        VerifyGameSocket.GetInstance().f_GetPcName(GloData.glo_iGameId, tccCallback);
    }

    private void Callback_VerifyPcInfor(object Obj)
    {
        string strName = (string)Obj;
        GloData.glo_strPcName = strName;
        Log("机器名：" + strName);
    }


    #endregion


}