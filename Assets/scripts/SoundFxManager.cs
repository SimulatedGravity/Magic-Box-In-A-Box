using Unity.Mathematics;
using UnityEngine;

public class SoundFxManager : MonoBehaviour
{
    public static SoundFxManager Instance;
    [SerializeField] private AudioSource soundFxObject;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            if(Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void PlaySoundFxClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFxObject, spawnTransform.position, quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject,clipLength);
    }
    public void PlayRandomFxClip(AudioClip[] audioClips, Transform spawnTransform, float volume)
    {
        int rand = UnityEngine.Random.Range(0, audioClips.Length);
        AudioSource audioSource = Instantiate(soundFxObject, spawnTransform.position, quaternion.identity);

        audioSource.clip = audioClips[rand];
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
