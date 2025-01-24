using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : Singleton<AudioManager>, IMessageHandle
{
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private AudioSource _musicSource;

    [Header("Music Clips")] 
    public AudioClip _musicMainMenu;
    public AudioClip _musicGamePlay;


    [Header("SFX Clips")] 
    public AudioClip _sfxGameOver;
    public AudioClip _sfxButtonClick;
    public AudioClip _sfxEnemyDie;
    public AudioClip _sfxCollectCoin;
    public AudioClip _sfxHitEnemy;
    public AudioClip _sfxPopupShow;
    
    
    
    private List<AudioSource> _audioSourcePool;
    private AudioClip _currentMusicClip;

    private float _musicVolume = 1f;
    private float _sfxVolume = 1f;

    public float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            _musicSource.volume = value;
        }
    }

    public float SFXVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
            foreach (var source in _audioSourcePool)
            {
                source.volume = value;
            }
        }
    }

    private void Start()
    {
        PlayMusic(_musicMainMenu, true);
    }

    private void OnEnable()
    {
        _musicSource.volume = MusicVolume;
        InitializeAudioSourcePool();
        
        MessageManager.Instance.AddSubcriber(NamMessageType.OnGameLose, this);
        MessageManager.Instance.AddSubcriber(NamMessageType.OnGameStart, this);
        MessageManager.Instance.AddSubcriber(NamMessageType.OnCollectCoin, this);
        MessageManager.Instance.AddSubcriber(NamMessageType.OnEnemyDie, this);
        MessageManager.Instance.AddSubcriber(NamMessageType.OnHitEnemy, this);
        
    }

    private void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(NamMessageType.OnGameLose, this);
        MessageManager.Instance.RemoveSubcriber(NamMessageType.OnGameStart, this);
        MessageManager.Instance.RemoveSubcriber(NamMessageType.OnCollectCoin, this);
        MessageManager.Instance.RemoveSubcriber(NamMessageType.OnEnemyDie, this);
        MessageManager.Instance.RemoveSubcriber(NamMessageType.OnHitEnemy, this);
    }

    private void InitializeAudioSourcePool()
    {
        _audioSourcePool = new List<AudioSource>();
        for (int i = 0; i < _poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.volume = SFXVolume;
            _audioSourcePool.Add(source);
        }
    }

    public void PlaySfx(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        AudioSource source = GetAvailableAudioSource();
        source.volume = volume;
        source.pitch = pitch;
        source.PlayOneShot(clip);
    }
   

    public void PlayMusic(AudioClip clip, bool isLoop = true, float volume = 1f)
    {
        StartCoroutine(FadeOutAndIn(_musicSource, clip, isLoop, 1f));
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in _audioSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        // If no available source, create a new one
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.volume = SFXVolume;
        _audioSourcePool.Add(newSource);
        return newSource;
    }

    private IEnumerator FadeOutAndIn(AudioSource audioSource, AudioClip newClip, bool isLoop, float volume = 1f)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / 1f);
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.loop = isLoop;
        audioSource.volume = volume;
        audioSource.Play();

        currentTime = 0;
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, MusicVolume, currentTime / 1f);
            yield return null;
        }

        _currentMusicClip = newClip;
    }

    public AudioClip GetCurrentMusicClip()
    {
        return _currentMusicClip;
    }

    public void Handle(Message message)
    {
        // Debug.Log($"AudioManager: Handle message {message.type.ToString()}");
        switch (message.type)
        {
            //MUSIC
            case NamMessageType.OnGameStart:
                PlayMusic(_musicGamePlay, true, 6f);
                break;
            
            //SFX
            case NamMessageType.OnGameLose:
                PlaySfx(_sfxGameOver);
                break;
            case NamMessageType.OnCollectCoin:
                PlaySfx(_sfxCollectCoin);
                break;
            case NamMessageType.OnHitEnemy:
                PlaySfx(_sfxHitEnemy);
                break;
            case NamMessageType.OnEnemyDie:
                PlaySfx(_sfxEnemyDie, 0.4f);
                break;
        }
    }
}