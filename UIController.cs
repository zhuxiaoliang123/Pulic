using ArduinoBluetoothAPI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/*********
作者:zhuxiaoliang
邮箱:
功能:蓝牙模块的核心交互模块
*********/
namespace Buletooth
{
    public class UIController : MonoBehaviour
    {
        private BluetoothHelper _m_helper;

 
      
        public Text m_log_txt;         // 输入的日志信息

        public Text xinlv;
        public Text xueyang;
        private float timeInterver;
        public AzureDemoUIController azure;
 
        // 消息处理的回调
        // Action<string> _m_msgHandler;
        // public void AddMsgHandler(Action<string> handler)
        // {
        //     _m_msgHandler = handler;
        // }
 
        void Start()
        {
            buffer =new byte [24];
            index=0;
            // 添加连接按钮的事件
            // m_connect_btn.onClick.AddListener(() =>
            // {
            //     if (!string.IsNullOrEmpty(m_deviceName_ipt.text))
            //     {
            //         Log("开始连接蓝牙:" + m_deviceName_ipt.text);
            //         // 设置连接的设备名字
            //         _m_helper.setDeviceName(m_deviceName_ipt.text);
            //         // 开始连接
            //         _m_helper.Connect();
            //     //             _m_helper = BluetoothHelper.GetInstance();
            //     // // 打开蓝牙
            //     // _m_helper.EnableBluetooth(true);
            //     // // 设置收发字符的长度，这里是重点，不设置则接，发 不了消息，要到数据缓存的一定的量才一次性发送
            //     // _m_helper.setFixedLengthBasedStream(1);
            //     }
            // });
          

            // 添加断开连接按钮的事件
           // m_disconnect_btn.onClick.AddListener(Disconnect);
 
            // 添加日志清除按钮的事件
            // m_clearLog_btn.onClick.AddListener(() =>
            // {
            //     m_log_txt.text = "日志";
            // });
 
            // 隐藏面板操作事件监听
            // m_hideLog_btn.onClick.AddListener(() =>
            // {
            //     gameObject.SetActive(false);
            // });
 
            try
            {
                // 获取 BluetoothHelper 实例
                if(_m_helper!=null){
                    
                  _m_helper.setDeviceName("BerryMed");
                 //   _m_helper.setDeviceName("BleModuleA");
                 // print("我是存在的！！！");
              
             _m_helper.Connect();
                    return;

                }
                _m_helper = BluetoothHelper.GetInstance();
            
               // _m_helper.OnScanEnded+=OnScanEnded;
                BluetoothHelper.BLE=true;
                // 打开蓝牙
                _m_helper.EnableBluetooth(true);
                // 设置收发字符的长度，这里是重点，不设置则接，发 不了消息，要到数据缓存的一定的量才一次性发送
                _m_helper.setFixedLengthBasedStream(24);
                
                
                // 连接成功的回调函数
                _m_helper.OnConnected += () =>
                {
                    Log("连接成功");
                    // 连接成功，开始监听消息
                    _m_helper.StartListening();
                };
 
                // 连接失败的回调函数
                _m_helper.OnConnectionFailed += () =>
                {
                    Log("连接失败,请匹配蓝牙设备");
                    Disconnect();
                };
 
                // 没有找到设备的回调函数
                _m_helper.OnServiceNotFound += serviceName =>
                {
                    Log("没有找到设备:" + serviceName);
                    // 断开连接
                    Disconnect();
                };
 
                // 接收到消息的回调函数
                
                _m_helper.OnDataReceived += ListenData;
            
               
            }
            catch (Exception e)
            {
                Log("连接异常：" + e.Message);
                Disconnect();
            }

           _m_helper.setDeviceName("BerryMed");
           //   _m_helper.setDeviceName("BleModuleA");
            _m_helper.Connect();
        }
        private bool isReceive;
        public Tween tween;
        private void ListenData(){
                   isReceive=true;
                    byte [] bs =_m_helper.ReadBytes();
                  //  string res =System.Text.Encoding.UTF8.GetString(bs);

                   // LogLine(bs[3].ToString()+" "+bs[4].ToString()+"/");
                  //  _m_msgHandler(_m_helper.Read());
                  for (int i = 0; i < bs.Length; i++)
                  {
                      buffer[index]= bs[i];
                      index++;
                      if(index==buffer.Length){
                          spo2=buffer[4];
                          heat=buffer[3]|((buffer[2]& 0x40)<<1); 
                         // LogLine("脉搏："+heat.ToString()+"血氧："+spo2.ToString()+"    /");    
                          index=0;

                      }
                  }
            
        }
        private void Update()
        {
         //  if(isReceive){
               timeInterver+=Time.deltaTime;
               if(timeInterver>=3){
                   if(heat>=65&heat<=105){
                    xinlv.text=heat.ToString();
                    if(heat>=61&heat<=85){
                        if(tween==null){
                            tween=azure.timelineSlider.DOValue(14.5f,3).OnComplete(()=>tween=null);
                        }
                     
                    }
                    else{
                        if(tween==null){
                            tween=azure.timelineSlider.DOValue(5f,3).OnComplete(()=>tween=null);
                        }
                    }

                   }
                   if(spo2>=65&spo2<=105){
                         xueyang.text=spo2.ToString();
                   }
                  timeInterver=0;
                  
               }
             

         //  }
        }
    private int spo2;
    private int heat;
    private bool isScanning;
    private int index;
    private bool isConnecting;
    private byte[] buffer ;
    private LinkedList<BluetoothDevice> devices;
    void OnScanEnded(LinkedList<BluetoothDevice> devices){
        this.isScanning = false;
        this.devices = devices;
        foreach (var item in devices)
        {
            Log(item.DeviceName);
        }
    }


    public void StartScanning(){
        if(!_m_helper.isConnected()&&!isScanning&&!isConnecting){
            isScanning=_m_helper.ScanNearbyDevices();

        }

    }
        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="log">日志内容</param>
        public void Log(string log)
        {
            if(m_log_txt){
                  m_log_txt.text += "\n" + log;
            }
         
        }
        public void LogLine(string log){
            m_log_txt.text+=log;

        }
 
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">消息的内容</param>
        public void Send(string msg)
        {
            _m_helper.SendData(msg);
        }
 
 
        void OnDestroy()
        {
          //  Disconnect();
        }
 
 
        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
          //  Log("断开连接");
            if (_m_helper != null)
               
             //  _m_helper.Disconnect();
               

                 m_log_txt.text = "使用前请配对BerryMed蓝牙设备!!!";

              //  _m_helper=null;
        }
    }
}