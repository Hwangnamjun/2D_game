using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Animator animator;
    Rigidbody2D rb;
    private float moveSpeed = 0.3f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    CapsuleCollider2D cc;
    bool isDelay = false;
    Vector2 vec;
    SpriteRenderer spriteRenderer;
    private int damage;
    public float Health
    {
        set 
        {
            health = value;

            if (health <= 0)
            {
                Defeated();
            }
            else
            {
                Damaged();
            }
        }
        get 
        {
            return health;
        }
    }

    public float health;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        damage = 1;
    }

    private void FixedUpdate()
    {
        if(!isDelay)
        {
            isDelay = true;
            StartCoroutine(monsterMove());
        }
        TryMove(vec);
    }

    IEnumerator monsterMove()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        vec = new Vector2(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2));
        isDelay = false;
    }

    private void TryMove(Vector2 direction)
    {
        if(isDelay)
        {
            if (direction != Vector2.zero)
            {
                int count = rb.Cast(
                     direction,
                     movementFilter,
                     castCollisions,
                     moveSpeed * Time.fixedDeltaTime + collisionOffset);

                if (count == 0)
                {
                    rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

                    animator.SetBool("isMoving", true);
                    if (direction.x < 0)
                    {
                        spriteRenderer.flipX = true;
                    }
                    else if (direction.x > 0)
                    {
                        spriteRenderer.flipX = false;
                    }
                }
                else
                {
                    animator.SetBool("isMoving", false);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();

            if (player.isDelay == true)
            {
                player.Health -= damage;
            }
        }
    }

    private void Damaged()
    {
        animator.SetTrigger("Damaged");
    }

    private void Defeated()
    {
        animator.SetTrigger("Defeated");
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }

}
