using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using System;

public class XMLCtrl : MonoBehaviour
{
    public float timer = 1f;

    public Button quitBtn;
    public Button reStartBtn;

    // Start is called before the first frame update
    void Start()
    {
     //   SetIsEnd("false");
     //   ReSetData();
      //  quitBtn.onClick.AddListener(OnQuitBtn);
      //  reStartBtn.onClick.AddListener(OnReStartBtn);
       // GetData();
    
    }

    // Update is called once per frame
    void Update()
     {
    //     if(PortControl._instance.IsEnd)
    //     {
    //         SetIsEnd("true");
  
    //         SaveData();
    //     }
    //     else
    //     {
          
    //         if (timer <= 0)
    //         {
    //             timer = 1f;
    //             SaveHeartRate();
    //         }
    //         else
    //         {
    //             timer -= Time.deltaTime;
    //     //        print("dddddddddddddddddtimer" + timer);
             

                
    //         }
            
    //     }
      

    }
    public void SetIsEnd(string isEnd)
    {
        // string path = Application.dataPath + "/StreamingAssets/TeamGameConfig.xml";
        // if (File.Exists(path))
        // {
        //     XmlDocument xml = new XmlDocument();
        //     xml.Load(path);
        //     XmlNodeList root = xml.SelectSingleNode("Comment").ChildNodes;
        //     foreach (XmlElement xl1 in root)
        //     {
        //         if (xl1.Name == "IsEnd")
        //         {
        //             xl1.SetAttribute("IsEnd", "");
        //             xl1.InnerText = isEnd;
        //         }

        //     }
        //     xml.Save(path);
        // }
    }
    public void SaveData()//保存数据
    {
        // string path = Application.dataPath + "/StreamingAssets/TeamGameConfig.xml";
        // if (File.Exists(path))
        // {
        //     XmlDocument xml = new XmlDocument();
        //     xml.Load(path);
        //     XmlNodeList root = xml.SelectSingleNode("Comment").ChildNodes;
        //     foreach (XmlElement xl1 in root)
        //     {
        //         if (xl1.Name == "Data")
        //         {
        //             xl1.SetAttribute("Data", "");
        //             xl1.InnerText = PortControl._instance.starttime + "|" + PortControl._instance.averageHeartlist.ToString()+"|"  + PortControl._instance.mixHeartList.ToString() + "|" + PortControl._instance.maxHeartList.ToString()+"|"+ "场景训练-单项训练"+"-鹰击长空" + "|"+PortControl._instance.GetTime((int)(PortControl._instance.timerlength));
        //         }
          
        //     }
        //     SetIsEnd("true");
        //     xml.Save(path);
        // }

    }
    public void SaveHeartRate()//保存数据
    {
        // string path = Application.dataPath + "/StreamingAssets/TeamGameConfig.xml";
        // if (File.Exists(path))
        // {
        //     XmlDocument xml = new XmlDocument();
        //     xml.Load(path);
        //     XmlNodeList root = xml.SelectSingleNode("Comment").ChildNodes;
        //     foreach (XmlElement xl1 in root)
        //     {
        //         if (xl1.Name == "HeartRate")
        //         {
        //             xl1.SetAttribute("HeartRate", "");
        //             xl1.InnerText = PortControl._instance.heartRate.ToString();
        //         }

        //     }
           
        //     xml.Save(path);
        // }

    }
    public void ReSetData()//
    {
        // string path = Application.dataPath + "/StreamingAssets/TeamGameConfig.xml";
        // if (File.Exists(path))
        // {
        //     XmlDocument xml = new XmlDocument();
        //     xml.Load(path);
        //     XmlNodeList root = xml.SelectSingleNode("Comment").ChildNodes;
        //     foreach (XmlElement xl1 in root)
        //     {
        //         if (xl1.Name == "Data")
        //         {
        //             xl1.SetAttribute("Data", "");
        //             xl1.InnerText = 0.ToString() + "|" + 0.ToString() + "|" + 0.ToString() + "|" + 0.ToString() + "|" + 0.ToString();
        //         }
        //         else if (xl1.Name == "HeartRate")
        //         {
        //             xl1.SetAttribute("HeartRate", "");
        //             xl1.InnerText = 0.ToString();
        //         }

        //     }

        //     xml.Save(path);
        // }

    }
    public string GetData()//获取用户名
    {
        // string com = "";
        // string path = Application.dataPath + "/StreamingAssets/TeamGameConfig.xml";
        // if (File.Exists(path))
        // {
        //     XmlDocument xml = new XmlDocument();
        //     xml.Load(path);
        //     XmlNodeList root = xml.SelectSingleNode("Comment").ChildNodes;
        //     foreach (XmlElement xl1 in root)
        //     {
        //         if (xl1.Name == "COM")
        //         {
        //             com = xl1.InnerText;
        //         }
        //     }
        // }
        // return com;
        return "";
    }
    public void OnReStartBtn()
    {
       // Application.LoadLevel(Application.loadedLevel);
    }
    public void OnQuitBtn()
    {
        // SetIsEnd("true");
        // SaveData();
        // Application.Quit();

    }
}
