using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb => gameObject.GetComponent<Rigidbody2D>();
    Animator animator => GetComponent<Animator>();
    bool isGrounded = false;
    bool wasGrounded = false;

    [SerializeField] float jumpForce;
    [SerializeField] Vector2 groundCheckDimensions;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpBuffer;
    [SerializeField] float cyoteTime;
    [SerializeField] Vector3 cameraPos;
    [SerializeField] float cameraSpeed;
    [SerializeField] AudioClip[] fallClip;
    [SerializeField] AudioClip enterClip;
    [SerializeField] AudioClip exitClip;
    [SerializeField] AudioClip clickClip;
    [SerializeField] AudioClip endClip;

    float jumpTimer;
    float cyoteTimer;
    float horizontalInput;
    bool changingRooms;
    Box touchingBox = null;
    RoomBox touchingEntrance = null;
    Exit touchingExit = null;
    Lever touchingLever = null;
    Transform heldObject = null;
    bool holding = false;
    bool entering = false;
    bool exiting = false;
    bool exitEnd = false;
    int gravityScale = 1;
    float timeScale = 1;
    bool paused = false;

    public void OnJump()
    {
        jumpTimer = jumpBuffer;
    }

    public void FinishAnimation()
    {
        gravityScale = 1;
        if (entering) transform.position = touchingEntrance.EndPos.position;
        else
        {
            transform.position = touchingExit.EndPos.position + Vector3.up/2;
            exitEnd = true;
        }
        Camera.main.GetComponent<Animator>().SetBool("Fade", false);
    }

    public void OnInteract()
    {
        if (!(entering || exiting))
        {
            if (touchingLever != null)
            {
                touchingLever.PowerOn = !touchingLever.PowerOn;
                SoundFxManager.Instance.PlaySoundFxClip(touchingLever.sound, touchingLever.transform, 1f);
            }
            else
            {
                if (holding == false)
                {
                    if (touchingBox != null)
                    {
                        holding = true;
                        heldObject = touchingBox.transform;
                        touchingBox.IsHeld = true;
                    }
                }
                else
                {
                    holding = false;
                    heldObject.GetComponent<Box>().IsHeld = false;
                }
            }
        }
    }

    public void OnCrouch()
    {
        if (touchingEntrance != null)
        {
            if(!(entering||exiting))
            if (touchingEntrance.rb.linearVelocity.y > -0.1)
            {
                entering = true;
                animator.SetTrigger("entering");
                rb.linearVelocity = Vector2.zero;
                transform.position = new Vector3(touchingEntrance.transform.position.x, transform.position.y, transform.position.z);
                touchingEntrance.Enter();
                Camera.main.GetComponent<Animator>().SetBool("Fade",true);
                SoundFxManager.Instance.PlaySoundFxClip(enterClip, transform, 1f);
            }
        }
    }

    public void OnMove(InputValue vector)
    {
        horizontalInput = vector.Get<Vector2>().x;
    }

    public void OnSprint(InputValue value)
    {
        if(value.Get<float>() > 0)
        {
            if (Time.timeScale == 1) SoundFxManager.Instance.PlaySoundFxClip(clickClip,transform,1f);
            timeScale = 1.7f;
            Camera.main.GetComponent<Animator>().SetBool("Sprinting", true);
        }
        else
        {
            timeScale = 1;
            Camera.main.GetComponent<Animator>().SetBool("Sprinting", false);
        }
    }

    public void OnPause()
    {
        paused = true;
        FindAnyObjectByType<Menu>().Pause();
    }

    public void Resume()
    {
        paused = false;
        FindAnyObjectByType<Menu>().Resume();
    }

    private void Update()
    {
        if (paused) Time.timeScale = 0;
        else
        {
            Time.timeScale = timeScale;
            rb.gravityScale = (rb.linearVelocityY < 0 ? 3 : 2) * gravityScale;

            if (holding)
            {
                heldObject.position = transform.GetChild(0).position;
            }

            CheckForGround();
            if (isGrounded)
            {
                if (rb.linearVelocityY <= 0)
                {
                    cyoteTimer = cyoteTime;
                    animator.SetBool("jumping", false);
                }
            }
            else
            {
                cyoteTimer -= Time.deltaTime;
            }

            animator.SetBool("grounded", isGrounded);

            if (!(entering || exiting))
            {
                if (jumpTimer > 0 && (cyoteTimer > 0 || isGrounded))
                {
                    animator.SetBool("jumping", true);
                    rb.linearVelocity = jumpForce * Vector2.up;
                    jumpTimer = 0;
                    cyoteTimer = 0;
                }
            }
            jumpTimer -= Time.deltaTime;
        }
        Transform camform = Camera.main.transform;

        if (entering || exiting) camform.position = cameraPos;
        else
        {
            camform.position = Vector3.MoveTowards(camform.position, cameraPos, Time.deltaTime * cameraSpeed);
        }

        if (exitEnd && CheckCameraBounds())
        {
            exiting = false;
            touchingExit = null;
            entering = false;
            camform.position = cameraPos;
            exitEnd = false;
        }

        if (isGrounded && !wasGrounded)
        {
            if (rb.linearVelocityY <= 0.1f)
                SoundFxManager.Instance.PlayRandomFxClip(fallClip, transform, 0.5f);
        }
        wasGrounded = isGrounded;
    }

    private bool CheckCameraBounds()
    {
        return (transform.position.x < cameraPos.x + 10 && transform.position.x > cameraPos.x - 10 && transform.position.y < cameraPos.y + 10 && transform.position.y > cameraPos.y - 10);
    }

    private void FixedUpdate()
    {
        if (changingRooms)
        {
            rb.linearVelocity = new Vector2(MathF.Sign((cameraPos - transform.position).x) * movementSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
            animator.SetBool("walking", true);
        }
        else if (!(entering || exiting))
        {
            rb.linearVelocity = new Vector2(horizontalInput * movementSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
            animator.SetBool("walking", (Mathf.Abs(horizontalInput) > 0.1));
        }
    }

    private void CheckForGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, groundCheckDimensions, 0f, -transform.up, 0.1f, platformLayer);
        isGrounded = hit;
        try
        {
            if (hit.collider.CompareTag("Entrance"))
            {
                touchingEntrance = hit.collider.GetComponentInParent<RoomBox>();
            }
            else
            {
                touchingEntrance = null;
            }
        }
        catch (Exception e) {
            touchingEntrance = null;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, (Vector3)groundCheckDimensions);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<Animator>().SetTrigger("consume");
            Camera.main.GetComponent<Animator>().SetTrigger("End");
            SoundFxManager.Instance.PlaySoundFxClip(endClip, collision.transform, 1f);
        }
        if (collision.CompareTag("RoomEdge"))
        {
            if (!changingRooms)
            {
                cameraPos = collision.transform.parent.position;
                changingRooms = true;
            }
        }
        if (!entering)
        {
            if (collision.CompareTag("Exit"))
            {
                touchingExit = collision.GetComponent<Exit>();

                if (holding == true && touchingExit.EndPos == heldObject)
                {
                    Camera.main.GetComponent<Animator>().SetBool("Glitching", true);
                }
                else
                {
                    exiting = true;
                    animator.SetTrigger("exiting");
                    gravityScale = 0;
                    rb.linearVelocity = Vector2.zero;
                    Camera.main.GetComponent<Animator>().SetTrigger("Fade");
                    SoundFxManager.Instance.PlaySoundFxClip(exitClip, transform, 1f);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Lever"))
        {
            touchingLever = collision.GetComponentInParent<Lever>();
        }
        if (collision.CompareTag("Box"))
        {
            touchingBox = collision.GetComponentInParent<Box>();
        }
        if (collision.CompareTag("Room"))
        {
            if (!changingRooms)
            {
                cameraPos = collision.transform.position;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("RoomEdge"))
        {
            if (cameraPos != collision.transform.parent.position) changingRooms = false;
        }
        if (collision.CompareTag("Exit"))
        {
            if (entering)
            {
                entering = false;
            }
            Camera.main.GetComponent<Animator>().SetBool("Glitching", false);
        }
        if (collision.CompareTag("Box"))
        {
            touchingBox = null;
        }
        if (collision.CompareTag("Lever"))
        {
            touchingLever = null;
        }
    }
}
