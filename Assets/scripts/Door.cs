using UnityEngine;

public class Door : Power
{
    Animator animator => GetComponent<Animator>();
    // Update is called once per frame
    void Update()
    {
        animator.SetBool("open", PowerOn);
    }
}
