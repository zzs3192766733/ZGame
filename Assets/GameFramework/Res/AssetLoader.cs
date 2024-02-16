//========================================================
// 描述: 资源加载器
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/24 8:24:59
//========================================================

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.ClassExt;
using GameFramework.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework.Res
{
    public class AssetLoader : SingleBase<AssetLoader>
    {
        //对应每个模块的全部资源
        public Dictionary<string, Hashtable> Base2Assets;

        public AssetLoader()
        {
            Base2Assets = new Dictionary<string, Hashtable>();
        }

        public Hashtable ConfigAssembly(ModuleABConfig moduleAbConfig)
        {
            var dic = new Dictionary<string, BundleRef>();
            foreach (var item in moduleAbConfig.BundleArray)
            {
                dic[item.Key] = new BundleRef(item.Value);
            }

            var hash2AssetRef = new Hashtable();
            foreach (var assetInfo in moduleAbConfig.AssetArray)
            {
                var assetRef = new AssetRef(assetInfo);
                assetRef.SelfBundleRef = dic[assetInfo.BundleName];
                var count = assetInfo.Dependencies.Count;
                assetRef.Dependencies = new BundleRef[count];
                for (var i = 0; i < count; i++)
                    assetRef.Dependencies[i] = dic[assetInfo.Dependencies[i]];
                hash2AssetRef.Add(assetInfo.AssetPath, assetRef);
            }

            return hash2AssetRef;
        }

        public async Task<ModuleABConfig> LoadAssetBundleConfig(string moduleName)
        {
            if (ResConfigBase.BundleMode == false)
                return null;
            else
                return await LoadAssetBundleConfig_RunTime(moduleName);
        }

        private async Task<ModuleABConfig> LoadAssetBundleConfig_RunTime(string moduleName)
        {
            var url = Application.streamingAssetsPath + "/" + moduleName + "/" + moduleName.ToLower() + ".json";
            var req = UnityWebRequest.Get(url);
            await req.SendWebRequest();
            return req.error.IsNullOrEmpty()
                ? LitJson.JsonMapper.ToObject<ModuleABConfig>(req.downloadHandler.text)
                : null;
        }

        public GameObject LoadGameObjectNotClone(string moduleName, string path,GameObject go)
        {
            var assetRef = LoadAssetRef<GameObject>(moduleName, path);
            if (assetRef == null || assetRef.Asset == null) return null;
            if (assetRef.Children == null)
                assetRef.Children = new List<GameObject>();
            assetRef.Children.Add(go);
            return assetRef.Asset as GameObject;
        }
        public GameObject Clone(string moduleName, string path)
        {
            var assetRef = LoadAssetRef<GameObject>(moduleName, path);
            if (assetRef == null || assetRef.Asset == null) return null;
            var go = UnityEngine.Object.Instantiate(assetRef.Asset) as GameObject;

            if (assetRef.Children == null)
                assetRef.Children = new List<GameObject>();
            assetRef.Children.Add(go);
            return go;
        }
        public async Task<GameObject> CloneAsync(string moduleName, string path)
        {
            var assetRef = await LoadAssetRefAsync<GameObject>(moduleName, path);
            if (assetRef == null || assetRef.Asset == null) return null;
            var go = UnityEngine.Object.Instantiate(assetRef.Asset) as GameObject;

            if (assetRef.Children == null)
                assetRef.Children = new List<GameObject>();
            assetRef.Children.Add(go);
            return go;
        }

        public T CreateAsset<T>(string moduleName, string path, GameObject go) where T : UnityEngine.Object
        {
            if (typeof(T) == typeof(GameObject) || moduleName.IsNullOrEmpty() ||
                (!string.IsNullOrEmpty(path) && path.EndsWith(".prefab")))
            {
                Debug.LogError("不可以加载GameObject类型，请直接使用AssetLoader.Instance.Clone接口，path：" + path);

                return null;
            }

            if (go == null)
            {
                Debug.LogError("CreateAsset必须传递一个gameObject其将要被挂载的GameObject对象！");

                return null;
            }

            var assetRef = LoadAssetRef<T>(moduleName, path);

            if (assetRef == null || assetRef.Asset == null)
            {
                return null;
            }

            if (assetRef.Children == null)
            {
                assetRef.Children = new List<GameObject>();
            }

            assetRef.Children.Add(go);

            return assetRef.Asset as T;
        }
        public async Task<T> CreateAssetAsync<T>(string moduleName, string path, GameObject go) where T : UnityEngine.Object
        {
            if (typeof(T) == typeof(GameObject) || moduleName.IsNullOrEmpty() ||
                (!string.IsNullOrEmpty(path) && path.EndsWith(".prefab")))
            {
                Debug.LogError("不可以加载GameObject类型，请直接使用AssetLoader.Instance.Clone接口，path：" + path);

                return null;
            }

            if (go == null)
            {
                Debug.LogError("CreateAsset必须传递一个gameObject其将要被挂载的GameObject对象！");

                return null;
            }

            var assetRef = await LoadAssetRefAsync<T>(moduleName, path);

            if (assetRef == null || assetRef.Asset == null)
            {
                return null;
            }

            if (assetRef.Children == null)
            {
                assetRef.Children = new List<GameObject>();
            }

            assetRef.Children.Add(go);

            return assetRef.Asset as T;
        }
        public void Unload(Dictionary<string, Hashtable> module2Assets)
        {
            foreach (string moduleName in module2Assets.Keys)
            {
                var path2AssetRef = module2Assets[moduleName];

                if (path2AssetRef == null)
                {
                    continue;
                }

                foreach (AssetRef assetRef in path2AssetRef.Values)
                {
                    if (assetRef.Children == null || assetRef.Children.Count == 0)
                    {
                        continue;
                    }

                    for (int i = assetRef.Children.Count - 1; i >= 0; i--)
                    {
                        GameObject go = assetRef.Children[i];

                        if (go == null)
                        {
                            assetRef.Children.RemoveAt(i);
                        }
                    }

                    // 如果这个资源assetRef已经没有被任何GameObject所依赖了，那么此assetRef就可以卸载了

                    if (assetRef.Children.Count == 0)
                    {
                        assetRef.Asset = null;

                        Resources.UnloadUnusedAssets();

                        // 对于assetRef所属的这个bundle，解除关系

                        assetRef.SelfBundleRef.AssetRefs.Remove(assetRef);

                        if (assetRef.SelfBundleRef.AssetRefs.Count == 0)
                        {
                            assetRef.SelfBundleRef.SelfAssetBundle.Unload(true);
                        }

                        // 对于assetRef所依赖的那些bundle列表，解除关系

                        foreach (BundleRef bundleRef in assetRef.Dependencies)
                        {
                            bundleRef.AssetRefs.Remove(assetRef);

                            if (bundleRef.AssetRefs.Count == 0)
                            {
                                bundleRef.SelfAssetBundle.Unload(true);
                            }
                        }
                    }
                }
            }
        }
        private AssetRef LoadAssetRef<T>(string moduleName, string path) where T : UnityEngine.Object
        {
            if (moduleName.IsNullOrEmpty() || path.IsNullOrEmpty()) return null;
            if (ResConfigBase.BundleMode == false)
            {
#if UNITY_EDITOR
                var assetRef = new AssetRef(null);
                assetRef.Asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                return assetRef;
#endif
                return null;
            }
            else
            {
                var moduleExists = Base2Assets.TryGetValue(moduleName, out var hashtable);
                if (!moduleExists)
                {
                    GameLogger.LogError("未找到资源对应的模块：moduleName " + moduleName + " assetPath " + path);
                    return null;
                }

                if (!(hashtable[path] is AssetRef assetRef))
                {
                    GameLogger.LogError("未找到资源：moduleName " + moduleName + " path " + path);
                    return null;
                }

                if (assetRef.Asset != null)
                    return assetRef;
                //1.处理Bundle依赖列表
                foreach (var oneDependency in assetRef.Dependencies)
                {
                    if (oneDependency.SelfAssetBundle == null)
                    {
                        var bundlePath = GetBundlePath(moduleName, oneDependency.SelfBundleInfo.BundleName);
                        oneDependency.SelfAssetBundle = AssetBundle.LoadFromFile(bundlePath);
                    }

                    if (oneDependency.AssetRefs == null)
                        oneDependency.AssetRefs = new List<AssetRef>();
                    oneDependency.AssetRefs.Add(assetRef);
                }

                //2.处理本身Bundle
                var selfBundleRef = assetRef.SelfBundleRef;
                if (selfBundleRef.SelfAssetBundle == null)
                {
                    var bundlePath = GetBundlePath(moduleName, assetRef.SelfBundleRef.SelfBundleInfo.BundleName);
                    selfBundleRef.SelfAssetBundle = AssetBundle.LoadFromFile(bundlePath);
                }

                if (selfBundleRef.AssetRefs == null)
                    selfBundleRef.AssetRefs = new List<AssetRef>();
                selfBundleRef.AssetRefs.Add(assetRef);

                //3.处理AssetRef
                assetRef.Asset = selfBundleRef.SelfAssetBundle.LoadAsset<T>(path);
                if (typeof(T) == typeof(GameObject) && assetRef.SelfAssetInfo.AssetPath.EndsWith(".prefab"))
                    assetRef.IsGameObject = true;
                else
                    assetRef.IsGameObject = false;
                return assetRef;
            }
        }
        private async Task<AssetRef> LoadAssetRefAsync<T>(string moduleName, string path) where T : UnityEngine.Object
        {
            if (moduleName.IsNullOrEmpty() || path.IsNullOrEmpty()) return null;
            if (ResConfigBase.BundleMode == false)
            {
#if UNITY_EDITOR
                var assetRef = new AssetRef(null);
                assetRef.Asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                return assetRef;
#endif
                return null;
            }
            else
            {
                var moduleExists = Base2Assets.TryGetValue(moduleName, out var hashtable);
                if (!moduleExists)
                {
                    GameLogger.LogError("未找到资源对应的模块：moduleName " + moduleName + " assetPath " + path);
                    return null;
                }

                if (!(hashtable[path] is AssetRef assetRef))
                {
                    GameLogger.LogError("未找到资源：moduleName " + moduleName + " path " + path);
                    return null;
                }

                if (assetRef.Asset != null)
                    return assetRef;
                //1.处理Bundle依赖列表
                foreach (var oneDependency in assetRef.Dependencies)
                {
                    if (oneDependency.SelfAssetBundle == null)
                    {
                        var bundlePath = GetBundlePath(moduleName, oneDependency.SelfBundleInfo.BundleName);
                        oneDependency.SelfAssetBundle = await AssetBundle.LoadFromFileAsync(bundlePath);
                    }

                    if (oneDependency.AssetRefs == null)
                        oneDependency.AssetRefs = new List<AssetRef>();
                    oneDependency.AssetRefs.Add(assetRef);
                }

                //2.处理本身Bundle
                var selfBundleRef = assetRef.SelfBundleRef;
                if (selfBundleRef.SelfAssetBundle == null)
                {
                    var bundlePath = GetBundlePath(moduleName, assetRef.SelfBundleRef.SelfBundleInfo.BundleName);
                    selfBundleRef.SelfAssetBundle = await AssetBundle.LoadFromFileAsync(bundlePath);
                }

                if (selfBundleRef.AssetRefs == null)
                    selfBundleRef.AssetRefs = new List<AssetRef>();
                selfBundleRef.AssetRefs.Add(assetRef);

                //3.处理AssetRef
                assetRef.Asset = await selfBundleRef.SelfAssetBundle.LoadAssetAsync<T>(path);
                if (typeof(T) == typeof(GameObject) && assetRef.SelfAssetInfo.AssetPath.EndsWith(".prefab"))
                    assetRef.IsGameObject = true;
                else
                    assetRef.IsGameObject = false;
                return assetRef;
            }
        }

        private string GetBundlePath(string moduleName, string bundleName) =>
            Application.streamingAssetsPath + "/" + moduleName + "/" + bundleName;
    }
}