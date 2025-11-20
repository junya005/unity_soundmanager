using System;
using System.Collections.Generic;
using UnityEngine;

namespace Junya005.AudioSystem
{
    [DefaultExecutionOrder(-1000)]
    public class SoundManager : MonoBehaviour
    {
        #region Singleton

        private static SoundManager instance;
        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (SoundManager)FindObjectOfType(typeof(SoundManager));
                    if (instance == null)
                    {
                        SetupInstance();
                    }
                }
                return instance;
            }
        }

        private static void SetupInstance()
        {
            instance = (SoundManager)FindObjectOfType(typeof(SoundManager));
            if (instance == null)
            {
                GameObject gameObj = new GameObject();
                gameObj.name = typeof(SoundManager).Name;
                instance = gameObj.AddComponent<SoundManager>();
                DontDestroyOnLoad(gameObj);
            }
        }

        private void RemoveDuplicates()
        {
            if (instance == null)
            {
                instance = this as SoundManager;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region AudioLogic

        enum ESoundType
        {
            BGM,
            SE,
        }

        private const string PATH_AUDIO_LIST = "AudioData";

        private SOAudioData _audioList;

        private Dictionary<string, AudioClip> _bgmAudioClips = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip> _seAudioClips = new Dictionary<string, AudioClip>();

        private List<AudioSource> _bgmAudioSources = new List<AudioSource>();
        private List<AudioSource> _seAudioSources = new List<AudioSource>();

        private bool _isInitialized = false;

        private void InitializeAudio()
        {
            if (_isInitialized) return;

            _audioList = Resources.Load<SOAudioData>(PATH_AUDIO_LIST);

            if (_audioList == null)
            {
                Debug.LogWarning("AudioDataが設定されていません");
                return;
            }

            if (_audioList.BGMList.Count > 0)
            {
                foreach (var clip in _audioList.BGMList)
                {
                    _bgmAudioClips.Add(clip.name, clip);
                }
            }

            if (_audioList.SEList.Count > 0)
            {
                foreach (var clip in _audioList.SEList)
                {
                    _seAudioClips.Add(clip.name, clip);
                }
            }

            _isInitialized = true;
        }

        /// <summary>BGMを再生する</summary>
        /// <param name="name">clip file name</param>
        public void PlayBGM(string name)
        {
            StopBGM();
            Play(name, ESoundType.BGM);
        }

        /// <summary>SEを再生する</summary>
        /// <param name="name">clip file name</param>
        public void PlaySE(string name)
        {
            Play(name, ESoundType.SE);
        }

        /// <summary>音源再生用の内部関数</summary>
        /// <param name="name">再生するクリップの名前</param>
        /// <param name="soundType">音源のタイプ</param>
        private void Play(string name, ESoundType soundType)
        {
            // 対応する音源タイプの配列を格納する変数を定義
            Dictionary<string, AudioClip> audioClips = null;
            List<AudioSource> audioSources = null;

            // 対応する音源タイプの配列を定義した変数に格納
            switch (soundType)
            {
                case ESoundType.BGM:
                    audioClips = _bgmAudioClips;
                    audioSources = _bgmAudioSources;
                    break;
                case ESoundType.SE:
                    audioClips = _seAudioClips;
                    audioSources = _seAudioSources;
                    break;
            }

            // 指定音源があるかを調べて再生
            if (audioClips.TryGetValue(name, out var clip))
            {
                // 空いているAudioSourceを検索して再生
                foreach (var audioSource in audioSources)
                {
                    if (audioSource.isPlaying == false)
                    {
                        if (soundType == ESoundType.BGM)
                            audioSource.loop = true;
                        audioSource.clip = clip;
                        audioSource.Play();
                        return;
                    }
                }

                /*
                空いているAudioSourceがなければ新しく作成して追加(ObjectPoolパターン)
                BGMもObjectPoolの対象だが、PlayBGMはStopBGMを実行してからなのと、
                フェード処理実装の際に取り回しがいいのでこのまま
                */
                AudioSource newAudioSource = this.gameObject.AddComponent<AudioSource>();
                if (soundType == ESoundType.BGM)
                    newAudioSource.loop = true;
                newAudioSource.clip = clip;
                newAudioSource.Play();

                // 新しく生成したAudioSourceを対応する配列に格納
                switch (soundType)
                {
                    case ESoundType.BGM:
                        _bgmAudioSources.Add(newAudioSource);
                        break;
                    case ESoundType.SE:
                        _seAudioSources.Add(newAudioSource);
                        break;
                }
            }
            else
            {
                // 音源がなければ警告
                Debug.LogWarning("指定された音源は存在しませんでした");
            }
        }

        /// <summary>BGMを停止する</summary>
        public void StopBGM()
        {
            foreach (var audioSource in _bgmAudioSources)
            {
                audioSource.Stop();
            }
        }

        /// <summary>BGMのボリュームを変更</summary>
        /// <param name="volume">ボリューム値</param>
        public void SetBgmVolume(float volume)
        {
            foreach (var audioSource in _bgmAudioSources)
            {
                audioSource.volume = volume;
            }
        }

        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            new GameObject("SoundManager", typeof(SoundManager));
            SetupInstance();
        }

        private void Awake()
        {
            RemoveDuplicates();
        }

        private void Start()
        {
            InitializeAudio();
        }
    }
}
