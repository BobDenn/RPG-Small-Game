using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool bgmPlaying;
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
        if(!bgmPlaying)
            StopBGM();
        else
            if(!bgm[_bgmIndex].isPlaying)
                PlayRandomBGM();
    }

    public void PlaySFx(int sfxIndex, Transform source)
    {
        if(sfx[sfxIndex].isPlaying)
            return;
        
        if(source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, source.position) > sfxMinDistance)
            return;
            
        if (sfxIndex < sfx.Length)
        {
            sfx[sfxIndex].pitch = Random.Range(0.8f, 1.1f);
            sfx[sfxIndex].Play();
        }
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
