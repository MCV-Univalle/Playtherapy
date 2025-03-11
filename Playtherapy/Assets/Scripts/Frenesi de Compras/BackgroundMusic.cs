using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;

    void Awake()
    {
        // Singleton: Evita que la música se duplique al cambiar de escena
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

        // Obtiene el componente AudioSource y reproduce la música
        audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Método opcional para detener la música cuando termine la partida
    public void StopMusic()
    {
        audioSource.Stop();
    }
}
