using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    private bool canPlaySFX;
    
    private void Awake()
    {
        if(instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
        
        Invoke("AllowSFX", 1f);
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
        /*if(sfx[sfxIndex].isPlaying)
            return;*/
        if(!canPlaySFX)
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

    public void StopSfxWithTime(int index) => StartCoroutine(DecreaseVoluem(sfx[index]));
    
    private IEnumerator DecreaseVoluem(AudioSource audio)
    {
        float defaultVolume = audio.volume;
        while (audio.volume > .1f)
        {
            audio.volume -= audio.volume * .2f;
            yield return new WaitForSeconds(0.2f);

            if (audio.volume <= .1f)
            {
                audio.Stop();
                audio.volume = defaultVolume;
                break;
            }
        }
    }
    
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

    private void AllowSFX() => canPlaySFX = true;
}
