using Unity.Mathematics;
using UnityEngine;

public class SoundFxManager : MonoBehaviour
{
    public static SoundFxManager Instance;
    [SerializeField] private AudioSource soundFxObject;
    [SerializeField] private AudioSource existingSource;
    float noiseTimer = 0.2f;

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
    private void Update()
    {
        noiseTimer -= Time.deltaTime;
    }

    public void PlaySoundFxClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        if (noiseTimer > 0) return;

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
