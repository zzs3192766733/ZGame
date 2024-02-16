//========================================================
// 描述:数据保存管理器（以Json格式保存到本地的persistentDataPath）
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/10 10:51:10
//========================================================

using GameFramework.Common;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LitJson;

namespace GameFramework.Save
{
	
	public class SaveDataManager : SingleBase<SaveDataManager>
	{
		private readonly string _savePath = Application.persistentDataPath + "/SaveData/";
		private readonly Dictionary<string,SaveDataBase> _key2DataCache = new Dictionary<string, SaveDataBase>();
		/// <summary>
		/// 异步保存数据
		/// </summary>
		/// <param name="key">数据key</param>
		/// <param name="val">数据值</param>
		/// <typeparam name="T">数据类型</typeparam>
		/// <returns></returns>
		public async Task SaveDataAsync<T>(string key, T val)where T : SaveDataBase
		{
			var jsonData = JsonMapper.ToJson(val);
			//保存json数据到本地,并更新缓存
			if (!Directory.Exists(_savePath))
				Directory.CreateDirectory(_savePath);
			var path = _savePath + key + ".json";
			if (!File.Exists(path))
				File.Create(path).Close();
			using (var sw = new StreamWriter(path))
			{
				_key2DataCache[key] = val;
				await sw.WriteAsync(jsonData);
			}
		}
		/// <summary>
		/// 同步保存数据
		/// </summary>
		/// <param name="key">数据key</param>
		/// <param name="val">数据值</param>
		/// <typeparam name="T">数据类型</typeparam>
		public void SaveData<T>(string key, T val) where T : SaveDataBase
		{
			var jsonData = JsonMapper.ToJson(val);
			//保存json数据到本地,并更新缓存
			if (!Directory.Exists(_savePath))
				Directory.CreateDirectory(_savePath);
			var path = _savePath + key + ".json";
			if (!File.Exists(path))
				File.Create(path).Close();
			File.WriteAllText(path,jsonData);
			_key2DataCache[key] = val;
		}
		/// <summary>
		/// 同步加载保存数据
		/// </summary>
		/// <param name="key">数据key</param>
		/// <typeparam name="T">数据类型</typeparam>
		/// <returns></returns>
		public T GetData<T>(string key) where T : SaveDataBase
		{
			//1.先检查缓存中是否含有该资源
			SaveDataBase data = null;
			if (_key2DataCache.ContainsKey(key))
			{
				data = _key2DataCache[key];
			}
			//2.没有时去本地加载，放入缓存
			if (data == null)
			{
				var path = _savePath + key + ".json";
				using (var sr = new StreamReader(path))
				{
					var jsonData = sr.ReadToEnd();
					data = JsonMapper.ToObject<T>(jsonData);
					_key2DataCache[key] = data;
				}
			}
			return data as T;
		}
		/// <summary>
		/// 异步加载保存数据
		/// </summary>
		/// <param name="key">数据Key</param>
		/// <typeparam name="T">数据类型</typeparam>
		/// <returns></returns>
		public async Task<T> GetDataAsync<T>(string key) where T : SaveDataBase
		{
			//1.先检查缓存中是否含有该资源
			SaveDataBase data = null;
			if (_key2DataCache.ContainsKey(key))
			{
				data = _key2DataCache[key];
			}
			//2.没有时去本地加载，放入缓存
			if (data == null)
			{
				var path = _savePath + key + ".json";
				using (var sr = new StreamReader(path))
				{
					var jsonData  = await sr.ReadToEndAsync();
					data = JsonMapper.ToObject<T>(jsonData);
					_key2DataCache[key] = data;
				}
			}
			return data as T;
		}
		/// <summary>
		/// 删除内存中缓存的数据
		/// </summary>
		public void Clear()
		{
			_key2DataCache.Clear();
		}
		/// <summary>
		/// 删除本地persistentDataPath下的所有json文件(慎用!!!)
		/// </summary>
		public void DeleteSaveAllData()
		{
			var rootDir = new DirectoryInfo(_savePath);
			var fileInfos = rootDir.GetFiles();
			foreach (var info in fileInfos)
			{
				if (info.FullName.EndsWith(".json"))
				{
					File.Delete(info.FullName);
				}
			}
		}
	}
}
