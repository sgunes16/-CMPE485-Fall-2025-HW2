using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("Background music AudioSource.")]
    public AudioSource musicSource;

    [Tooltip("Start with music muted?")]
    public bool startMuted = false;

    private bool isMuted;

    void Awake()
    {
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }

        isMuted = startMuted;
        ApplyMuteState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMute();
        }
    }

    void ToggleMute()
    {
        isMuted = !isMuted;
        ApplyMuteState();
    }

    void ApplyMuteState()
    {
        if (musicSource != null)
        {
            musicSource.mute = isMuted;
        }
    }
}
