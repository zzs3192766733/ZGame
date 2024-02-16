//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/31 8:36:45
//========================================================

using System;
using GameFramework.Common;
using GameFramework.Math;
using GameFramework.Setting;
using GameFramework.Time;
using UnityEngine;
using UnityEngine.UI;

public class TestBox : MonoBehaviour
{
    public float angle;
    public float radius;
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Text _text;

    private void Awake()
    {
        TimeManager.Instance.DoTimeTask(TimeSpan.FromSeconds(0.2f),
            () => { _text.text = ((int) (1.0f / Time.deltaTime)).ToString(); }, this.gameObject);
    }

    public void Test(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameLogger.Log("player进入");
        }
    }

    public float f_UpdateInterval = 0.5F;

    private float f_LastInterval;

    private int i_Frames = 0;

    private float f_Fps;

    void Start()
    {
        //Application.targetFrameRate=60;

        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;

        foreach (var item in Screen.resolutions)
        {
            Debug.Log(item.height + " : " + item.width + " : " + item.refreshRate);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 100, 200, 200), "FPS:" + f_Fps.ToString("f2"));
    }

    void Update()
    {
        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            RunTimeGlobalSettingMgr.Instance.SetFullScreen();
        }
        else if(Input.GetKeyDown(KeyCode.W))
        {
            RunTimeGlobalSettingMgr.Instance.SetWindowScreen();
        }
    }


    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         RunTimeGlobalSettingMgr.Instance.Set(new Resolution(){width = 1920,height = 1080,refreshRate = 60});
    //     }
    // }
}