using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Client : MonoBehaviour {

    //以下默认都是私有的成员  
    Socket socket; //目标socket  
    IPEndPoint ipEnd; //端口  

    string sendStr; //发送的字符串  
    byte[] recvData = new byte[1024]; //接收的数据，必须为字节  
    byte[] sendData = new byte[1024]; //发送的数据，必须为字节  
    byte[] sendTexData;
    int recvLen; //接收的数据长度  

    public void Init(string ip,int port) {
        //定义连接的服务器ip和端口，可以是本机ip，局域网，互联网  
        ipEnd = new IPEndPoint(IPAddress.Parse(ip), port);
        //定义套接字类型,在主线程中定义  
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //建立初始连接 
        SocketSend("hello");
    }

    /// <summary>
    /// 发送数据给指定服务端  
    /// </summary>
    /// <param name="sendStr">string字符串</param>
    public void SocketSend(string sendStr)
    {
        //清空发送缓存  
        sendData = new byte[1024];
        //数据类型转换  
        sendData = Encoding.ASCII.GetBytes(sendStr);
        //发送给指定服务端  
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
    }

    /// <summary>
    /// 发送数据给指定服务端
    /// </summary>
    /// <param name="sendData">byte数组</param>
    public void SocketSend(byte[] sendData)
    {
        //发送给指定服务端  
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
    }

}
