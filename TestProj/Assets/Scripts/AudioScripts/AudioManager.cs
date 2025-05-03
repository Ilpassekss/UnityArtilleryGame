using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioManager : MonoBehaviour
{
    public static AudioManager AudioManagerInstance;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        // Singleton
        if (AudioManagerInstance == null)
        {
            AudioManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Не уничтожать при загрузке сцены
        DontDestroyOnLoad(gameObject);

        // Если не назначен в инспекторе — добавить
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}

