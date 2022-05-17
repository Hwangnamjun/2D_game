using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{

    Animator animator;
    private float health = 3;
    public bool isDelay;
    public GameObject[] heart;
    private Image image;
    Object[] heartSprites;
    public float Health
    {
        set
        {
            health = value;

            image = heart[(int)health].GetComponent<Image>();
            image.sprite = heartSprites[3] as Sprite;

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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isDelay = true;
        heartSprites = Resources.LoadAll("heart");
    }

    private void Update()
    {

    }

    private void Damaged()
    {
        isDelay = false;
        animator.SetTrigger("Damaged");
    }
    private void endDelay()
    {
        isDelay = true;
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
