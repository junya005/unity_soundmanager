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

        [SerializeField]
        private SOAudioData _audioList;

        private Dictionary<string, AudioSource> _bgmAudioSources = new Dictionary<string, AudioSource>();
        private Dictionary<string, AudioSource> _seAudioSources = new Dictionary<string, AudioSource>();

        private bool _isInitialized = false;

        private void InitializeAudio()
        {
            if (_isInitialized) return;

            if (_audioList.BGMList.Count > 0)
            {
                foreach (var clip in _audioList.BGMList)
                {
                    var audioSource = this.gameObject.AddComponent<AudioSource>();
                    audioSource.clip = clip;
                    audioSource.playOnAwake = false;
                    audioSource.loop = true;
                    _bgmAudioSources.Add(audioSource.clip.name, audioSource);
                }
            }

            if (_audioList.SEList.Count > 0)
            {
                foreach (var clip in _audioList.SEList)
                {
                    var audioSource = this.gameObject.AddComponent<AudioSource>();
                    audioSource.clip = clip;
                    audioSource.playOnAwake = false;
                    audioSource.loop = false;
                    _seAudioSources.Add(audioSource.clip.name, audioSource);
                }
            }

            _isInitialized = true;
        }

        public void PlayBGM(string name)
        {
            foreach (var audioSource in _bgmAudioSources)
            {
                if (_bgmAudioSources[audioSource.Key].isPlaying) return;
            }
            _bgmAudioSources[name].Play();
        }

        public void PlaySE(string name)
        {
            _seAudioSources[name].Play();
        }

        public void StopBGM()
        {
            foreach (var audioSource in _bgmAudioSources)
            {
                _bgmAudioSources[audioSource.Key].Stop();
            }
        }

        public void SetBgmVolume(float volume)
        {
            foreach (var audioSource in _bgmAudioSources)
            {
                audioSource.Value.volume = volume;
            }
        }

        public void SetBgmVolume(float volume, string bgmName)
        {
            _bgmAudioSources[bgmName].volume = volume;
        }

        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
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
