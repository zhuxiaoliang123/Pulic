using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulDisplayRenderManager : Singleton<MulDisplayRenderManager> {
    [Tooltip("是否开启屏幕投影")]
    public bool IsActice = true;
    [Tooltip("投屏设备的IP地址")]
    public string IP = "192.168.1.120";
    [Tooltip("投屏设备的端口号")]
    public int Port = 8008;
    [Tooltip("EyeID")]
    public int EyeID = 2;
    [Tooltip("渲染Texture的大小")]
    public Vector2 TextureSize = new Vector2(640,360);
    [Tooltip("发送的时间间隔[0.03,0.1]")]
    private float Send_FrameTime = 0.05f;
    [Tooltip("设置应用的帧率")]
    private int TargetFrameRate = 30;

    private Client client;

    private Texture2D tex2D;

    private bool isSend = true;

    private bool isCreateT2D = true;

    private bool isRenderToT2D = true;

    private RenderTexture renderTexture;


    protected override void Awake()
    {
        base.Awake();
        if (!IsActice) gameObject.SetActive(false);
    }

    private void Update()
    {
        //Graphics.Blit()
        //RenderTexture rt = new RenderTexture(GameObject.FindObjectOfType<Pvr_UnitySDKEye>().eyecamera.targetTexture);
    }

    void Start() {
//#if UNITY_ANDROID
        Application.targetFrameRate = TargetFrameRate;//此处限定60帧
        //设置发射帧率区间
        Send_FrameTime = Send_FrameTime <= 0.03f ? 0.03f : Send_FrameTime >= 0.1f ? 0.1f : Send_FrameTime;
        client = transform.Find("Client").GetComponent<Client>();
        client.Init(IP, Port);
        StartCoroutine(SendTexToDevices(Send_FrameTime));

    }

    IEnumerator SendTexToDevices(float dt)
    {
       // renderTexture = new RenderTexture(896, 896, 4);
        renderTexture = new RenderTexture(640, 480, 4);
        renderTexture.antiAliasing = 8;
        renderTexture.wrapMode = TextureWrapMode.Clamp;
        renderTexture.filterMode = FilterMode.Trilinear;
        renderTexture.hideFlags = HideFlags.HideAndDontSave;
        renderTexture.Create();
        while (true)
        {
            yield return new WaitForSeconds(dt);
            if (IsActice ) //&& Application.isMobilePlatform
            {
                //Create Texture2D
                int width = (int)TextureSize.x; 
                int height = (int)TextureSize.y;

                if (isRenderToT2D)
                {
                    yield return new WaitForEndOfFrame();

                    FindObjectOfType<Pvr_UnitySDKEye>().eyecamera.targetTexture = renderTexture;
                    FindObjectOfType<Pvr_UnitySDKEye>().eyecamera.Render();

                    yield return null;
                    if (isCreateT2D) tex2D = new Texture2D((int)TextureSize.x, (int)TextureSize.y, TextureFormat.RGB24, false);
                    tex2D.hideFlags = HideFlags.HideAndDontSave;
                    tex2D.wrapMode = TextureWrapMode.Clamp;
                    tex2D.filterMode = FilterMode.Trilinear;
                    tex2D.hideFlags = HideFlags.HideAndDontSave;
                    tex2D.anisoLevel = 0;
                    float _x = (renderTexture.width - TextureSize.x) / 2f;
                    float _y = (renderTexture.height - TextureSize.y) / 2f;
                    RenderTexture.active = renderTexture;
                    tex2D.ReadPixels(new Rect(_x, _y, TextureSize.x, TextureSize.y), 0, 0);
                    tex2D.Apply();
                    RenderTexture.active = null;
                }

                yield return null;

                //Send Texture2D byte
                if (isSend) client.SocketSend(tex2D.EncodeToJPG());
                if (tex2D != null)
                {
                    Destroy(tex2D);
                }
                renderTexture.DiscardContents();
            }
        }
    }
//#endif

}
