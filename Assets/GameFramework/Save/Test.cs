//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/10 13:31:17
//========================================================

using System;
using System.Collections.Generic;
using GameFramework.Common;
using GameFramework.Time;
using UnityEngine;
using UniRx;
namespace GameFramework.Save
{
	
	public class Test : MonoBehaviour
	{
	    private async void Start()
	    {
	        var testData = new TestSaveData
	        {
		        name = "lzq",
		        age = 18,
		        sorce = new List<int>{1,2,3,4,5,6,7,8,9}
	        };

	        await SaveDataManager.Instance.SaveDataAsync("Test1", testData);

	        var readData = await SaveDataManager.Instance.GetDataAsync<TestSaveData>("Test1");
	        Debug.Log(readData.name);
	        
	        SaveDataManager.Instance.DeleteSaveAllData();
	        GameLogger.LogWarning("Hello",DebugColor.Yellow);
	        
	        TimeManager.Instance.DoTimeTask(TimeSpan.FromSeconds(1f), () =>
	        {
		        GameLogger.Log("ooo",DebugColor.Red);
	        },this.gameObject);
	        
	        TestDispatcher.Instance.AddEventListener("Hello", (str) =>
	        {
		        GameLogger.Log(str,DebugColor.Green);
	        },this);
	    }
	}
}
