using UnityEngine;

public class Door : Power
{
    bool wasOpen = false;
    [SerializeField] AudioClip openClip;
    [SerializeField] AudioClip closeClip;
    Animator animator => GetComponent<Animator>();
    // Update is called once per frame
    void Update()
    {
        animator.SetBool("open", PowerOn);

        if(PowerOn != wasOpen)
        {
            wasOpen = PowerOn;
            if(PowerOn)
            {
                SoundFxManager.Instance.PlaySoundFxClip(openClip, transform, 1f);
            }
            else
            {
                SoundFxManager.Instance.PlaySoundFxClip(closeClip, transform, 1f);
            }
        }
    }
}
