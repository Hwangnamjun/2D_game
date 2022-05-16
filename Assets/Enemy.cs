using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Animator animator;
    Rigidbody2D rb;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    CapsuleCollider2D cc;
    bool isDelay = false;

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
    }

    private void FixedUpdate()
    {
        if(!isDelay)
        {
            isDelay = true;
            StartCoroutine(monsterMove());
        }
    }

    IEnumerator monsterMove()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        TryMove(new Vector2(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2)));
        isDelay = false;
    }

    private bool TryMove(Vector2 direction)
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
        else
        {
            return false;
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
