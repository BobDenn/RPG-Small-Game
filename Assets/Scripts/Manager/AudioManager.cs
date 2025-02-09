using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    private bool _isPlaying;
    private int _bgmIndex;
    
    private void Awake()
    {
        if(instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    private void Update()
    {
        if(!_isPlaying)
            StopBGM();
        else
            if(!bgm[_bgmIndex].isPlaying)
                PlayBgm(_bgmIndex);
    }

    public void PlaySFx(int sfxIndex)
    {
        if(sfxIndex < sfx.Length)
            sfx[sfxIndex].Play();
    }
    
    public void StopSFx(int sfxIndex) => sfx[sfxIndex].Stop();

    public void PlayRandomBGM()
    {
        _bgmIndex = Random.Range(0, bgm.Length);
        PlayBgm(_bgmIndex);
    }
    
    public void PlayBgm(int bgmIndex)
    {
        _bgmIndex = bgmIndex;
        StopBGM();
        bgm[_bgmIndex].Play();
    }
    
    public void StopBGM()
    {
        foreach (var i in bgm)
        {
            i.Stop();
        }
    }
}
