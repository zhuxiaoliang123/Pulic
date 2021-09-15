/*
 * 一个基于UGUI的分页功能的实现
 * 作者：朱小良
 * 时间：2020年11月11日
 * 博客：http://zhuxiaoliang.com
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;
using ToucanSystems;
using UnityEngine.Networking;
using System.IO;
using NAudio.Wave;
using AwesomeCharts;

public class OperationMgr : MonoBehaviour
{
    /// <summary>
    /// 当前页面索引
    /// </summary>
    private int m_PageIndex = 1;

    /// <summary>
    /// 总页数
    /// </summary>
    private int m_PageCount = 0;

    /// <summary>
    /// 元素总个数
    /// </summary>
    private int m_ItemsCount = 0;
    public int badeCount=6;

    /// <summary>
    /// 元素列表
    /// </summary>
	
    [SerializeField]
    public List<OperItem> m_ItemsList;  //临时储存
    private List<OperItem> tempList;

    
	
	#region ///搜索业务
    public InputField pnameInput;
    public InputField nameInput;
    public Dropdown sexInput;
    public InputField trainInput;
    #endregion


    /// <summary>
    /// 显示当前页数的标签
    /// </summary>
    public  Text m_PanelText;
	
	///增加元素
    public void OnSearchButtonClick(){

               bool add= GobalValue.ins.QueryPower("数据中心","查询");
        if(!add){
            WrongMsgInfo.ins.ShowMsg("没有查询权限");
            return;

        }

        if(string.IsNullOrEmpty(pnameInput.text)){
            WrongMsgInfo.ins.ShowMsg("请输出查询条件");
            return;
           
        }
        m_ItemsList =tempList.Where((p)=>p.pname.Contains(pnameInput.text)).ToList();
        if(m_ItemsList.Count>0){
              m_PageIndex=1;
        UpdateItem();
        }
        else{
            WrongMsgInfo.ins.ShowMsg("提示：没有查询到数据结果！");
            return;
        }
    
         
        // if(string.IsNullOrEmpty(pnameInput.text)&string.IsNullOrEmpty(nameInput.text)&string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text=="性别"){
        //     WrongMsgInfo.ins.ShowMsg("请输出查询条件");
        //     return;
           
        // }
   
    //     if(!string.IsNullOrEmpty(pnameInput.text)&string.IsNullOrEmpty(nameInput.text)&string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text=="性别"){
    //           m_ItemsList= tempList.Where((p)=> p.pname==pnameInput.text).ToList();
    //          m_PageIndex=1;
    //     UpdateItem();
    //     print("刷新了记录数据");
    //     return;

    //     }
    //     if(string.IsNullOrEmpty(pnameInput.text)&!string.IsNullOrEmpty(nameInput.text)&string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text=="性别"){
    //           m_ItemsList= tempList.Where((p)=> p.uname==nameInput.text).ToList();
    //          m_PageIndex=1;
    //     UpdateItem();
    //     print("刷新了记录数据");
    //     return;

    //     }
    //        if(string.IsNullOrEmpty(pnameInput.text)&string.IsNullOrEmpty(nameInput.text)&string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text!="性别"){
    //           int gender =sexInput.captionText.text=="男"?1:0;
    //           m_ItemsList= tempList.Where((p)=> p.sex==gender).ToList();
    //          m_PageIndex=1;
    //     UpdateItem();
    //     print("刷新了记录数据");
    //     return;

    //     }
    //       if(string.IsNullOrEmpty(pnameInput.text)&!string.IsNullOrEmpty(nameInput.text)&!string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text=="性别"){
    //           m_ItemsList= tempList.Where((p)=> p.ptimer==trainInput.text).ToList();
    //          m_PageIndex=1;
    //     UpdateItem();
    //     print("刷新了记录数据");
    //     return;

    //     }

    //    if(string.IsNullOrEmpty(pnameInput.text)&string.IsNullOrEmpty(nameInput.text)&!string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text=="性别"){
    //          //m_ItemsList
    //         var temp = tempList.Where((p)=> p.ptimer.Contains(trainInput.text)).ToList();
    //           if(temp.Count<=0){
    //               WrongMsgInfo.ins.ShowMsg("输入时间有误");
    //               return;

    //           }
    //           m_ItemsList=temp;
    //          m_PageIndex=1;
    //     UpdateItem();
    //     print("刷新了记录数据");
    //     return;

    //     }
    //       if(!string.IsNullOrEmpty(pnameInput.text)&!string.IsNullOrEmpty(nameInput.text)&string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text=="性别"){
    //           m_ItemsList= tempList.Where((p)=> p.pname==pnameInput.text&p.uname==nameInput.text).ToList();
    //          m_PageIndex=1;
    //     UpdateItem();
    //     print("刷新了记录数据");
    //     return;

    //     }
    //     if(!string.IsNullOrEmpty(pnameInput.text)&string.IsNullOrEmpty(nameInput.text)&!string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text=="性别"){
    //           m_ItemsList= tempList.Where((p)=> p.pname==pnameInput.text&p.ptimer==trainInput.text).ToList();
    //          m_PageIndex=1;
    //     UpdateItem();
    //     print("刷新了记录数据");
    //     return;

    //     }
    //     if(!string.IsNullOrEmpty(pnameInput.text)&string.IsNullOrEmpty(nameInput.text)&string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text!="性别"){
    //          int gender =sexInput.captionText.text=="男"?1:0;
    //           m_ItemsList= tempList.Where((p)=> p.pname==pnameInput.text&p.sex==gender).ToList();
    //          m_PageIndex=1;
    //     UpdateItem();
    //     print("刷新了记录数据");
    //     return;

    //     }
    //     if(string.IsNullOrEmpty(pnameInput.text)&!string.IsNullOrEmpty(nameInput.text)&!string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text=="性别"){
    //  //        int gender =sexInput.captionText.text=="男"?1:0;
    //           m_ItemsList= tempList.Where((p)=> p.uname==nameInput.text&p.ptimer==trainInput.text).ToList();
    //          m_PageIndex=1;
    //     UpdateItem();
    //     print("刷新了记录数据");
    //     return;

    //     }
    //     if(string.IsNullOrEmpty(pnameInput.text)&!string.IsNullOrEmpty(nameInput.text)&string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text!="性别"){
    //          int gender =sexInput.captionText.text=="男"?1:0;
    //           m_ItemsList= tempList.Where((p)=> p.uname==nameInput.text&p.sex==gender).ToList();
    //          m_PageIndex=1;
    //     UpdateItem();
    //     print("刷新了记录数据");
    //     return;

    //     }
    //       if(string.IsNullOrEmpty(pnameInput.text)&string.IsNullOrEmpty(nameInput.text)&!string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text!="性别"){
    //          int gender =sexInput.captionText.text=="男"?1:0;
    //           m_ItemsList= tempList.Where((p)=> p.ptimer==trainInput.text&p.sex==gender).ToList();
    //          m_PageIndex=1;
    //     UpdateItem();
    //     print("刷新了记录数据");
    //     return;

    //     }
    //     if(!string.IsNullOrEmpty(pnameInput.text)&!string.IsNullOrEmpty(nameInput.text)&string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text!="性别"){
    //          int gender =sexInput.captionText.text=="男"?1:0;
    //           m_ItemsList= tempList.Where((p)=> p.uname==nameInput.text&p.sex==gender&p.pname==pnameInput.text).ToList();
    //          m_PageIndex=1;
    //     UpdateItem();
    //     print("刷新了记录数据");
    //     return;

    //     }
    //       if(!string.IsNullOrEmpty(pnameInput.text)&!string.IsNullOrEmpty(nameInput.text)&!string.IsNullOrEmpty(trainInput.text)&sexInput.captionText.text!="性别"){
    //          int gender =sexInput.captionText.text=="男"?1:0;
    //           m_ItemsList= tempList.Where((p)=> p.uname==nameInput.text&p.sex==gender&p.pname==pnameInput.text&p.ptimer==trainInput.text).ToList();
    //          m_PageIndex=1;
    //     UpdateItem();
    //     print("刷新了记录数据");
    //     return;

    //     }
    
    
    
    
     
     
        //      m_PageIndex=1;
        // UpdateItem();
        // print("刷新了列表记录数据");


    }
    

    public Transform detailPanel;
    
    

    private void PlayInfoAnim(string str){
   
    }


    void Awake() 
    {
       m_ItemsList =new List<OperItem>();
       deleteItem=new List<OperItem>();
       tempList=new List<OperItem>();
       guidAudio=detailPanel.GetComponent<AudioSource>();
       allToggle.onValueChanged.AddListener(ListenAllToggle);
       //OnEnableData();
    
    }
    

    private SQLiteHelper sql ;
    // private void OnDisable()
    // {
    //     guidAudio.Stop();
    // }
  
    public void OnEnable()
    {
        trainInput.text=string.Empty;
        pnameInput.text=string.Empty;
        nameInput.text=string.Empty;
        sexInput.captionText.text="性别";

        m_ItemsList.Clear();
        tempList.Clear();
        transform.parent.parent.gameObject.SetActive(true);
        detailPanel.gameObject.SetActive(false);
  
       sql = new SQLiteHelper();
       string str =("select * from t_Test");
       var dic= sql.QueryInfo(str);
       string  userName= GobalValue.uname;

        for (int i = 0; i < dic.Count; i++)
        {
            
                 OperItem  item = new OperItem();
                 item.num=i+1;
                 item.id=int.Parse(dic[i][0]); //dic[i][0];
                 item.uname=dic[i][1];
                 item.fulname=dic[i][2];
                 item.sex=int.Parse(dic[i][3]);
                 item.pid=int.Parse(dic[i][4]);
                 item.pname=dic[i][5];
                 item.ptimer=dic[i][6];
                 item.pcompete=dic[i][7];
                 item.ptrytimes=int.Parse(dic[i][8]);
                 item.ptimelen=int.Parse(dic[i][9]);
                 item.pinterven=int.Parse(dic[i][10]);
                 item.psound=dic[i][11];
                 item.data1=dic[i][12];
                 item.data2=dic[i][13];
                 item.etimer=dic[i][17];
                // item.capture=dic[i][14];
               // tempList.Add(item);
                 
                 
                 m_ItemsList.Add(item);
             
 
           

        }
      
        m_ItemsList= m_ItemsList.OrderByDescending((p)=>p.id).ToList();
       //  public List<OperItem> m_ItemsList;

       var List =new List<OperItem>(); 
       for (int i = 0; i < m_ItemsList.Count; i++)
       {
           var op=m_ItemsList[i];
           op.num=i;
           List.Add(op);

       }
       m_ItemsList=List;
         tempList=m_ItemsList;    
        
        InitItems();
        sql.CloseConnection();
       
    }
    public void OnQueryButtonClick(){
     
        ////::TODO

    }


    /// <summary>
    /// 初始化GUI
    /// </summary>
 

    /// <summary>
    /// 初始化元素
    /// </summary>
    private void InitItems()
    {
     
       UpdateItem();
    }
	///更新界面显示
	private void UpdateItem(){
		   //计算元素总个数
            m_ItemsCount = m_ItemsList.Count;
          
        //计算总页数
        m_PageCount = (m_ItemsCount % badeCount) == 0 ? m_ItemsCount / badeCount : (m_ItemsCount / badeCount) + 1;
         
        BindPage(m_PageIndex);
        //更新界面页数
        m_PanelText.text = string.Format("{0}/{1}", m_PageIndex.ToString(), m_PageCount.ToString());
	}
 
	public void DeleteItem(){

         if(deleteItem.Count<=0){
           WrongMsgInfo.ins.ShowMsg("请选择要删除的数据");
           return;

         }   
         sql =new SQLiteHelper();    
         for (int i = 0; i < deleteItem.Count; i++)
         {
               string str =string.Format("delete from t_Test where id='{0}'",deleteItem[i].id);
               sql.ExecuteNonQuery(str);

             if(m_ItemsList.Contains(deleteItem[i])){
                   m_ItemsList.Remove(deleteItem[i]);
             }
             if(tempList.Contains(deleteItem[i])){
                 tempList.Remove(deleteItem[i]);

             }
           

            
         }
         sql.CloseConnection();
         
         ////////////////////再这里处理数据库的删除逻辑
        deleteItem.Clear();

         if(m_ItemsList.Count%6==0){
                
                if(m_PageCount>0){
                   m_PageIndex--;
                }
              
            }
		///更新界面
		 m_ItemsCount = m_ItemsList.Count;
          
        //计算总页数
        m_PageCount = (m_ItemsCount % badeCount) == 0 ? m_ItemsCount / badeCount : (m_ItemsCount / badeCount) + 1;
         
        BindPage(m_PageIndex);
        //更新界面页数
        m_PanelText.text = string.Format("{0}/{1}", m_PageIndex.ToString(), m_PageCount.ToString());
		
	

	}

    public void OnFristPage(){
        m_PageIndex=1;
        UpdateItem();
    }
    public void OnLastPage(){
        
            m_PageIndex = (m_ItemsCount % badeCount) == 0 ? m_ItemsCount / badeCount: (m_ItemsCount / badeCount) + 1;
        
        UpdateItem();   

    }
    /// <summary>
    /// 下一页
    /// </summary>
    public void Next()
    {
        if(m_PageCount <= 0)
            return;
        //最后一页禁止向后翻页
        if(m_PageIndex >= m_PageCount)
            return;

        m_PageIndex += 1;
        if (m_PageIndex >= m_PageCount)
            m_PageIndex = m_PageCount;

        BindPage(m_PageIndex);

        //更新界面页数
        m_PanelText.text = string.Format("{0}/{1}", m_PageIndex.ToString(), m_PageCount.ToString());
    }

    /// <summary>
    /// 上一页
    /// </summary>
    public void Previous()
    {
        if(m_PageCount <= 0)
            return;
        //第一页时禁止向前翻页
        if(m_PageIndex <= 1)
            return;
        m_PageIndex -= 1;
        if(m_PageIndex < 1)
            m_PageIndex = 1;

        BindPage(m_PageIndex);

        //更新界面页数
        m_PanelText.text = string.Format("{0}/{1}", m_PageIndex.ToString(), m_PageCount.ToString());
    }

    /// <summary>
    /// 绑定指定索引处的页面元素
    /// </summary>
    /// <param name="index">页面索引</param>
    private void BindPage(int index)
    {
        //列表处理
        if(m_ItemsList == null || m_ItemsCount <= 0){
            
            deleteItem.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
			return;
		}
            

        //索引处理
        if(index < 0 || index > m_ItemsCount)
            return;

        //按照元素个数可以分为1页和1页以上两种情况
        if(m_PageCount == 1)
        {
       //     print("-------------------");
            int canDisplay = 0;
            for(int i = badeCount; i > 0; i--)
            {
                if(canDisplay < m_ItemsList.Count){
                    BindGridItem(transform.GetChild(canDisplay), m_ItemsList[badeCount - i]);
                    transform.GetChild(canDisplay).gameObject.SetActive(true);
                }else{
                    //对超过canDispaly的物体实施隐藏
                    transform.GetChild(canDisplay).gameObject.SetActive(false);
                }
                canDisplay += 1;
            }
        }else if(m_PageCount > 1){
            //1页以上需要特别处理的是最后1页
            //和1页时的情况类似判断最后一页剩下的元素数目
            //第1页时显然剩下的为12所以不用处理
            if(index == m_PageCount){
                int canDisplay = 0;
                for(int i = badeCount; i > 0; i--)
                {
                    //最后一页剩下的元素数目为 m_ItemsCount - 12 * (index-1)
                    if(canDisplay < m_ItemsCount - badeCount * (index-1)){
                        BindGridItem(transform.GetChild(canDisplay), m_ItemsList[badeCount * index-i]);
                        transform.GetChild(canDisplay).gameObject.SetActive(true);
                    }else{
                        //对超过canDispaly的物体实施隐藏
                        transform.GetChild(canDisplay).gameObject.SetActive(false);
                    }
                    canDisplay += 1;
                }
            }
            else{
                for(int i = badeCount; i > 0; i--)
                {   
                    BindGridItem(transform.GetChild(badeCount - i), m_ItemsList[badeCount * index - i]);
                    transform.GetChild(badeCount - i).gameObject.SetActive(true);
                }
            }
        }
        deleteItem.Clear();
           
    }



    /// <summary>
    /// 将一个GridItem实例绑定到指定的Transform上
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="gridItem"></param>
  
    public List<OperItem> deleteItem;
    private OperItem currentItem;
  
    
  
   
    private void BindGridItem(Transform trans,OperItem gridItem)
    {
        trans.Find("id").GetComponent<Text>().text = gridItem.num.ToString();
        trans.Find("train").GetComponent<Text>().text=gridItem.pname;
        trans.Find("begin").GetComponent<Text>().text = gridItem.ptimer;
        trans.Find("end").GetComponent<Text>().text=gridItem.etimer;
        trans.Find("cost").GetComponent<Text>().text=gridItem.ptimelen.ToString();
        trans.Find("percent").GetComponent<Text>().text=gridItem.pcompete;

        Button button =trans.Find("Button").GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(()=>
        {
          bool add= GobalValue.ins.QueryPower("数据中心","查看");
        if(!add){
            WrongMsgInfo.ins.ShowMsg("没有查看权限");
            return;

        }
            currentItem=gridItem;
          
            OnButtonListen(gridItem);
          
            UpdateDetailPanel(detailPanel,currentItem);
            detailPanel.GetComponent<tset>().operItem=gridItem;
            detailPanel.gameObject.SetActive(true);
       

        });
        Toggle toggle =trans.Find("Toggle").GetComponent<Toggle>();
        if(allToggle.isOn){
            toggle.isOn=true;

        }
        else{
        toggle.isOn=false;
        }
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener((ison)=>{
             if(ison){
                 if(!deleteItem.Contains(gridItem)){
                     deleteItem.Add(gridItem);

                 }

             }
             else{
                 if(deleteItem.Contains(gridItem)){
                     deleteItem.Remove(gridItem);

                 }
             }

        });
        

      
        
    }
    public Toggle allToggle;
    private void ListenAllToggle(bool ison){
        if(ison){
         //   deleteItem=m_ItemsList;
            UpdateItem();

        }else{
           // deleteItem=new List<OperItem>();
           print(m_ItemsList.Count+"个数");
            UpdateItem();
            print(m_ItemsList.Count+"个数2");
        }
    }
    private string microPath;
    private AudioSource guidAudio;
    public void OnMicroMusicPlay(){
        if(string.IsNullOrEmpty(microPath)){
            WrongMsgInfo.ins.ShowMsg("录音失败");
            return;
        }
      //detailPanel.transform. StartCoroutine(GetClipData("Application.streamingAssetsPath+/RecordMicro/"+microPath));
        detailPanel.GetComponent<Show>().StartCoroutine(GetClipData(Application.streamingAssetsPath+"/RecordMicro/"+microPath));
        
          
    }
    IEnumerator GetClipData(string path){
        using (UnityWebRequest www =UnityWebRequestMultimedia.GetAudioClip(path,AudioType.WAV))
        {
         //   print(path+"***");
              yield return www.SendWebRequest();
              if(www.isNetworkError||www.isHttpError){
                //   window.ShowWindow("数据操作提示", "上传失败", MessageBoxButtons.Cancel, null);
                  Debug.Log("没有找到！");
                 Debug.Log(www.error);
              }
              else{
              //     window.ShowWindow("数据操作提示", "上传成功", MessageBoxButtons.Cancel, null);
                  Debug.Log("播放成功！");
                  guidAudio.clip=DownloadHandlerAudioClip.GetContent(www);
                  guidAudio.Play();
                
              }
        }

    }
    private string SelectImage(int idd){
           sql =new SQLiteHelper();    
        
               string str =string.Format("select capture from t_Test where id='{0}'",idd);
              string imageStr = sql.QuerySingle(str);
              sql.CloseConnection();
               return imageStr;
    }
    private void UpdateDetailPanel(Transform tf ,OperItem op){
        tf.Find("Data/Image").gameObject.SetActive(false);
          if(op.pid==0){
              tf.Find("Data").gameObject.SetActive(false);
          }
          else{
              tf.Find("Data").gameObject.SetActive(true);
          }
          if(op.pid==3){   //演讲
             
              microPath=op.psound;
//                print(microPath+"yy");
              if(!string.IsNullOrEmpty(microPath)){
                
                 
                   if(File.Exists(Application.streamingAssetsPath+"/RecordMicro/"+op.psound)){
                         tf.Find("ContentLeft/Data/psound").gameObject.SetActive(true);
                       var stream =File.Open(Application.streamingAssetsPath+"/RecordMicro/"+op.psound,FileMode.Open);
                       var reader =new WaveFileReader(stream);
              
                       double M =reader.TotalTime.TotalSeconds/60;
                       double s =reader.TotalTime.TotalSeconds%60;
                       stream.Close();
                       stream.Dispose();
                       string len =string.Format("{0:00}",M)+":"+string.Format("{0:00}",s);
                       tf.Find("ContentLeft/Data/psound/Text").GetComponent<Text>().text=len;

                   }
                   else{
                       tf.Find("ContentLeft/Data/psound").gameObject.SetActive(false);
                   }
                     

              }
              else{
                        tf.Find("ContentLeft/Data/psound").gameObject.SetActive(false);
              }
          }
          else{
              tf.Find("ContentLeft/Data/psound").gameObject.SetActive(false);
          }
          if(op.pid==2){    //社交
                 tf.Find("Data/Image").gameObject.SetActive(true);
                  tf.Find("Data/Image/Pic").gameObject.SetActive(false);
                 string res = SelectImage(op.id);
              
                 if(!string.IsNullOrEmpty(res)){
                     byte[] cap =Convert.FromBase64String(res);
                    Texture2D texture = new Texture2D(Screen.width/2, Screen.height/2);
                    texture.LoadImage(cap);//流数据转换成Texture2D
                    tf.Find("Data/Image/Pic").GetComponent<RawImage>().texture=texture;// sp;


                 }
                 else{
                     tf.Find("Data/Image").gameObject.SetActive(false);
                 }
                 


          }
          tf.Find("Data/LineChart").GetComponent<LineChart>().oper=currentItem;
          tf.Find("ContentLeft/Data/uname").GetComponent<Text>().text=op.uname;
          tf.Find("ContentLeft/Data/fulname").GetComponent<Text>().text=op.fulname;
           string sex ="男";
           if(op.sex==1){
               sex="男";

           }else{
               sex="女";
           }
           tf.Find("ContentLeft/Data/sex").GetComponent<Text>().text=sex;
           string bir =GobalValue.ins.QuerySingleBirth(op.uname);
            tf.Find("ContentLeft/Data/brith").GetComponent<Text>().text=bir;
            string age =GobalValue.ins.QuerySingleAge(op.uname);
              tf.Find("ContentLeft/Data/age").GetComponent<Text>().text=age;
                tf.Find("ContentLeft/Data/ptimer").GetComponent<Text>().text=op.ptimer;
                  tf.Find("ContentLeft/Data/pname").GetComponent<Text>().text=op.pname;
                    tf.Find("ContentLeft/Data/pcompete").GetComponent<Text>().text=op.pcompete;
                      tf.Find("ContentLeft/Data/ptrytimes").GetComponent<Text>().text=op.ptrytimes.ToString();

                        int H = (int)(op.ptimelen / 60);
                         float S = op.ptimelen % 60;
                     
                        tf.Find("ContentLeft/Data/ptimelen").GetComponent<Text>().text= string.Format("{0:00}",H) +":" + string.Format("{0:00}", S);

                          tf.Find("ContentLeft/Data/pinterven").GetComponent<Text>().text=op.pinterven.ToString();
                          
 



    }
   
   public void OnCloseDetailPanel(){
        guidAudio.Stop();
     //  detailPanel.gameObject.SetActive(false);
     //  transform.parent.parent.gameObject.SetActive(true);
   }
    private void OnButtonListen(OperItem itme){

    }
    /// <summary>
/// 阿拉伯数字转换成中文数字
/// </summary>
/// <param name="x"></param>
/// <returns></returns>
public string NumToChinese(string x)
{
    string[] pArrayNum = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
    //为数字位数建立一个位数组
    string[] pArrayDigit = { "", "十", "百", "千" };
    //为数字单位建立一个单位数组
    string[] pArrayUnits = { "", "万", "亿", "万亿" };
    var pStrReturnValue = ""; //返回值
    var finger = 0; //字符位置指针
    var pIntM = x.Length % 4; //取模
    int pIntK;
    if (pIntM > 0)
        pIntK = x.Length / 4 + 1;
    else
        pIntK = x.Length / 4;
    //外层循环,四位一组,每组最后加上单位: ",万亿,",",亿,",",万,"
    for (var i = pIntK; i > 0; i--)
    {
        var pIntL = 4;
        if (i == pIntK && pIntM != 0)
            pIntL = pIntM;
        //得到一组四位数
        var four = x.Substring(finger, pIntL);
        var P_int_l = four.Length;
        //内层循环在该组中的每一位数上循环
        for (int j = 0; j < P_int_l; j++)
        {
            //处理组中的每一位数加上所在的位
            int n = Convert.ToInt32(four.Substring(j, 1));
            if (n == 0)
            {
                if (j < P_int_l - 1 && Convert.ToInt32(four.Substring(j + 1, 1)) > 0 && !pStrReturnValue.EndsWith(pArrayNum[n]))
                    pStrReturnValue += pArrayNum[n];
            }
            else
            {
                if (!(n == 1 && (pStrReturnValue.EndsWith(pArrayNum[0]) | pStrReturnValue.Length == 0) && j == P_int_l - 2))
                    pStrReturnValue += pArrayNum[n];
                pStrReturnValue += pArrayDigit[P_int_l - j - 1];
            }
        }
        finger += pIntL;
        //每组最后加上一个单位:",万,",",亿," 等
        if (i < pIntK) //如果不是最高位的一组
        {
            if (Convert.ToInt32(four) != 0)
                //如果所有4位不全是0则加上单位",万,",",亿,"等
                pStrReturnValue += pArrayUnits[i - 1];
        }
        else
        {
            //处理最高位的一组,最后必须加上单位
            pStrReturnValue += pArrayUnits[i - 1];
        }
    }
    return pStrReturnValue;
}

   

}
///我的测试记录
public struct OperItem{
     public int num;
     public int id;    
     public string uname;
     public string fulname;
     public int sex;
    //  public string  birth;
    //  public int age;
     public int pid;   ///类型 减压放松 1 ， 人际关系2  , 社交恐惧3   ； 普通 0
     public string pname;
      public string ptimer;
      public string etimer;

      public string pcompete;
      
      public int ptrytimes;
      public int ptimelen;    //训练用时
      public int pinterven;   //干预记录
      public string psound;
      public string data1; //脉搏的二维数组
      public string data2;  //血氧的二维数组
      public string data3;   // 脉搏血氧的最大，最小，平均值
     // public string capture;
      

   

}
