//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/27 15:30:44
//========================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GameFramework.Common;
using UniRx;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DownLoad : MonoBehaviour
{
    private void Start()
    {
        // var url =
        //     "https://xy218x24x86x203xy.mcdn.bilivideo.cn:4483/upgcxcode/53/92/467709253/467709253_nb2-1-30080.m4s?e=ig8euxZM2rNcNbdlhoNvNC8BqJIzNbfqXBvEqxTEto8BTrNvN0GvT90W5JZMkX_YN0MvXg8gNEV4NC8xNEV4N03eN0B5tZlqNxTEto8BTrNvNeZVuJ10Kj_g2UB02J0mN0B5tZlqNCNEto8BTrNvNC7MTX502C8f2jmMQJ6mqF2fka1mqx6gqj0eN0B599M=&uipk=5&nbs=1&deadline=1640596169&gen=playurlv2&os=mcdn&oi=2945750967&trid=0001b359949dcc97470d8fa4bca1f8ea2f09u&platform=pc&upsig=22e10db48ea881aabd0e2874ccf657f1&uparams=e,uipk,nbs,deadline,gen,os,oi,trid,platform&mcdnid=1002385&mid=38956200&bvc=vod&nettype=0&orderid=0,3&agrr=1&bw=153326&logo=A0000001";
        // var headers = new Dictionary<string, string>();
        // headers.Add("origin", "https://www.bilibili.com");
        // headers.Add("referer", "https://www.bilibili.com/video/BV1VY411p7PJ");
        // headers.Add("user-agent",
        //     "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36");
        // var outPut = @"D:/MyEVMP4";
        // ObservableWWW.GetAndGetBytes(url, headers).Subscribe(buffer =>
        // {
        //     if (!Directory.Exists(outPut))
        //         Directory.CreateDirectory(outPut);
        //     using (var fs = new FileStream(outPut+"/bilibili.mp4",FileMode.Create))
        //     {
        //         fs.Write(buffer,0,buffer.Length);
        //         GameLogger.Log("完成");
        //     }
        // });

        // var p = new Process
        // {
        //     StartInfo =
        //     {
        //         FileName = "cmd.exe",
        //         UseShellExecute = false,
        //         RedirectStandardInput = true,
        //         RedirectStandardOutput = true,
        //         RedirectStandardError = true,
        //         CreateNoWindow = false
        //     }
        // };
        // try
        // {
        //     //p.Start();
        //     //var strOutput = "ipconfig";
        //     //p.StandardInput.WriteLine(strOutput);
        //     //p.Close();
        // }
        // catch (Exception)
        // {
        //     // ignored
        //     Debug.Log("error");
        // }

        ObservableWWW.Get("https://www.bilibili.com/video/BV1dR4y1W7XN?spm_id_from=333.5.0.0").Subscribe(html =>
        {
            GameLogger.Log(html);
        });

    }
}