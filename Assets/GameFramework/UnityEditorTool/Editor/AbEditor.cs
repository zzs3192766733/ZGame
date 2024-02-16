//========================================================
// 描述:AssetBundle打包编辑器
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/24 8:43:18
//========================================================

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using GameFramework.Common;
using GameFramework.Res;
using GameFramework.Tool;

namespace GameFramework.UnityEditorTool.Editor
{
    public static class AbEditor
    {
        private static List<AssetBundleBuild> _assetBundleBuilds = new List<AssetBundleBuild>();
        private static Dictionary<string, string> _asset2Bundle = new Dictionary<string, string>();
        private static Dictionary<string, List<string>> _asset2Dependencies = new Dictionary<string, List<string>>();

        [MenuItem("GameFramework/BuildAssetBundle")]
        public static void Build()
        {
            GameLogger.Log("---开始打包---");
            var abOutPutPath = GameFrameworkGlobalConfig.AbOutputPath;
            var rootPath = GameFrameworkGlobalConfig.GAssetPath;

            if (Directory.Exists(abOutPutPath))
                Directory.Delete(abOutPutPath, true);
            var dirInfo = new DirectoryInfo(rootPath);
            var moduleDirInfos = dirInfo.GetDirectories();
            foreach (var moduleDirInfo in moduleDirInfos)
            {
                var moduleName = moduleDirInfo.Name;
                _assetBundleBuilds.Clear();
                _asset2Bundle.Clear();
                _asset2Dependencies.Clear();
                ScanChildDirectories(moduleDirInfo);
                AssetDatabase.Refresh();
                var moduleOutPutPath = abOutPutPath + "/" + moduleName;
                if (Directory.Exists(moduleOutPutPath))
                    Directory.Delete(moduleOutPutPath);
                Directory.CreateDirectory(moduleOutPutPath);
                BuildPipeline.BuildAssetBundles(moduleOutPutPath, _assetBundleBuilds.ToArray(),
                    GameFrameworkGlobalConfig.BuildAssetBundleOptions,
                    GameFrameworkGlobalConfig.BuildAssetBundleTarget);
                CalculateDependencies();
                SaveAbModuleConfig(moduleName);
                AssetDatabase.Refresh();
            }

            GameLogger.Log("---打包完成---");
        }

        private static void ScanChildDirectories(DirectoryInfo moduleDirInfo)
        {
            ScanCurrDirectories(moduleDirInfo);
            var dirInfos = moduleDirInfo.GetDirectories();
            foreach (var dirInfo in dirInfos)
            {
                ScanChildDirectories(dirInfo);
            }
        }

        private static void ScanCurrDirectories(DirectoryInfo moduleDirInfo)
        {
            var assetNameLst = new List<string>();
            var fileInfos = moduleDirInfo.GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                if (fileInfo.FullName.EndsWith(".meta"))
                    continue;
                var assetName = fileInfo.FullName.Substring(Application.dataPath.Length - "Assets".Length)
                    .Replace('\\', '/');
                assetNameLst.Add(assetName);
            }

            if (assetNameLst.Count > 0)
            {
                var assetBundleName = moduleDirInfo.FullName.Substring(Application.dataPath.Length + 1)
                    .Replace('\\', '_').ToLower();
                var bundleBuild = new AssetBundleBuild
                {
                    assetNames = new string[assetNameLst.Count],
                    assetBundleName = assetBundleName
                };
                for (var i = 0; i < assetNameLst.Count; i++)
                {
                    bundleBuild.assetNames[i] = assetNameLst[i];
                    _asset2Bundle.Add(assetNameLst[i], assetBundleName);
                }

                _assetBundleBuilds.Add(bundleBuild);
            }
        }

        private static void CalculateDependencies()
        {
            foreach (var asset in _asset2Bundle.Keys)
            {
                var selfBundle = _asset2Bundle[asset];
                var dependencies = AssetDatabase.GetDependencies(asset);
                var assetDependenciesList = dependencies.Where(str => !str.EndsWith(".cs") && str != asset).ToList();
                if (assetDependenciesList.Count > 0)
                {
                    var lst = new List<string>();
                    foreach (var oneAsset in assetDependenciesList)
                    {
                        if (!_asset2Bundle.TryGetValue(oneAsset, out var bundle)) continue;
                        if (bundle != selfBundle)
                            lst.Add(bundle);
                    }

                    _asset2Dependencies.Add(asset, lst);
                }
            }
        }

        private static void SaveAbModuleConfig(string moduleName)
        {
            var moduleAbConfig = new ModuleABConfig(_asset2Bundle.Count);
            //装配BundleInfo
            foreach (var assetBundle in _assetBundleBuilds)
            {
                var bundleInfo = new BundleInfo {BundleName = assetBundle.assetBundleName, Assets = new List<string>()};
                foreach (var assetName in assetBundle.assetNames)
                    bundleInfo.Assets.Add(assetName);
                var path = GameFrameworkGlobalConfig.AbOutputPath + "/" + moduleName + "/" + bundleInfo.BundleName;
                using (var fs = File.OpenRead(path))
                {
                    var crc = AssetUtility.GetCRC32Hash(fs);
                    bundleInfo.CRC = crc;
                }

                moduleAbConfig.AddBundle(bundleInfo.BundleName, bundleInfo);
            }

            //装配AssetInfo
            var assetIndex = 0;
            foreach (var item in _asset2Bundle)
            {
                var assetInfo = new AssetInfo
                {
                    BundleName = item.Value,
                    AssetPath = item.Key,
                    Dependencies = new List<string>()
                };
                if (_asset2Dependencies.TryGetValue(item.Key, out var lst))
                {
                    foreach (var t in lst)
                        assetInfo.Dependencies.Add(t);
                }
                moduleAbConfig.AddAsset(assetIndex++,assetInfo);
            }
            
            //写入Json文件到各个模块
            var moduleConfigName = moduleName.ToLower() + ".json";

            var jsonPath = GameFrameworkGlobalConfig.AbOutputPath + "/" + moduleName + "/" + moduleConfigName;

            if (File.Exists(jsonPath))
                File.Delete(jsonPath);
            File.Create(jsonPath).Dispose();
            var jsonData = LitJson.JsonMapper.ToJson(moduleAbConfig);
            File.WriteAllText(jsonPath, ToolUtility.ConvertJsonString(jsonData));
        }
    }
}