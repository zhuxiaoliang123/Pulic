using UnityEngine;
using System.Collections;
using System.IO.Ports;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using UnityEngine.UI;
using DG.Tweening;
using Valve.VR;

public class PortControl : MonoBehaviour
{

    public string sceneName;
    public static bool isFeedbacking; //是否开启指脉连接
    ///当任务模式时 ，个体将不能对场景进行选择，场景中根据指脉数据（一段时间内的平均值，进行自动跳转）
    public static bool isTraning;     //是否是任务模式 

    #region 定义串口属性
    public static PortControl _instance;

    
    //public GUIText Test;
    //定义基本信息
    //public string portName = "COM5";//串口名
    public int baudRate = 115200;//波特率
    public Parity parity = Parity.None;//效验位
    public int dataBits = 8;//数据位
   
    public StopBits stopBits = StopBits.One;//停止位
    List<SerialPort> sp = null;
    Thread dataReceiveThread;
    //发送的消息
   // public List<byte> listReceive = new List<byte>();
   // char[] strchar = new char[100];//接收的字符信息转换为字符数组信息
  
    public Text XinLvText;
    public int Heart;
    public Text XunYangText;
    byte[] buffer = new byte[1024];
    int bytes = 0;
    public int heartRate = 0;
    public int OxRate = 0;
   // public float timer = 180f;
   // public Text countDownText;


    
    
    
 
    [HideInInspector]
    public string starttime;
    
   // public Text EndPanelText;
   
    private bool IsEnd = false;

    private List<int> heartList = new List<int>();
    private List<int> oxList = new List<int>();
    int totleHeartlist;
    int totleOxlist;
    public int averageHeartlist;
    public int maxHeartList;
    public int mixHeartList;
    int averageOxlist;
    public float rate = 3f;
   // public float timerlength = 0f;
    bool IsStartGame = false;
    public Text warningText;

    public Button reconnectBtn;
   
    public void ReConnectPort(){
       reconnectBtn.interactable=false;
      warningText.text = "指脉设备连接中...";
       isFeedbacking=false;
       dataReceiveThread.Abort();
        dataReceiveThread = new Thread(new ThreadStart(DataReceiveFunction));
        dataReceiveThread.Start();
        starttime = System.DateTime.Now.ToString();
        IsStartGame = true;

        Invoke("ShowMsg",25);

    }
    void ShowMsg(){
        if(!isFeedbacking){
            warningText.text = "指脉设备连接失败，请检查设备状态";
            reconnectBtn.interactable=true;
        }
        else{
             warningText.text = "指脉设备连接成功";
        }
       
    }
  
    #endregion
    private float TimeInterval=0;
    public static int startIndex;
      public static TrainItem trainItem;
      public static List<string >sceneList;
    public void LoadScneneInTraing(){
        startIndex++;
        if(startIndex>=sceneList.Count){
            DoAnimMgr.ins.ShowTrainOver();
            isTraning=false;
            return;

        }
        else{
                PlayerPrefs.SetString("LoadScene",sceneList[PortControl.startIndex]);
                 
          UnityEngine.SceneManagement.SceneManager.LoadScene("LoadScene");
        }
        



    }
    void Start()
    {

       _instance = this;

         warningText.text = "指脉设备连接中,请等待...";
       isTraning=false;
       isFeedbacking=false;
     //   portName = GetComponent<XMLCtrl>().GetData();
        //   if(portName.Length>4){
        //    portName="\\\\?\\"+portName;
        //}

    
        dataReceiveThread = new Thread(new ThreadStart(DataReceiveFunction));
        dataReceiveThread.Start();
        starttime = System.DateTime.Now.ToString();
        IsStartGame = true;

        Invoke("ShowMsg",25);

    
    }


    public string GetCurrenTime()
    {
        float second = 0;
        float minute=0;
        float hour=0;
        float timeInterval = 0;
        timeInterval+= Time.deltaTime;
        if (timeInterval>=1)
        {
            second++;
        }
        if (second>=60)
        {
            minute++;
        }
        if (minute>=60)
        {
            hour++;
        }
        if (hour>99)
        {
            hour = 0;
        }
        string timeStr = string.Format("{0:D2}:{1:D2}:{2:D2}",hour,minute,second);
        return timeStr;

    }


    public string GetTime(int miao)//得到时分秒的数目
    {
        string time = "";
        if(miao<60)
        {
            time = "0时" + "0分" + miao.ToString() + "秒";
        }
        else if(miao<3600)
        {
            time = "0时" + (miao / 60).ToString() + "分" + (miao % 60).ToString() + "秒";
        }
        else
        {
            time = (miao / 3600).ToString() + "时" + (miao % 3600 / 60).ToString() + "分" + (((miao %3600)% 60)).ToString() + "秒";
        }
       // print(time);
        return time;
    }
    void Update()
    {
       // warningText.text = warningstring;
        // if (timer >= 0)
        // {
        //     timer -= Time.deltaTime;
        //     countDownText.text = "倒计时:" + timer.ToString("0") + "秒";
        // }
        // else
        // {
        //     timer = -1;
        //     countDownText.text = "倒计时:" + "0" + "秒";
        // }
        if (IsStartGame && IsEnd == false)
        {
            // timerlength += Time.deltaTime;
            // if(timerlength<=1){
            //     return;

            // }
           

            if (Int32.Parse(buffer[3].ToString()) != 127 && Int32.Parse(buffer[4].ToString()) != 127)
            {
            
                   if(isTraning){
                TimeInterval+=Time.deltaTime;
                if(TimeInterval>20){
                    //当平均值大于85时，跳转场景
                     if(averageHeartlist>=85){
                       LoadScneneInTraing();
                     }
                     TimeInterval=0;

                }

            }
                if (rate <= 0)
                {
                    XinLvText.text = buffer[3].ToString();
                    Heart=int.Parse(buffer[3].ToString());
                    heartList.Add(Int32.Parse(buffer[3].ToString()));

                    XunYangText.text = buffer[4].ToString();
                    oxList.Add(Int32.Parse(buffer[4].ToString()));
                    heartList.Sort();

                    oxList.Sort();
                 // print("最小心率:" + heartList[0] + "    " + "最大心率:" + heartList[heartList.Count - 1]);
                    maxHeartList = heartList[heartList.Count - 1];
                    mixHeartList = heartList[0];
                 // print("最小血氧:" + oxList[0] + "    " + "最大血氧:" + oxList[oxList.Count - 1]);
                    for (int i = 0; i < heartList.Count; i++)
                    {
                        totleHeartlist = totleHeartlist + heartList[i];
                    }
                    averageHeartlist = totleHeartlist / heartList.Count;
                    totleHeartlist = 0;
                 // print("平均心率:" + averageHeartlist);
                    for (int i = 0; i < oxList.Count; i++)
                    {
                        totleOxlist = totleOxlist + oxList[i];
                    }
                    averageOxlist = totleOxlist / oxList.Count;
                 // print("平均血氧:" + averageOxlist);
                    totleOxlist = 0;
                    
                    
                    rate = 3f;
                }
                else
                {
                    rate -= Time.deltaTime;
                }
                    heartRate = Int32.Parse(buffer[3].ToString());
                    OxRate = Int32.Parse(buffer[4].ToString());
                if (heartRate != 0 && OxRate != 0)
                {
                    if(!string.IsNullOrEmpty(warningText.text)){
                        warningText.text = "";
                    }
                     
                        isFeedbacking=true;
                        reconnectBtn.interactable=false;
                    if (heartRate >= 61 && heartRate <= 85)
                    {
                        // if (sceneName=="FanKui")
                        // {
                        //     if (tweenAn != null)
                        //     {
                        //         tweenAn.Kill();
                        //         tweenAn = null;
                        //     }
                        //     if (tweenLinag == null)
                        //     {
                        //         tweenLinag = skyBox.DOColor(liang, 20);
                        //         SteamVR_Fade.Start(liang,20);
                        //     }

                        // }
                        // if (sceneName=="Cat")
                        // {
                        //     if (tweenNear!=null)
                        //     {
                        //         tweenNear.Kill();
                        //         tweenNear = null;
                        //     }
                        //     if (tweenFar==null)
                        //     {
                        //         cat.LookAt(farPos);
                        //         cat.DOMove(farPos.position,60);

                        //     }

                        // }
                        // if (sceneName=="Lift")
                        // {
                        //     // left.position = far0.position;
                        //     // right.position = far1.position;

                        // }
                        // if(sceneName=="PaoPao"){
                        //     BalloonScene.isPlaying=true;

                        // }






                    }
                    else
                    {
                        // if (sceneName=="FanKui")
                        // {
                        //     if (tweenLinag != null)
                        //     {
                        //         tweenLinag.Kill();
                        //         tweenLinag = null;

                        //     }
                        //     if (tweenAn == null)
                        //     {
                        //         tweenAn = skyBox.DOColor(an, 20);
                        //       SteamVR_Fade.Start(an,20);
                        //     }

                        // }
                        // if(sceneName=="PaoPao"){
                        //     BalloonScene.isPlaying=true;

                        // }

                        // if (sceneName == "Cat")
                        // {
                        //     if (tweenFar != null)
                        //     {
                        //         tweenFar.Kill();
                        //         tweenFar = null;
                        //     }
                        //     if (tweenNear == null)
                        //     {
                        //         cat.LookAt(nearPos);
                        //         cat.DOMove(nearPos.position, 60);

                        //     }

                        // }
                        // if (sceneName == "Lift")
                        // {
                        //     // left.position = near0.position;
                        //     // right.position = near1.position;

                        // }




                   

                    }
                }
                else{
                   //  warningText.text = "指脉设备连接中,请等待...";
                }
            }
            else
            {
                      if(sceneName=="PaoPao"){
                            BalloonScene.isPlaying=false;

                        }
                        reconnectBtn.interactable=true;
                warningText.text = "指脉设备连接失败，请检查设备状态";
                XinLvText.text="0.00";
                XunYangText.text="0.00";
            
             //   isFeedbacking=false;
            }
          //  timerlength=0;
        }
        else
        {
            // if (HaiTun!=null)
            // {
            //     HaiTun.transform.Translate(Vector3.down * Time.deltaTime * 0);
            // }
         
            // for (int i = 0; i < YunSprite.Length; i++)
            // {
            //     YunSprite[i].transform.Translate(Vector3.down * Time.deltaTime * 0);
            // }
        }
    }
    private bool isPort;
    #region 创建串口，并打开串口
    public void OpenPort()
    {

        // }
        sp = new List<SerialPort>();
     //     string [] strs = new string[]{"COM5","COM6","COM8","COM7"};
        string[] strs = SerialPort.GetPortNames();
        for (int i = 0; i < strs.Length; i++)
        {
            isPort = true;
        // if(strs[i].Length>4){
        //     strs[i] ="\\\\?\\"+strs[i];

        // }
            SerialPort sr = new SerialPort(strs[i], baudRate, parity, dataBits, stopBits);
            try
            {
                sr.Open();
             
            }
            catch
            {
                print("有异常");
                isPort = false;
                sr.Close();
               


            }
            if (!isPort)
            {
                continue;
            }
            else
            {
                sr.ReadTimeout = 400;
               // print(strs[i]);
                sp.Add(sr);
                // break;
            }
        }
    }
    #endregion



    #region 程序退出时关闭串口
    void OnApplicationQuit()
    {
        ClosePort();
    }
    private void OnDestroy()
    {
        ClosePort();
    }
    public void ClosePort()
    {
        try
        {
            IsEnd = true;
            for (int i = 0; i < sp.Count; i++)
            {
                if (sp[i] != null)
                {
                    sp[i].Close();
                }

            }

            dataReceiveThread.Abort();
        }
        catch (Exception ex)
        {
            // warningtext.text = ex.Message;
        }

    }
    #endregion


    /// <summary>
    /// 打印接收的信息
    /// </summary>
    // void PrintData()
    // {
    //     for (int i = 0; i < listReceive.Count; i++)
    //     {
    //         strchar[i] = (char)(listReceive[i]);
    //         str = new string(strchar);
    //     }
    //     Debug.Log(str);
    // }

  
    void DataReceiveFunction()
    {
        OpenPort();
        while (true)
        {
            for (int i = 0; i < sp.Count; i++)
            {
                if (sp[i] != null && sp[i].IsOpen)
                {
                    try
                    {
                        bytes = sp[i].Read(buffer, 0, buffer.Length);//接收字节
                        if (bytes == 0)
                        {
                           // print(bytes+"长度");
                            continue;
                        }
                        else
                        {
                            
                           // string strbytes = Encoding.Default.GetString(buffer);

                          //  print(buffer[3]+"    "+buffer[4]);

//                            Debug.Log(strbytes);
                        }
                    }
                    catch (Exception ex)
                    {
//                         warningText.text = "操作有误，请联系管理员";
                        sp[i].Close();
                    //    warningstring = "操作有误，请联系管理员";
                        if (ex.GetType() != typeof(ThreadAbortException))
                        {

                        }
                    }
                }


            }
            Thread.Sleep(10);
        }

    }

  
    public void WriteData(string dataStr)
    {
        //if (sp.IsOpen)
        //{
        //    sp.Write(dataStr);
        //}
    }
   
    
}
