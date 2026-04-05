using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // ── SINGLETON ────────────────────────────────────────────────
    public static AudioManager Instance;

    // ── SOUND CLIPS ──────────────────────────────────────────────
    [Header("Player")]
    [SerializeField] private AudioClip footstepsClip;

    [Header("Loot")]
    [SerializeField] private AudioClip boxOpenClip;       // Plays when minigame starts
    [SerializeField] private AudioClip lootCollectedClip; // RE4 pickup on success
    [SerializeField] private AudioClip lootFailClip;      // MGS door buzz on failure

    [Header("Guards")]
    [SerializeField] private AudioClip guardAlertClip;    // MGS alert when spotted
    [SerializeField] private AudioClip missionFailedClip; // COD mission failed voice

    [Header("Extraction")]
    [SerializeField] private AudioClip missionCompleteClip; // GTA SA mission complete

    [Header("Timer")]
    [SerializeField] private AudioClip timerWarningClip;  // 24 countdown loops at 10s

    // ── AUDIO SOURCES ────────────────────────────────────────────
    private AudioSource sfxSource;          // One-shot sounds
    private AudioSource footstepSource;     // Looping footsteps
    private AudioSource timerWarningSource; // Looping timer warning

    // ── PRIVATE STATE ────────────────────────────────────────────
    private bool timerWarningPlayed = false;

    // ── UNITY METHODS ────────────────────────────────────────────

    void Awake()
    {
        // Set up singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // General one-shot audio source
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        // Dedicated looping source for footsteps
        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.playOnAwake = false;
        footstepSource.loop = true;
        footstepSource.clip = footstepsClip;
        footstepSource.volume = 0.4f;

        // Dedicated looping source for timer warning
        timerWarningSource = gameObject.AddComponent<AudioSource>();
        timerWarningSource.playOnAwake = false;
        timerWarningSource.loop = true;
        timerWarningSource.clip = timerWarningClip;
        timerWarningSource.volume = 0.8f;
    }

    // ── PUBLIC METHODS ───────────────────────────────────────────

    // Called every frame from PlayerController — plays footsteps while moving
    public void SetFootsteps(bool isMoving)
    {
        if (isMoving && !footstepSource.isPlaying)
            footstepSource.Play();
        else if (!isMoving && footstepSource.isPlaying)
            footstepSource.Stop();
    }

    // Play box opening sound when minigame starts
    public void PlayBoxOpen()
    {
        PlaySFX(boxOpenClip);
    }

    // Play RE4 pickup sound when loot successfully collected
    public void PlayLootCollected()
    {
        PlaySFX(lootCollectedClip);
    }

    // Play MGS door buzz when crate gets locked
    public void PlayLootFail()
    {
        PlaySFX(lootFailClip);
    }

    // Play MGS alert when player is spotted by guard
    public void PlayGuardAlert()
    {
        PlaySFX(guardAlertClip);
    }

    // Play COD mission failed voice after a short delay
    // so MGS alert plays first without overlapping
    public void PlayMissionFailed()
    {
        StartCoroutine(PlayDelayed(missionFailedClip, 1.5f));
    }

    // Play GTA SA mission complete on successful extraction
    public void PlayMissionComplete()
    {
        PlaySFX(missionCompleteClip);
    }

    // Start looping the 24 countdown — only triggers once at 10 seconds
    public void PlayTimerWarning()
    {
        if (!timerWarningPlayed)
        {
            timerWarningPlayed = true;
            timerWarningSource.Play();
        }
    }

    // Stop the timer warning loop — called when game ends
    public void StopTimerWarning()
    {
        timerWarningSource.Stop();
        timerWarningPlayed = false;
    }

    // ── PRIVATE METHODS ──────────────────────────────────────────

    // Generic one-shot sound player with null check
    private void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    // Plays a clip after a delay — used for COD voice after MGS alert
    private IEnumerator PlayDelayed(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySFX(clip);
    }
}