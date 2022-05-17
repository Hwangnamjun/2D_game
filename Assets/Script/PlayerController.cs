using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;
    SpriteRenderer spriteRenderer;
    bool canMove = true;
    ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ps = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if(canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));

                    if (!success)
                    {
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }
                animator.SetBool("isMoving", success);
                if(!ps.isPlaying)
                {
                    ps.Play();
                }
            }
            else
            {
                ps.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
                animator.SetBool("isMoving", false);
            }

            var shape = ps.shape;
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
                shape.position = new Vector3(0.2f,shape.position.y,shape.position.z);
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
                shape.position = new Vector3(-0.2f, shape.position.y, shape.position.z);
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {

        if(direction != Vector2.zero)
        {
            int count = rb.Cast(
                 direction,
                 movementFilter,
                 castCollisions,
                 moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {

            return false;
        }

    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack()
    {
        LockMovement();

        if(spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Chest")
        {
            Debug.Log("123");
            other.GetComponent<Animator>().SetTrigger("IsOpen");
        }
    }
    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
}
