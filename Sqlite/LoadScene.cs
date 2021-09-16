using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public static LoadScene loadscene;
    public Slider progressBar;
    private string loadscenename;
    public Text LoadingText;

    // 目标进度
    float target = 0;
    // 读取场景的进度，取值范围0~1
    float progress = 0;
    // 异步对象
    AsyncOperation op = null;
    [HideInInspector]
    public bool IsLoadOver = false;
    void Start()
    {
        loadscene = this;
        Debug.Log("开始LoadScene");
        loadscenename = PlayerPrefs.GetString("LoadScene");
        op = SceneManager.LoadSceneAsync(loadscenename);
        op.allowSceneActivation = false;
        progressBar.value = 0;

        // 开启协程，开始调用加载方法
        StartCoroutine(processLoading());
    }

    float dtimer = 0;
    void Update()
    {
        progressBar.value = Mathf.Lerp(progressBar.value, target, dtimer * 0.015f);
        dtimer += Time.deltaTime;
        LoadingText.text = "加载进度：  "+((int)((progressBar.value)*100)).ToString()+"%";
        if (progressBar.value > 0.99f)
        {
            progressBar.value = 1;
            op.allowSceneActivation = true;
            IsLoadOver = true;
        }
    //            progressBar.value = Mathf.Lerp(progressBar.value, 1, dtimer * 0.015f);
    //     dtimer += Time.deltaTime;
    //     LoadingText.text = "加载进度：  "+((int)((progressBar.value)*100)).ToString()+"%";
    //  if (progressBar.value > 0.99f)
    //     {
    //         progressBar.value = 1;
    //       if(!IsLoadOver){
    //           SceneManager.LoadScene(loadscenename);
    //           IsLoadOver = true;
    //       }
            
    //     }
    
    }

    // 加载进度
    IEnumerator processLoading()
    {
        while (true)
        {
            target = op.progress; // 进度条取值范围0~1
            if (target >= 0.9f)
            {
                target = 1;
                yield break;
            }
            yield return 0;
        }
    }

}