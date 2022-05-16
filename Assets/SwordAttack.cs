using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    Vector2 rightAttackOffset;
    public Collider2D swordCollder;
    public float damage = 3;

    private void Start()
    {
        rightAttackOffset = transform.position;
    }
    public void AttackRight()
    {
        swordCollder.enabled = true;
        transform.localPosition = rightAttackOffset;
    }
    public void AttackLeft()
    {
        swordCollder.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }
    public void StopAttack()
    {
        swordCollder.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
     if(other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            Debug.Log("in");
            if(enemy != null)
            {
                enemy.Health -= damage;
            }
        }
    }
}
