using UnityEngine;

public class Box : Power
{
    private bool _isHeld;

    public Rigidbody2D rb;

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
            }
            else
            {
                rb.simulated = true;
            }
        }
    }
}
