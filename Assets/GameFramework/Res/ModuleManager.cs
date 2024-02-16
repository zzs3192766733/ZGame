//========================================================
// 描述:资源模块管理器
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/24 8:24:21
//========================================================

using System;
using System.Threading.Tasks;
using GameFramework.Common;

namespace GameFramework.Res
{
    public class ModuleManager : SingleBase<ModuleManager>
    {
        public async Task<bool> Load(ModuleConfig moduleConfig)
        {
            if (ResConfigBase.BundleMode == false)
            {
                return true;
            }
            else
            {
                var moduleAbConfig = await AssetLoader.Instance.LoadAssetBundleConfig(moduleConfig.ModuleName);
                if (moduleAbConfig == null) return false;
                GameLogger.Log($"模块包含AB包数量:{moduleAbConfig.BundleArray.Count}");

                var hashTableAsset = AssetLoader.Instance.ConfigAssembly(moduleAbConfig);
                AssetLoader.Instance.Base2Assets.Add(moduleConfig.ModuleName,hashTableAsset);
                return true;
            }
        }
    }
}