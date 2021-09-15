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

    public static bool isFeedbacking;
    


    #region 定义串口属性
    public static PortControl _instance;
    //public GUIText Test;
    //定义基本信息
    public string portName = "COM5";//串口名
    public int baudRate = 9600;//波特率
    public Parity parity = Parity.None;//效验位
    public int dataBits = 8;//数据位
   
    public StopBits stopBits = StopBits.One;//停止位
    List<SerialPort> sp = null;
    Thread dataReceiveThread;
    //发送的消息
    string message = "";
    
    
    
    public Text XinLvText;
    public Text xueyangText;
  
    public float timer = 300f;
    public Text countDownText;

    
    
  
  
    [HideInInspector]
    public string starttime;
    private string endtime;
   // public Transform EndPos;
   // public GameObject EndPanel;
   
   
    public bool IsEnd = false;


    bool IsStartGame = false;
    public Text warningText;
    
    
    #endregion
    void Start()
    {
        _instance = this;
        isFeedbacking=false;
      
        dataReceiveThread = new Thread(new ThreadStart(DataReceiveFunction));
      
        dataReceiveThread.IsBackground=true;
          dataReceiveThread.Start();
        starttime = System.DateTime.Now.ToString();
        IsStartGame = true;

        isStartThread=true;

     
        
    }
    public void ReStart(){
         ClosePort();
     //    OpenPort();
           dataReceiveThread = new Thread(new ThreadStart(DataReceiveFunction));
           dataReceiveThread.IsBackground=true;
          dataReceiveThread.Start();
          timer=180;
             starttime = System.DateTime.Now.ToString();
        IsStartGame = true;
        IsEnd=false;
         isStartThread=true;


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
      //  print(time);
        return time;
    }
    private float timeInterval;
    void Update()
    {
  
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
            countDownText.text = "倒计时:" + timer.ToString("0") + "秒";
        }
        else
        {
            timer = -1;
            countDownText.text = "倒计时:" + "0" + "秒";
            if(timer<=0){
                IsEnd=true;
                warningText.text="监测结束！";

            }
        }
        if (IsStartGame && IsEnd == false)
        {
             timeInterval+=Time.deltaTime;
             if(timeInterval>=2){
                 timeInterval=0;
                 XinLvText.text=zzd.ToString();
                xueyangText.text=fsd.ToString();
                 if(single==200){
                 //     warningText.text="she";
                // }
               //  else{
                      warningText.text="请调整脑波和耳机设备,信号弱";
                 }
                 if(single==0){
                      warningText.text="脑波数据读取中...";

                 }
                 if(single==-2){
                   
                       warningText.text=state;

                 }
               

             }

        }
    }
    private bool isPort;
    #region 创建串口，并打开串口
    public void OpenPort()
    {

        sp = new List<SerialPort>();
        string[] strs = SerialPort.GetPortNames();
        for (int i = 0; i < strs.Length; i++)
        {
            isPort = true;
            SerialPort sr = new SerialPort(strs[i], baudRate, parity, dataBits, stopBits);
            try
            {
                sr.Open();
            }
            catch
            {
//                print("有异常");
               // warningText.text="请连接脑波设备...";
                isPort = false;
                sr.Close();
               


            }
            if (!isPort)
            {
                continue;
            }
            else
            {
                sr.ReadTimeout = 500;
         
                sp.Add(sr);
             
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
       isStartThread =false;
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
   private int single=-2;
   private string state="设备连接中...";
    #region 接收数据
    private byte[] buff = new byte[1024];
    private bool   isStartThread =false;
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
                     //   int n =sp[i].BytesToRead;
                        //byte[] buff =new byte[n];
                        sp[i].Read(buff,0,buff.Length);
                         string cou=byteToHexStr(buff);
                         if(cou.Length<=72){
                             continue;
                         }
                        for (int k = 0; k < 16; k++)
                        {
                            if(cou.Substring(0,4)!="AAAA"){
                                cou=cou.Substring(2,cou.Length-2);
                            }
                        }
                        if(cou.Substring(4,2)!="20"){
                            state="数据读取中...";
                            //在这里只处理符合要求的大数据，小数据不处理
                            continue;
                        }
                        else{
                             if (cou.Length>=70)
                                 {      
                       cou= cou.Substring(6,64);
              // print(cou);
               if(cou.Length>=64){
                    single=Convert.ToInt32(Hex2Ten(cou.Substring(2,2)));
                     
                         if(single==200){
                           //  state="请佩戴好脑波设备";
                         }
                         else{
                               zzd=Convert.ToInt32(Hex2Ten(cou.Substring(58,2)));
                             fsd=Convert.ToInt32(Hex2Ten(cou.Substring(62,2)));
                             if(!isFeedbacking){
                                    isFeedbacking=true;
                             }                     
                         //    state="设备已连接成功!";
                         }                        
                                print("zzd："+zzd+"---"+"fsd："+fsd+"---"+"信号："+single);
                    }                           
                        }
                        }
                    }
                    catch (Exception ex)
                    {
                      // warningText.text = "设备连接失败，请重新连接后进入场景！";
                        sp[i].Close();
                        
                         // isStartThread=false;
                        if (ex.GetType() != typeof(ThreadAbortException))
                        {
                        }
                    }
                }


            }
            Thread.Sleep(10);
        }
        #endregion
    }
    private int zzd=0;
    private int fsd=0;
       public static string Hex2Ten(string hex)
        {
            int ten = 0;
            for (int i = 0, j = hex.Length - 1; i < hex.Length; i++)
            {
                ten += HexChar2Value(hex.Substring(i, 1)) * ((int)Math.Pow(16, j));
                j--;
            }
            return ten.ToString();
        }
         public static int HexChar2Value(string hexChar)
        {
            switch (hexChar)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    return Convert.ToInt32(hexChar);
                case "a":
                case "A":
                    return 10;
                case "b":
                case "B":
                    return 11;
                case "c":
                case "C":
                    return 12;
                case "d":
                case "D":
                    return 13;
                case "e":
                case "E":
                    return 14;
                case "f":
                case "F":
                    return 15;
                default:
                    return 0;
            }
        }
     public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                  //  if(bytes[i]!=0){
                        returnStr += bytes[i].ToString("X2");
                 //   }
                   
                }
            }
            return returnStr;
        }


    #region 发送数据
    public void WriteData(string dataStr)
    {
        //if (sp.IsOpen)
        //{
        //    sp.Write(dataStr);
        //}
    }
   
    #endregion
}
