using UnityEngine;

public class Box : Power
{
    private bool _isHeld;

    [SerializeField] AudioClip fallClip;
    [SerializeField] AudioClip pickupClip;
    public Rigidbody2D rb;
    [SerializeField] LayerMask platformLayer;

    bool firstCollision = true;

    Animator animator => GetComponent<Animator>();

    public void Enter()
    {
        animator.SetTrigger("enter");
    }

    public bool IsHeld
    {
        get { return _isHeld; }
        set
        {
            _isHeld = value;
            animator.SetBool("held", value);
            if(value == true)
            {
                rb.simulated = false;
                SoundFxManager.Instance.PlaySoundFxClip(pickupClip, transform, 1f);
            }
            else
            {
                rb.simulated = true;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((platformLayer & (1 << collision.gameObject.layer)) != 0)
        {
            if (!firstCollision)
            {
                SoundFxManager.Instance.PlaySoundFxClip(fallClip, transform, 1f);
            }
            else
            {
                firstCollision = false;
            }
        }
    }
}
