using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Lever : Power
{
    public Power target;
    Animator animator => GetComponent<Animator>();
    public AudioClip sound;

    private void Update()
    {
        animator.SetBool("power", PowerOn);
        target.PowerOn = PowerOn;
    }
}
