//========================================================
// 描述:音效管理器
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2021/12/11 9:47:13
//========================================================

using System;
using System.Collections.Generic;
using GameFramework.Common;
using GameFramework.ObjectPool;
using UnityEngine;

namespace GameFramework.Audio
{
    public class AudioManager : SingleMonoBase<AudioManager>
    {
        private static float _backGroundMusic;
        public static float BackgroundMusic
        {
            set
            {
                _backGroundMusic = value;
                BackGroundAudioSource.volume = value;
            }
            get => _backGroundMusic;
        }

        public static float ButtonMusic;
        public static float GameMusic;

        private PoolObject _audioSourcePoolObject;

        private PoolObject AudioSourcePoolObject
        {
            get
            {
                if (_audioSourcePoolObject == null)
                {
                    _audioSourcePoolObject =
                        PoolManager.Instance.CreateObjectPool("Audio", Resources.Load<GameObject>("AudioSource"));
                }

                return _audioSourcePoolObject;
            }
        }

        private static AudioSource _backGroundAudioSource;

        private static AudioSource BackGroundAudioSource
        {
            get
            {
                if (_backGroundAudioSource == null)
                {
                    var backGroundObj = new GameObject("BackGroundAudioSource");
                    backGroundObj.transform.SetParent(AudioManager.Instance.transform);
                    _backGroundAudioSource = backGroundObj.AddComponent<AudioSource>();
                }

                return _backGroundAudioSource;
            }
        }

        private readonly Dictionary<string, int> _allAudioDic = new Dictionary<string, int>();


        public void PlayClip(AudioClip clip, bool isGameClip = true)
        {
            var isHave = false;
            if (!_allAudioDic.ContainsKey(clip.name))
            {
                //如果不存在
                isHave = false;
            }
            else
            {
                isHave = true;
                //已经存在过
                if (_allAudioDic[clip.name] > 5)
                {
                    //如果播放的声音数量大于5个，那么为了节省开销，不再进行播放
                    return;
                }
            }
            var audioSource = AudioSourcePoolObject.GetObject().GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = isGameClip ? GameMusic : ButtonMusic;
            audioSource.Play();
            if (isHave)
                _allAudioDic[clip.name]++;
            else
                _allAudioDic.Add(clip.name, 1);
            
            Time.TimeManager.Instance.Delay(TimeSpan.FromSeconds(clip.length),
                () =>
                {
                    AudioSourcePoolObject.SetObject(audioSource.gameObject);
                    _allAudioDic[clip.name]--;
                    if (_allAudioDic[clip.name] <= 0)
                        _allAudioDic.Remove(clip.name);
                }, AudioSourcePoolObject.gameObject);
        }

        public void PlayBackGround(AudioClip clip)
        {
            BackGroundAudioSource.volume = BackgroundMusic;
            BackGroundAudioSource.clip = clip;
            BackGroundAudioSource.loop = true;
            BackGroundAudioSource.Play();
        }
    }
}