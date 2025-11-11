using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;

    public AudioClip gameOverClip; // Canción que se reproducirá al finalizar la partida

    void Awake()
    {
        // Singleton: Evita duplicados de música
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void PlayBackgroundMusic()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Método para cambiar la música al terminar la partida
    public void PlayGameOverMusic()
    {
        if (gameOverClip != null)
        {
            audioSource.Stop(); // Detiene la música actual
            audioSource.clip = gameOverClip; // Asigna la nueva canción
            audioSource.loop = false; // No repetir (opcional)
            audioSource.Play(); // Reproduce la música de Game Over
        }
    }
}
