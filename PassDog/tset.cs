using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPOI.XWPF.UserModel;
using System.IO;
using System;
using NPOI.OpenXmlFormats.Wordprocessing;
using UnityEngine.UI;
using LitJson;
using System.Text.RegularExpressions;
using SFB;

public class tset : MonoBehaviour
{
     private XWPFDocument doc=new XWPFDocument();
     string u0 = "123456";
     string u1 = "男";
     string u2 = "我能打开了吗？";
   string u3 = "好开心！";
     string u4 = "20";
     string u5 = "2020/06/23 12:00:00";
     string u6 = "未选择天气";
     string u7 = "2020/06/23 17:00:00";
     string u8 = "2020/06/23 18:00:00";
          string TemplatePath1 = Application.streamingAssetsPath+"/Template3.docx";//模板1路径
       // string TemplatePath1 = @"C:/Users/Administrator/Desktop/Template1.docx";//模板1路径
      //  string SavePath1 = @"C:/Users/Administrator/Desktop/3D心理电子沙盘系统用户报告.docx";//根据模板1生成的文件路径
         string SavePath1 = @"C:/Users/Administrator/Desktop/VR心理训练系统数据报告.docx";




// Regex rex = new Regex(@"^[\u4E00-\u9FA5A-Za-z0-9]+$");
// var result = rex.Match(text);
// if (!result.Success)
// {
//     Debug.Log("文本框内包含非法字符!");
//     return;
// }

 
   public OperItem operItem;
   public TrainItem trainItem;

   public InputField fileName;
   public InputField filePath;


    public void OnClickBtn() {
       
      
        if(string.IsNullOrEmpty(fileName.text)){
          WrongMsgInfo.ins.ShowMsg("文件名不能为空");
          return;

       }
         Regex rex = new Regex(@"^[\u4E00-\u9FA5A-Za-z0-9]+$");
      var result = rex.Match(fileName.text);
        if (!result.Success)
          {
          //Debug.Log("文本框内包含非法字符!");
          WrongMsgInfo.ins.ShowMsg("文件名包含非法字符！");
        return;
          }

        var path = StandaloneFileBrowser.SaveFilePanel("Title", "", fileName.text, "docx");
        if (!string.IsNullOrEmpty(path)) {
          //  File.WriteAllText(path, _data);
          
          filePath.text=path;
          
        }
    }
    
    private void Start()
    {
     
       
    

      
    }
    // Start is called before the first frame update
    public void Export()
    {

       if(string.IsNullOrEmpty(fileName.text)){
          WrongMsgInfo.ins.ShowMsg("文件名不能为空");
          return;

       }
       if(string.IsNullOrEmpty(filePath.text)){
          WrongMsgInfo.ins.ShowMsg("文件路径不能为空");
          return;

       }
       Regex rex = new Regex(@"^[\u4E00-\u9FA5A-Za-z0-9]+$");
      var result = rex.Match(fileName.text);
        if (!result.Success)
          {
          //Debug.Log("文本框内包含非法字符!");
          WrongMsgInfo.ins.ShowMsg("文件名包含非法字符！");
        return;
          }


             SavePath1=filePath.text;

           
      // eva1=detailedController.inputSurvey.text!=string.Empty?detailedController.inputSurvey.text:" ";
      // eva2=detailedController.inputEvaluate.text!=string.Empty?detailedController.inputEvaluate.text:" ";
try
{


   
        if(!string.IsNullOrEmpty(operItem.pname)){
             u0 =operItem.num.ToString();
        u1=operItem.uname;
        u2=operItem.sex==1?"男":"女";
        u3 =operItem.fulname;
        u4=operItem.pname;
        print("u4"+u4);
        u5=operItem.ptimer;
           u6=operItem.pinterven.ToString();
        u7=operItem.pcompete;
        u8=operItem.ptimelen.ToString();
        }
      
         SQLiteHelper sql =new SQLiteHelper();
         string str =string.Format("select pData3 from t_Test where id='{0}'",operItem.id);
         string data3 =sql.QuerySingle(str);
         print(data3);

      // var data3Valuess=  JsonMapper.ToObject<Vector2[]>(data3);
         sql.CloseConnection();
     var ParamTemplate1 = new
        {
            u0 = u0,
            u1 = u1,
            u2 = u2,
             u3 = u3,
             u4=u4,
             u5=u5,
             u6=u6,
             u7=u7,
             u8=u8,
            // eva1=eva1,
            // eva2=eva2
        };
        WordTemplateHelper.WriteToPublicationOfResult(TemplatePath1, SavePath1, WordTemplateHelper.getProperties(ParamTemplate1),data3);
         //.WriteToPublicationOfResult(TemplatePath1, SavePath1);
            System.Diagnostics.Process.Start(SavePath1);

}
catch (System.Exception)
{

      // UIPopWindowManager.ins.ShowWindow("WORD操作提示", "请关闭已打开的Word文档!", MessageBoxButtons.Cancel, null);
      WrongMsgInfo.ins.ShowMsg("提示：请关闭已打开的word文档！");

    throw;
}
       
          
        //CreateParagraph(ParagraphAlignment.CENTER,20,"black","这是我测试的用户报告");

          // CreateParagraph(ParagraphAlignment.CENTER,20,"white","2222222222222222222");
        // NewLine( ParagraphAlignment.LEFT,10,"white","------------------------------------------------------------");
       //      NewLine( ParagraphAlignment.LEFT,10,"white","编号1987："+"10001");
      
       //CreateParagraph(ParagraphAlignment.CENTER,20,"black","这是我的测试报告！")
       // CreateParagrah(ParagraphAlignment.CENTER,20,"white","222");
       // NewLine(ParagrapAlignment.LEFT,10,"white","222");
       
    
    }
  private void CreateParagraph(ParagraphAlignment _alignment, int _fontSize,
        string _color, string _content)
    {
        XWPFParagraph paragraph= doc.CreateParagraph();
        paragraph.Alignment = _alignment;
        XWPFRun   run = paragraph.CreateRun();
        run.FontSize = _fontSize;
        run.SetColor(_color);
        run.FontFamily = "宋体";
        run.SetText(_content);
        FileStream   fs;
     if(File.Exists(SavePath1)){
      fs  =new FileStream(SavePath1,FileMode.Append);

     }
     else{
     fs = new FileStream(SavePath1, FileMode.Create);
     }
      
        doc.Write(fs);
        fs.Close();
        fs.Dispose();
        Debug.Log("写入成功");
    }
 
    private void NewLine( ParagraphAlignment _alignment,int _fontSize,string _color,string value)
        {
                 XWPFParagraph paragraph= doc.CreateParagraph();
                // paragraph.CreateRun().AddCarriageReturn();//新增式换行--当前行之后
                   paragraph.Alignment = _alignment;
        XWPFRun   run = paragraph.CreateRun();
        run.AddCarriageReturn();
           // value = value + "\r\n";
              run.FontSize = _fontSize;
        run.SetColor(_color);
        run.FontFamily = "宋体";
       // run.SetText(_content);
            
            run.SetText(value);
            
        FileStream     fs =new FileStream(SavePath1,FileMode.Append);
               doc.Write(fs);
        fs.Close();
        fs.Dispose();
   

        }
   private void NewLineInDoc(ParagraphAlignment _alignment,int _fontSize,string _color,string value){
         XWPFParagraph paragraph=doc.CreateParagraph();
         paragraph.Alignment=_alignment;
         XWPFRun run = paragraph.CreateRun();
         run.AddCarriageReturn();
         run.FontSize=_fontSize;
         run.SetColor(_color);
         run.FontFamily="宋体";
         run.SetText(value);
         FileStream fs = new FileStream(SavePath1,FileMode.Append);
         doc.Write(fs);
         fs.Close();
         fs.Dispose();


   }
      
    void CreatWordDocument(){
       XWPFDocument doc = new XWPFDocument();
      FileStream file= new FileStream(@"C:/Users/Administrator/Desktop/liuyu.docx",FileMode.OpenOrCreate,FileAccess.ReadWrite);

        var p1 = doc.CreateParagraph();
            p1.Alignment = ParagraphAlignment.CENTER; //字体居中
               var runTitle = p1.CreateRun();
            runTitle.IsBold = true;
            runTitle.SetText("VR心理训练系统数据报告");
            runTitle.FontSize = 16;
            runTitle.SetFontFamily("宋体", FontCharRange.None); 

              //创建段落对象2
            var p2 = doc.CreateParagraph();
            var run1 = p2.CreateRun();
            run1.SetText("---------------------------------------------------------------------");
            run1.FontSize = 12;
            run1.SetFontFamily("华文楷体", FontCharRange.None); //设置雅黑字体 

            ///创建段落对象3
            
                  var p3 = doc.CreateParagraph();
            var run2 = p3.CreateRun();
            run2.SetText("  编号："+u0+"        性    别:"+u1+"                所    属："+u2);
            run2.FontSize = 10;
            run2.SetFontFamily("华文楷体", FontCharRange.None); //设置雅黑字体  

        var p4 = doc.CreateParagraph();
        var run3 = p4.CreateRun();
        run3.SetText("  姓名：" + u3 + "          年    龄:" + u4 + "                  测试日期：" + u5);
        run3.FontSize = 10;
        run3.SetFontFamily("华文楷体", FontCharRange.None); //设置雅黑字体  

        var p5 = doc.CreateParagraph();
        var run4 = p5.CreateRun();
        run4.SetText("  模式：" + u6 + "    开始时间:" + u7 + "    结束日期：" + u8);
        run4.FontSize = 10;
        run4.SetFontFamily("华文楷体", FontCharRange.None); //设置雅黑字体  

        var p6 = doc.CreateParagraph();
        var run6 = p6.CreateRun();
        run6.SetText("---------------------------------------------------------------------");
        run6.FontSize = 10;
        run6.SetFontFamily("华文楷体", FontCharRange.None); //设置雅黑字体  

    }

     // var p6 =doc.CreateParagph();
     // var run6 =p6.CreateRun();
     // run6.SetText("...测试");
     //run6.SetFontFamily("微软黑体"，FontCharRange.None);


}
