using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip magicShootClip;
    public AudioClip cannonShootClip;

    private float shootCooldown = 0.2f;
    private float lastMagicSoundTime = -10f;
    private float lastCannonSoundTime = -10f;

    AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMagicShoot()
    {
        if (Time.time - lastMagicSoundTime > shootCooldown)
        {
            audioSource.PlayOneShot(magicShootClip);
            lastMagicSoundTime = Time.time;
        }
    }

    public void PlayCannonShoot()
    {
        if (Time.time - lastCannonSoundTime > shootCooldown)
        {
            audioSource.PlayOneShot(cannonShootClip);
            lastCannonSoundTime = Time.time;
        }
    }
}
