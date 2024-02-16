//========================================================
// 描述:时间管理器
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/11 8:59:32
//========================================================

using System;
using GameFramework.Common;
using UnityEngine;
using UniRx;

namespace GameFramework.Time
{
    public class TimeManager : SingleBase<TimeManager>
    {
        public void Stop() => UnityEngine.Time.timeScale = 0f;
        public void Start() => UnityEngine.Time.timeScale = 1f;
        public void SetTimeScale(float val) => UnityEngine.Time.timeScale = val;
        public float GameRunTime => UnityEngine.Time.realtimeSinceStartup;

        public void DelayGlobal(TimeSpan timeSpan, Action action) =>
            Observable.Timer(timeSpan).Subscribe(_ => action?.Invoke());
        public void Delay(TimeSpan timeSpan, Action action, GameObject go) =>
            Observable.Timer(timeSpan).Subscribe(_ => action?.Invoke()).AddTo(go);
        public void DoTimeTaskGlobal(TimeSpan timeSpan, int count, Action action) =>
            Observable.Interval(timeSpan).Take(count).Subscribe(_ => action?.Invoke());
        public void DoTimeTaskGlobal(TimeSpan timeSpan, Action action) =>
            Observable.Interval(timeSpan).Subscribe(_ => action?.Invoke());
        public void DoTimeTask(TimeSpan timeSpan, Action action, GameObject go) =>
            Observable.Interval(timeSpan).Subscribe(_ => action?.Invoke()).AddTo(go);
        public void DoTimeTask(TimeSpan timeSpan, int count, Action action, GameObject go) =>
            Observable.Interval(timeSpan).Take(count).Subscribe(_ => action?.Invoke()).AddTo(go);
    }
}