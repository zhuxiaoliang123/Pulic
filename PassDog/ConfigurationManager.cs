using System;
using UnityEngine;
using System.IO;


public class ConfigurationManager
{

    public ConfigurationManager()
    {
     

         

        if (!File.Exists(path))
        {
            Debug.Log("不存在" + path + "此文件");
        }
        else
        {
         path = Environment.CurrentDirectory + "\\App.config";
       //  path = Application.streamingAssetsPath + "/Config" + "\\App.config";
          
      
        }

    }
    private static string path = Environment.CurrentDirectory + "\\App.config";
    // private static string path = Application.streamingAssetsPath+"/Config"+ "\\App.config";

    public static AppSettings appSettings
    {
        get
        {
          
            if (!string.IsNullOrEmpty(path))
            {
               return new AppSettings(path);
            }
            return null;
        }
    }

}
