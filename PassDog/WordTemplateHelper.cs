
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPOI.XWPF.UserModel;
using System.IO;
using System;
using NPOI.OpenXmlFormats.Wordprocessing;
using ICSharpCode.SharpZipLib.Zip;
using System.Linq;
using System.Text;
using LitJson;

public class WordTemplateHelper
{




    // public static byte[] above;
    // public static byte [] font;
    // public static byte[] left;
    // public static byte[] right;
   
  //  private static Dictionary<int ,List<string>> dicPool = new Dictionary<int, List<string>>(); 
   // public static List<Operation> operationList=null;
    /// <summary>
    /// NPOI操作word
    /// </summary>
    /// <param name="TemplatePath">模板路径</param>
    /// <param name="SavePath">保存路径</param>
    /// <param name="keywords">关键字集合</param>

    public static void ExportPublicationOfResult(string TemplatePath, string SavePath, Dictionary<string, string> keywords,string vaue)
    {
        //       XWPFTable table0=  document.Tables[1];
        //  if(operationList!=null&&operationList.Count>0){
        // for (int i = 0; i < operationList.Count-1; i++)
        // {
        //      XWPFTableRow row = table0.CreateRow();
        // }
        //  for (var i = 0; i < operationList.Count; i++)
        //     {
        //         table0.GetRow(i)
        //             .GetCell(0)
        //             .SetParagraph(SetCellText(document, table0, i.ToString(), ParagraphAlignment.CENTER, 20));
        //         table0.GetRow(i)
        //             .GetCell(1)
        //             .SetParagraph(SetCellText(document, table0, operationList[i].model, ParagraphAlignment.CENTER, 20));
        //         table0.GetRow(i)
        //             .GetCell(2)
        //             .SetParagraph(SetCellText(document, table0, operationList[i].begin, ParagraphAlignment.CENTER, 20));
        //         table0.GetRow(i)
        //             .GetCell(3)
        //             .SetParagraph(SetCellText(document, table0, operationList[i].end, ParagraphAlignment.CENTER, 20));
        //             string str ="无操作";
        //            int type= (int)operationList[i].ot;
        //             if(type==0){
        //                    str="移动";
        //             }
        //             else if(type==1){
        //                     str ="删除";
        //             }else if(type==2){
        //                  str ="添加";
        //             }
        //             else if(type==3){
        //                   str ="旋转";
        //             }        
        //             else if(type==4){
        //                   str ="选择";
        //             }else if(type==5){
        //                   str ="拖拽";
        //             }else if(type==6){
        //                    str ="无操作";
        //             }
        //             else {
        //                 str ="缩放";
        //             }
        //         table0.GetRow(i)
        //             .GetCell(4)
        //             .SetParagraph(SetCellText(document, table0, str, ParagraphAlignment.CENTER, 20));
        //     }
        
        //  }
       
    }
   
    public static void WriteToPublicationOfResult(string TemplatePath, string SavePath, Dictionary<string, string> keywords,string vaue)
    {


  

       // dicPool =new Dictionary<int, List<string>>();
        FileStream fs = new FileStream(TemplatePath, FileMode.Open, FileAccess.Read);
        XWPFDocument document = new XWPFDocument(fs);

         
             
      
          
        foreach (var row in document.Tables[0].Rows)
        {
           foreach (var cell in row.GetTableCells())
           {
               ReplaceKeyWords(cell.Paragraphs, keywords);//替换表格中的关键字
           }
        }
        //document.Tables[0].GetRow(0).MergeCells(0,2);
        //document.Tables[0].GetRow(1).MergeCells(0,2);
        //document.Tables[0].GetRow(2).MergeCells(0,2);

      var list= vaue.Split('-');
 
   var ParamTemplate1 = new
        {
            b0=list[0],
            b1=list[1],
            b2=list[2],
            b3=list[3],
            b4=list[4],
            b5=list[5],
        };

        foreach (var row in document.Tables[1].Rows)
        {
           foreach (var cell in row.GetTableCells())
           {
               ReplaceKeyWords(cell.Paragraphs, getProperties(ParamTemplate1));//替换表格中的关键字
           }
        }

     // ReplaceKeyWords(document.Paragraphs, getProperties(ParamTemplate1));

        //  var data3Valuess=  JsonMapper.ToObject<Vector2[]>(vaue);

        //  XWPFTable table0=  document.Tables[1];
        //  if(data3Valuess!=null&&data3Valuess.Length>0){
        // for (int i = 0; i < data3Valuess.Length-1; i++)
        // {
        //      XWPFTableRow row = table0.CreateRow();
        // }
        //  for (var i = 0; i < data3Valuess.Length; i++)
        //     {
        //         table0.GetRow(i)
        //             .GetCell(0)
        //             .SetParagraph(SetCellText(document, table0, i.ToString(), ParagraphAlignment.CENTER, 20));
        //         table0.GetRow(i)
        //             .GetCell(1)
        //             .SetParagraph(SetCellText(document, table0, data3Valuess[i].x.ToString(), ParagraphAlignment.CENTER, 20));
        //         table0.GetRow(i)
        //             .GetCell(2)
        //             .SetParagraph(SetCellText(document, table0, data3Valuess[i].y.ToString(), ParagraphAlignment.CENTER, 20));
             
        //     }
        
        //  }
       

       
    
       
           








        // foreach (var table in document.Tables)
        // {

        // foreach (var row in document.Tables[0].Rows)
        //    {
        //        foreach (var cell in row.GetTableCells())
        //        {
        //            ReplaceKeyWords(cell.Paragraphs, keywords);//替换表格中的关键字         ////////////////替换表格中的关键字
        //        }
        //    }
        // }




            document.Document.body.sectPr = new CT_SectPr();
             CT_SectPr m_SectPr = document.Document.body.sectPr;
              XWPFParagraph m_xp = document.CreateParagraph();
            if(!m_xp.IsEmpty){
                 m_xp.CreateRun().AddBreak();   //分页

            }
          
            CT_Ftr m_ftr = new CT_Ftr();

            m_ftr.Items = new System.Collections.ArrayList();

            CT_SdtBlock m_Sdt = new CT_SdtBlock();

            CT_SdtPr m_SdtPr = m_Sdt.AddNewSdtPr();

            CT_SdtDocPart m_SdtDocPartObj = m_SdtPr.AddNewDocPartObj();

         //  m_SdtDocPartObj.AddNewDocPartGallery().val = "PageNumbers (Bottom of Page)";

            m_SdtDocPartObj.docPartUnique = new CT_OnOff();

              CT_SdtContentBlock m_SdtContent = m_Sdt.AddNewSdtContent();

            CT_P m_SdtContentP = m_SdtContent.AddNewP();

            CT_PPr m_SdtContentPpPr = m_SdtContentP.AddNewPPr();

            m_SdtContentPpPr.AddNewJc().val = ST_Jc.center;

            m_SdtContentP.Items = new System.Collections.ArrayList();

            CT_SimpleField m_fldSimple = new CT_SimpleField();

            //m_fldSimple.instr ="PAGE \\*MERGEFORMAT ";   //mergformat
      
            m_fldSimple.instr=" PAGE \\*MERGEFORMAT";//+ "NUMPAGES \\*MERGEFORMAT";

           m_SdtContentP.Items.Add(m_fldSimple);

            m_ftr.Items.Add(m_Sdt);
        //创建页脚关系（footern.xml）

            XWPFRelation Frelation = XWPFRelation.FOOTER;

            XWPFFooter m_f = (XWPFFooter)document.CreateRelationship(Frelation,XWPFFactory.GetInstance(),document.FooterList.Count + 1);

            //设置页脚

            m_f.SetHeaderFooter(m_ftr);

            CT_HdrFtrRef m_HdrFtr = m_SectPr.AddNewFooterReference();

            m_HdrFtr.type = ST_HdrFtr.@default;

            m_HdrFtr.id =m_f.GetPackageRelationship().Id;









         
        FileStream output = new FileStream(SavePath, FileMode.Create);
        document.Write(output);
       fs.Close();
        fs.Dispose();
        output.Close();
        output.Dispose();
        Debug.Log("修改word成功！！！！！！！！！！！！！");
    }
    
        public static XWPFParagraph SetCellText(XWPFDocument doc, XWPFTable table, string setText, ParagraphAlignment align,
            int textPos)
        {
            var para = new CT_P();
            var pCell = new XWPFParagraph(para, table.Body);
            pCell.Alignment = ParagraphAlignment.LEFT;//字体
            pCell.Alignment = align;

            var r1c1 = pCell.CreateRun();
            r1c1.SetText(setText);
            r1c1.FontSize = 10;
            r1c1.SetFontFamily("华文楷体", FontCharRange.None); //设置雅黑字体
            r1c1.SetTextPosition(textPos); //设置高度

            return pCell;
        }
private static void CreateParagraph(XWPFDocument doc,ParagraphAlignment _alignment, string _content)
    {
        XWPFParagraph paragraph= doc.CreateParagraph();
        paragraph.Alignment = _alignment;
        XWPFRun   run = paragraph.CreateRun();
        // run.FontSize = _fontSize;
        // run.SetColor(_color);
        run.FontFamily = "宋体";
        run.SetText(_content);
   
        Debug.Log("写入成功");
    }
  
    private static void NewLine(XWPFDocument doc, ParagraphAlignment _alignment,string value)
        {
                 XWPFParagraph paragraph= doc.CreateParagraph();
                // paragraph.CreateRun().AddCarriageReturn();//新增式换行--当前行之后
                   paragraph.Alignment = _alignment;
        XWPFRun   run = paragraph.CreateRun();
        run.AddCarriageReturn();
        //       run.FontSize = _fontSize;
        // run.SetColor(_color);
        run.FontFamily = "宋体";
            
            run.SetText(value);
  
        }

    private static void OperatePic(XWPFDocument doc, string flag,string name){

         var p1 = Application.streamingAssetsPath+"/ScreenShot/"+name+".jpg";
        //    if(Directory.Exists(p1)){
        //                     return ;
        //                   }
                 foreach (var para in doc.Paragraphs)
                {
                    string oldtext = para.ParagraphText;
                    if (oldtext == "")
                        continue;
                    string tempText = para.ParagraphText;
                    if(tempText.Contains(flag))
                    {
                        tempText = tempText.Replace(flag, "");
                        var pos = doc.GetPosOfParagraph(para);
                        var run = para.CreateRun();

                       
                      
                         using (FileStream picData = new FileStream(p1, FileMode.Open, FileAccess.Read))
                         {
                            run.AddPicture(picData, (int)PictureType.PNG,name+".jpg", 480 * 6000, 270 * 6000);
                            
                            
                        }
                    }

             
                    if(tempText.Contains("#LoverName#"))
                    {
                        tempText = tempText.Replace("#LoverName#", "Eyes Open");
                    }

                    if(tempText.Contains("#JJLength#"))
                    {
                        tempText = tempText.Replace("#JJLength", "-0.2 ~ 0.7");
                    }
                    para.ReplaceText(oldtext, tempText);
                }
    }
    public static void WriteParagraphText( XWPFDocument doc, string flag,string value )
    {

        foreach (var para in doc.Paragraphs)
        {

            string oldText = para.ParagraphText;
            if (oldText == "")
            {
                continue;
            }
            string tempText = para.ParagraphText;
            if (tempText.Contains(flag))
            {
                tempText = tempText.Replace(flag, value);
            }
            para.ReplaceText(oldText, tempText);
        }

    }
    /// <summary>
    /// 遍历段落，替换关键字
    /// </summary>
    /// <param name="Paragraphs">段落</param>
    /// <param name="keywords">关键字集合</param>
    public static void ReplaceKeyWords(IList<XWPFParagraph> Paragraphs, Dictionary<string, string> keywords)
    {
        foreach (var item in keywords)
        {
            foreach (var para in Paragraphs)
            {
                string oldtext = para.ParagraphText;
                if (oldtext == "") continue;
                string temptext = para.ParagraphText;
                if (temptext.Contains("{$" + item.Key + "}")) temptext = temptext.Replace("{$" + item.Key + "}", item.Value);
                para.ReplaceText(oldtext, temptext);
            }
        }

    }
    /// <summary>
    /// 格式化关键字集合
    /// </summary>
    /// <typeparam name="T">泛型对象</typeparam>
    /// <param name="t">关键字集对象</param>
    /// <returns></returns>
    public static Dictionary<string, string> getProperties<T>(T t)
    {
        Dictionary<string, string> keywords = new Dictionary<string, string>();
        if (t == null)
        {
            return keywords;
        }
        System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

        if (properties.Length <= 0)
        {
            return keywords;
        }
        foreach (System.Reflection.PropertyInfo item in properties)
        {
            string name = item.Name;
            object value = item.GetValue(t, null);
            if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
            {
                keywords.Add(name, value.ToString());
            }
            else
            {
                getProperties(value);
            }
        }
        return keywords;
    }


}
