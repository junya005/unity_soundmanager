using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junya005.AudioSystem
{
    /// <summary>音源設定用のデータアセットクラス</summary>
    [CreateAssetMenu(fileName = "AudioData", menuName = "Plugins/Junya005/AudioData")]
    public class SOAudioData : ScriptableObject
    {
        [Header("Set BGM AudioClip Here")]
        public List<AudioClip> BGMList;

        [Header("Set SE AudioClip Here")]
        public List<AudioClip> SEList;
    }
}
