using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rb => gameObject.GetComponent<Rigidbody2D>();
    Animator animator => GetComponent<Animator>();
    bool isGrounded = false;

    [SerializeField] float jumpForce;
    [SerializeField] Vector2 groundCheckDimensions;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] float movementSpeed;

    float horizontalInput;

    public void OnJump()
    {
        if(isGrounded)
        rb.linearVelocity = jumpForce * Vector2.up;
    }

    public void OnMove(InputValue vector)
    {
        horizontalInput = vector.Get<Vector2>().x;
    }

    private void Update()
    {
        CheckForGround();
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * movementSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
        animator.SetBool("walking", (Mathf.Abs(horizontalInput) > 0.1));
    }

    private void CheckForGround()
    {
        isGrounded = Physics2D.BoxCast(transform.position, groundCheckDimensions, 0f, -transform.up, 0.1f, platformLayer);
        animator.SetBool("grounded", isGrounded);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, (Vector3)groundCheckDimensions);
    }
}
