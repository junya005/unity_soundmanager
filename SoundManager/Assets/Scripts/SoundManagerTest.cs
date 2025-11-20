using System.Collections;
using System.Collections.Generic;
using Junya005.AudioSystem;
using UnityEngine;

public class SoundManagerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM("lofi_0");
    }

    public void OnClicked()
    {
        SoundManager.Instance.PlaySE("se_next");
    }
}
