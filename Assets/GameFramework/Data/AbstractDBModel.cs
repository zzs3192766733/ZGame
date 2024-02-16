//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/23 14:05:40
//========================================================

using System.Collections.Generic;
using System.IO;
using GameFramework.Common;
using UnityEngine;

namespace GameFramework.Data
{
    public abstract class AbstractDBModel<T, TP>
        where T : class, new()
        where TP : AbstractEntity
    {
        private static readonly object ObjLock = new object();
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (ObjLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }

                return _instance;
            }
        }

        protected readonly List<TP> MList;
        protected readonly Dictionary<int, TP> MDic;
        protected abstract string FileName { get; }

        protected AbstractDBModel()
        {
            MList = new List<TP>();
            MDic = new Dictionary<int, TP>();
            LoadData();
        }

        private void LoadData()
        {
#if UNITY_EDITOR
            var path = Application.dataPath + "/GameFramework/Data/CreateData/" + FileName;
#else
            var path = Application.streamingAssetsPath + "/CreateData/" + FileName;
#endif
            using (var gp = new GameDataTableParser(path))
            {
                while (!gp.Eof)
                {
                    var entity = MakeData(gp);
                    MList.Add(entity);
                    MDic.Add(entity.ID, entity);
                    gp.Next();
                }
            }
        }

        protected abstract TP MakeData(GameDataTableParser parser);
        public List<TP> GetList() => MList;
        public TP Get(int id) => MDic.ContainsKey(id) ? MDic[id] : null;
    }
}