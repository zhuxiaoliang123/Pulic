using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;

public class SqlAccess
{
    //public static MySqlConnection mySqlConnection;
   // public static SqliteConnection sqliteConnection;
    private string dbPath;
    public static SqliteConnection con;
    private SqliteCommand com;
    private Text UI;
    //数据库名称
   // public static string database = "myvr";

    //数据库IP
   // private static string host = "localhost";

    //用户名
   // private static string username = "root";

    //用户密码
    //private static string password = "root";

    //public static string sql = string.Format("database={0};server={1};user={2};password={3};port={4}",
    //    database, host, username, password, "3306");

    //public static MySqlConnection con;
    

    //private MySqlCommand com;
    
    void Awake()
    {
         
      
     
    }
    /// <summary>
    /// 构造方法开启数据库
    /// </summary>
    public SqlAccess()
    {
        CreateDataBase();
       // con = new MySqlConnection(sql);
        con = new SqliteConnection("URI=file:" + dbPath);
       // OpenMySQL(con);
        OpenSqlite(con);
    }
    

    /// <summary>
    /// 启动数据库
    /// </summary>    
    /// <param name="con"></param>
   
    public void OpenSqlite(SqliteConnection con)
    {
        con.Open();
       
//        Debug.Log("数据库已连接");
    }

    void CreateDataBase()
    {
#if UNITY_EDITOR
        dbPath = Application.dataPath + "/StreamingAssets/" + "userinfo.db";
#elif UNITY_STANDALONE_WIN
        dbPath = Application.dataPath + "/StreamingAssets/" + "userinfo.db";
#elif UNITY_ANDROID
        dbPath = Application.persistentDataPath +"/"+ "userinfo.db";
        if (!File.Exists(dbPath))
        {
          WWW www = new WWW("jar:file://" +Application.dataPath + "!/assets/"+ "userinfo.db");
          while(!www.isDone)
         {
             //UI = GameObject.FindGameObjectWithTag("UI").GetComponent<Text>();
             //UI.text = "没完成读取";
         }
         //UI = GameObject.FindGameObjectWithTag("UI").GetComponent<Text>();
         //UI.text = "完成读取";
         File.WriteAllBytes(dbPath, www.bytes);    
        
        
             
        }
     
       
#elif UNITY_IPHONE
        dbPath = Application.persistentDataPath + "/TestDb.db";
        if (!File.Exists(dbPath))
        {
            //拷贝数据库
            StartCoroutine(CopyDataBase());
        }
#endif

    }


    /// <summary>
    /// 创建表
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="con"></param>
    public void CreateTable(string sql)
    {
       // MySqlCommand com = new MySqlCommand(sql, con);
        SqliteCommand com = new SqliteCommand(sql, con);
        int res = com.ExecuteNonQuery();
    }

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="con"></param>
    public void InsertInfo(string sql)
    {
       // MySqlCommand com = new MySqlCommand(sql, con);
       /// int res = com.ExecuteNonQuery();
        SqliteCommand com = new SqliteCommand(sql, con);
        int res = com.ExecuteNonQuery();
    }
    
    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="con"></param>
    public void DeleteInfo(string sql)
    {
       // MySqlCommand com = new MySqlCommand(sql, con);
        //int res = com.ExecuteNonQuery();
        SqliteCommand com = new SqliteCommand(sql, con);
        int res = com.ExecuteNonQuery();
    }

    /// <summary>
    /// 修改数据
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="con"></param>
    public void UpdateInfo(string sql)
    {
       // MySqlCommand com = new MySqlCommand(sql, con);
        //int res = com.ExecuteNonQuery();
        SqliteCommand com = new SqliteCommand(sql, con);
        int res = com.ExecuteNonQuery();
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="con"></param>
    public object QuerySql(string sql ){

        SqliteCommand com = new SqliteCommand(sql, con);
        return com.ExecuteScalar();
      //  return com.ExecuteReader();
       // com.ExecuteScalar();


    }
    public Dictionary<int, List<string>> QueryInfo(string sql)
    {
        int indexDic = 0;
        int indexList = 0;
        Dictionary<int, List<string>> dic = new Dictionary<int, List<string>>();
        
        // MySqlCommand com = new MySqlCommand(sql, con);
        SqliteCommand com = new SqliteCommand(sql, con);
     
        SqliteDataReader reader= com.ExecuteReader();
      
        // MySqlDataReader reader = com.ExecuteReader();
        while (true)
        {
            if (reader.Read())
            {
             
                List<string> list = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                   // list.Add(reader[indexList].ToString());

                   list.Add(reader.IsDBNull(i)? "0":reader[indexList].ToString());
                    indexList++;
                   
                }

                dic.Add(indexDic, list);
                indexDic++;
                indexList = 0;
            }
            else
            {
                break;
            }
        }

        return dic;
    }

    /// <summary>
    /// 关闭数据库
    /// </summary>
    public void CloseMySQL()
    {
       // (new MySqlConnection(sql)).Close();
        (new SqliteConnection("URI=file:" + dbPath)).Close();
       // Debug.Log("关闭Sqlite数据库");
    }
}

