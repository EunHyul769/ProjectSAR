using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;
    
    
    [Header("Player")]
    public AudioClip eatFood;
    public AudioClip levelUp;
    public AudioClip meleeSwing;
    public AudioClip magicShot;
    public AudioClip fireMagic;
    public AudioClip expGain;  

    [Header("Monster")]
    public AudioClip enemyLongAttack;
    public AudioClip expDrop;

    [Header("BGM")]
    public AudioClip moonlightForest;

    public AudioSource sfxSource;
    public AudioSource bgmSource;

    private void Awake()
    {
        if (I == null) I = this;
        else Destroy(gameObject);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }
}
