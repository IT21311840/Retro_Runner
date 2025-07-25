using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private Sound[] effectSounds;
    [SerializeField] private Sound[] bgSounds;

    [SerializeField] private bool updateBgMusicOnSceneChange = false;

    private int currBgMusicIndex = -1;

    private float mVol;
    private float eVol;

    public bool isMuted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: keep alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        eVol = PlayerPrefs.GetFloat("EffectsVolume", 1f);

        createAudioSources(effectSounds, eVol);
        createAudioSources(bgSounds, mVol);

        if (updateBgMusicOnSceneChange)
            SceneManager.activeSceneChanged += ChangeBgMusicOnSceneChange;

        PlayMusic(true);
        SetMuteState();
    }

    void Update()
    {
        if (currBgMusicIndex != -1 && !bgSounds[currBgMusicIndex].source.isPlaying)
        {
            PlayMusic(true);
        }
    }

    private void ChangeBgMusicOnSceneChange(Scene curr, Scene next)
    {
        PlayMusic(true);
    }

    private void createAudioSources(Sound[] sounds, float volume)
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlaySound(AudioType audioName)
    {
        Sound s = Array.Find(effectSounds, sound => sound.name == audioName);

        if (s == null)
        {
            Debug.LogWarning("Unable to play sound " + audioName);
            return;
        }

        s.source.Play();
    }

    public void StopSound(AudioType audioName)
    {
        Sound s = Array.Find(effectSounds, sound => sound.name == audioName);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + audioName + " not found");
            return;
        }

        s.source.Stop();
    }

    public void PlayMusic(bool random, AudioType audioName = AudioType.None)
    {
        if (bgSounds.Length == 0) return;

        if (currBgMusicIndex != -1)
        {
            StopMusic();
        }

        if (random || audioName == AudioType.None)
        {
            currBgMusicIndex = GetRandomBgMusicIndex();
        }
        else
        {
            currBgMusicIndex = Array.FindIndex(bgSounds, sound => sound.name == audioName);
            if (currBgMusicIndex < 0)
                currBgMusicIndex = GetRandomBgMusicIndex();
        }

        bgSounds[currBgMusicIndex].source.Play();
    }

    public void StopMusic()
    {
        bgSounds[currBgMusicIndex].source.Stop();
        currBgMusicIndex = -1;
    }

    private int GetRandomBgMusicIndex()
    {
        return UnityEngine.Random.Range(0, bgSounds.Length);
    }

    public AudioType getCurrBgSongName()
    {
        return bgSounds[currBgMusicIndex].name;
    }

    public void musicVolumeChanged(float val)
    {
        mVol = val;
        PlayerPrefs.SetFloat("MusicVolume", mVol);

        foreach (Sound m in bgSounds)
        {
            m.source.volume = m.volume * mVol;
        }
    }

    public void effectVolumeChanged(float val)
    {
        eVol = val;
        PlayerPrefs.SetFloat("EffectsVolume", eVol);

        foreach (Sound s in effectSounds)
        {
            s.source.volume = s.volume * eVol;
        }
    }

    private void SetMuteState()
    {
        isMuted = PlayerPrefs.GetInt("Muted") == 1;
        SetAudioListener();
    }

    public void ToggleMuteState()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        SetAudioListener();
    }

    private void SetAudioListener()
    {
        AudioListener.pause = isMuted;
    }
}
