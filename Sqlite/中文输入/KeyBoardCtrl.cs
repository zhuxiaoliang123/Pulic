using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using VRTK;
public class KeyBoardCtrl : MonoBehaviour
{
    public GameObject keyBoard;
    [HideInInspector]
    public string target;
    private inputManager inputManagerr;
    public GameObject[] letter;
    public GameObject[] number;
    public Button deleteBtn;
    public Text text1;
    public Text text2;
    public GameObject DisplayPanel;
    public GameObject KeyBoard;
    public GameObject InputFiled;
    public GameObject LeterContent;
    public GameObject numberPanel;
    private string Language = "Chinese";
    public Button numberBtn;
    public Button languageBtn;
    [HideInInspector]
    public InputField[] pwdInput=new InputField[6];
    // Start is called before the first frame update
    void Start()
    {
        inputManagerr = this.GetComponent<inputManager>();
        deleteBtn.onClick.AddListener(OnDeleteLastLetter);
        numberBtn.onClick.AddListener(OnNumberBtn);
        languageBtn.onClick.AddListener(OnChangelanguageBtn);
   //     GetComponent<VRTK_UICanvas>().enabled=true;
    }

    // Update is called once per frame
   
    public void ShowKeyBoard(string name)
    {
       // inputManagerr.disp =
        keyBoard.GetComponent<Canvas>().enabled = true;
        
        target = name;
        CheckTarget(target);
    }

    public void CheckTarget(string t)
    {
        inputManagerr.disp = GameObject.FindGameObjectWithTag(t).GetComponent<InputField>();
        switch (t)
        {
            case "name": DisplayPanel.SetActive(true);
                text1.fontSize = 20;
                text2.fontSize = 10;
                numberPanel.SetActive(false);
                InputFiled.SetActive(true); break;
            case "password":
                DisplayPanel.SetActive(false);
                text1.fontSize = 10;
                text2.fontSize = 20;
                InputFiled.SetActive(false) ; break;
            case "insertname":
                DisplayPanel.SetActive(true);
                text1.fontSize = 20;
                text2.fontSize = 10;
                numberPanel.SetActive(false);
                InputFiled.SetActive(true); break;
            case "insertage":
                DisplayPanel.SetActive(false);
                text1.fontSize = 10;
                text2.fontSize = 20;
                InputFiled.SetActive(false); break;
            case "insertsex":
                DisplayPanel.SetActive(true);
                LeterContent.SetActive(true);
                text1.fontSize = 20;
                text2.fontSize = 10;
                numberPanel.SetActive(false);
                InputFiled.SetActive(true); break;
            case "insertpassword":
                DisplayPanel.SetActive(false);
                text1.fontSize = 10;
                text2.fontSize = 20;
                InputFiled.SetActive(false); break;
            case "inserttel":
                DisplayPanel.SetActive(false);
                text1.fontSize = 10;
                text2.fontSize = 20;
                InputFiled.SetActive(false); break;
            case "insertid":
                DisplayPanel.SetActive(false);
                text1.fontSize = 10;
                text2.fontSize = 20;
                InputFiled.SetActive(false); break;
            case "PWD1":
                DisplayPanel.SetActive(false);
                text1.fontSize = 10;
                text2.fontSize = 20;
                InputFiled.SetActive(false); break;
            case "PWD2":
                DisplayPanel.SetActive(false);
                text1.fontSize = 10;
                text2.fontSize = 20;
                InputFiled.SetActive(false); break;
            case "PWD3":
                DisplayPanel.SetActive(false);
                text1.fontSize = 10;
                text2.fontSize = 20;
                InputFiled.SetActive(false); break;
            case "PWD4":
                DisplayPanel.SetActive(false);
                text1.fontSize = 10;
                text2.fontSize = 20;
                InputFiled.SetActive(false); break;
            case "PWD5":
                DisplayPanel.SetActive(false);
                text1.fontSize = 10;
                text2.fontSize = 20;
                InputFiled.SetActive(false); break;
            case "PWD6":
                DisplayPanel.SetActive(false);
                text1.fontSize = 10;
                text2.fontSize = 20;
                InputFiled.SetActive(false); break;
        }
    }
    public void OnLetterBtnClick(int num)
    {
        if (target == "name" || target == "insertname" || target == "insertsex")
        {
            DisplayPanel.SetActive(true);
            InputFiled.SetActive(true);
            inputManagerr.input.text += letter[num].name;
        }
        else if (target == "password"||target == "insertpassword"|| target == "insertid")
        {
            GameObject.FindGameObjectWithTag(target).GetComponent<InputField>().text += letter[num].name;
        }
        else if (target == "PWD1" || target == "PWD2" || target == "PWD3" || target == "PWD4" || target == "PWD5" || target == "PWD6")
        {
            GameObject.FindGameObjectWithTag(target).GetComponent<InputField>().text += number[num].name;
        }
    }
    public void OnNumberBtn(int num)
    {
        if (target == "name"|| target == "insertname"|| target == "insertsex") return;
        else if (target == "password"|| target == "insertage"|| target == "insertpassword"|| target == "inserttel")
        {
            GameObject.FindGameObjectWithTag(target).GetComponent<InputField>().text += number[num].name;
        }
       else if(target=="PWD1"|| target == "PWD2" || target == "PWD3" || target == "PWD4" || target == "PWD5" || target == "PWD6")
        {
            GameObject.FindGameObjectWithTag(target).GetComponent<InputField>().text += number[num].name;
        }
    }
    public void OnDeleteLastLetter()
    {
        if (target == "name"|| target == "insertname" || target == "insertsex")
        {
            if (inputManagerr.input.text.Length > 0)
            {
                inputManagerr.input.text = inputManagerr.input.text.Substring(0, inputManagerr.input.text.Length - 1);
            }
            else return;
        }
        else if (target == "password"|| target == "insertage"|| target == "insertpassword"|| target == "inserttel"|| target == "inserid")
        {
            string PWD = GameObject.FindGameObjectWithTag(target).GetComponent<InputField>().text;
            if (PWD.Length > 0)
            {
                PWD =PWD.Substring(0, PWD.Length - 1);
            }
            else return;
        }
    }
    public void OnChangelanguageBtn()
    {
        LeterContent.SetActive(true);
        numberPanel.SetActive(false);
        if(Language=="Chinese")
        {
            DisplayPanel.SetActive(false);
            InputFiled.SetActive(false);
            text1.fontSize = 10;
            text2.fontSize = 20;
            Language = "English";
        }
        else if(Language=="English")
        {
            DisplayPanel.SetActive(true);
            InputFiled.SetActive(true);
            text1.fontSize = 20;
            text2.fontSize = 10;
            Language = "Chinese";
        }
    }
    public void OnNumberBtn()
    {
        numberPanel.SetActive(true);
        DisplayPanel.SetActive(false);
        InputFiled.SetActive(false);
        LeterContent .SetActive(false);
    }
}
