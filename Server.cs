//////By Author Zhuxiaoliang 2019.05.25
//////PC端服务器远程渲染流程

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class Server : MonoBehaviour {

    public Image img;
    private Socket socket;
    private EndPoint clientEnd;
    private IPEndPoint ipEnd;
    private string recvStr;
    private string sendStr;
    private byte[] recvData;
    private byte[] showData;

    private int recvLen;
    private Thread connectThread;
    private byte[] picBytes;
    private Texture2D tex2d;
    private float delaytime;


    public Server()
    {
        this.delaytime = 0.03f;
    }
    private void InitSocket()
    {
        this.ipEnd = new IPEndPoint(IPAddress.Any, 8008);
        this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        this.socket.Bind(this.ipEnd);
        IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);
        this.clientEnd = point;
        print("waiting for UDP dgram");
        this.connectThread = new Thread(new ThreadStart(this.SocketReceive));
        this.connectThread.Start();
    }

    private void OnApplicationQuit()
    {
        this.SocketQuit();
    }

    [DebuggerHidden]
    private IEnumerator Show(float dt)
    {
        return new c__Iterator0
        {
            _dt = dt,
            _this = this
        };

    }

    private void SocketQuit()
    {
        if (this.connectThread != null)
        {
            this.connectThread.Interrupt();
            this.connectThread.Abort();
        }
        if (this.socket != null)
        {
            this.socket.Close();
        }
        print("disconnect");
    }
    private void SocketReceive()
    {
        while (true)
        {
            this.recvData = new byte[131072];
            this.recvLen = this.socket.ReceiveFrom(this.recvData, ref this.clientEnd);
            object[] objArray1 = new object[] { "message from: ", this.clientEnd.ToString(), "----Len", this.recvLen +System.Text.UTF8Encoding.UTF8.GetString(recvData,0, recvLen) };
            print(string.Concat(objArray1));
            if (this.recvData.Length > 10)
            {
                this.showData = (byte[])this.recvData.Clone();
            }
        }
    }
    private void Start()
    {
        this.InitSocket();
        base.StartCoroutine(this.Show(this.delaytime));
    }
    [CompilerGenerated]

    private sealed class c__Iterator0: IEnumerator ,IDisposable, IEnumerator<object>
    {
        internal float _dt;
        internal Server _this;
        internal object _current;
        internal bool _disposing;
        internal int _PC;

    [DebuggerHidden]
    public void Dispose()
    {
        this._disposing = true;
        this._PC = -1;
    }

    public bool MoveNext()
    {
        uint num = (uint)this._PC;
        this._PC = -1;
        switch (num)
        {
            case 0:
                break;

            case 1:
                if (this._this.showData != null)
                    {
                    if (this._this.tex2d != null)
                        {
                        UnityEngine.Object.Destroy(this._this.tex2d);
                    }
                    this._this.tex2d = new Texture2D(480, 0x110);
                    this._this.tex2d.LoadImage(this._this.showData);
                    Sprite sprite = Sprite.Create(this._this.tex2d, new Rect(0f, 0f, (float)this._this.tex2d.width, (float)this._this.tex2d.height), new Vector2(0.5f, 0.5f));
                    this._this.img.sprite=sprite;
                }
                break;

            default:
                return false;
        }
        this._current = new WaitForSeconds(this._dt);
        if (!this._disposing)
            {
            this._PC = 1;
        }
        return true;
    }

    [DebuggerHidden]
    public void Reset()
    {
            throw new Exception();
    }

    object IEnumerator<object>.Current
    {
        [DebuggerHidden]
        get
        {
            return this._current;
        }
    }

    object IEnumerator.Current
    {
        [DebuggerHidden]
        get
        {
            return this._current;
        }
    }
}

}


