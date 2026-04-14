using System;
using UnityEngine;
using UnityEngine.UI;

public enum SoundID
{
    ITEMEXP,
    ITEMLIFE,
    ITEMSPECIAL,
    JUMP,
    RUN,
    INTERACTNPC,
    CRAFTING,
    ITEMCRAFTING
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SFX")]
    [SerializeField] private AudioClip[] m_sfxClips; // Taille 8, assigné dans l'Inspector
    private AudioSource m_sfxSource;
    private float m_sfxVolume;

    [Header("Soundtrack")]
    [SerializeField] private AudioClip m_musicClip;
    private AudioSource m_musicSource;
    private float m_musicVolume;

    [Header("Volume UI (ESC)")]
    [SerializeField] private GameObject m_volumeUI;
    [SerializeField] private Slider m_soundtrackSlider;
    [SerializeField] private Slider m_sfxSlider;

    private bool onPause;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
        m_sfxVolume = 1f;
        m_musicVolume = 0.5f;
        onPause = false;

        InitSources();
        InitUI();
    }

    private void InitSources()
    {
        m_sfxSource = gameObject.AddComponent<AudioSource>();
        m_musicSource = gameObject.AddComponent<AudioSource>();

        m_sfxSource.volume = m_sfxVolume;
        m_musicSource.volume = m_musicVolume;
        m_musicSource.loop = true;

        PlayMusic();
    }

    //Soundtrack
    private void PlayMusic()
    {
        if (m_musicClip == null) return;
        m_musicSource.clip = m_musicClip;
        m_musicSource.Play();
    }

    //Play Audio
    public void PlayAudio(SoundID id)
    {
        int index = (int)id;

        if (id == SoundID.CRAFTING)
        {
            if (m_sfxSource.isPlaying && m_sfxSource.clip == m_sfxClips[index]) return;

            m_sfxSource.clip = m_sfxClips[index];
            m_sfxSource.loop = true;
            m_sfxSource.Play();
        }
        else
        {
            m_sfxSource.loop = false;
            m_sfxSource.PlayOneShot(m_sfxClips[index], m_sfxVolume);
        }
    }

    public void StopAudio(SoundID id)
    {
        int index = (int)id;
        if (m_sfxSource.clip == m_sfxClips[index])
            m_sfxSource.Stop();
    }

    //UI
    private void InitUI()
    {
        m_volumeUI.SetActive(false);
        m_sfxSlider.value = m_sfxVolume;
        m_soundtrackSlider.value = m_musicVolume;

        m_sfxSlider?.onValueChanged.AddListener(SetSFXVolume);
        m_soundtrackSlider?.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        m_sfxVolume = Mathf.Clamp01(volume);
        m_sfxSource.volume = m_sfxVolume;
    }

    //Volume
    private void SetMusicVolume(float volume)
    {
        m_musicVolume = Mathf.Clamp01(volume);
        m_musicSource.volume = m_musicVolume;
    }

    public void ToggleVolumeUI()
    {
        if (onPause)
        {
            onPause = false;
        } else
        {
            onPause = true;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_volumeUI.SetActive(onPause);
    }
}
