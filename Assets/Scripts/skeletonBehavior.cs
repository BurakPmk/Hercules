using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonBehavior : MonoBehaviour
{
    private Animator s_Animator;
    public GameObject sAttack;
    public GameObject hercules;
    private Rigidbody2D rb;
    public LayerMask herculesLayer;
    public float attackRange = 0.5f;
    public float attackTime = 0;
    public float moveSpeed;
    private int health = 50;
    private bool walk = true;
    Vector3 fark;
    Vector2 direction;
    Vector2 movement;
    //float f = 0.50f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        s_Animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        
        fark = transform.position - hercules.transform.position;
        direction = new Vector2(hercules.transform.position.x - transform.position.x,  transform.position.y);
        direction.Normalize();
        movement = direction;
        if (hercules.transform.position.x < transform.position.x)
            transform.localScale = new Vector3(-3f, 3f, 1f);
        else if (hercules.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(3f, 3, 1f);

        if(health <= 0)
        {
            s_Animator.SetTrigger("dead");
            StartCoroutine(destroyAfterDeath());
        }
        
        if (fark.x > 0.5 && fark.x <= 10)
        {
            
            moveCharachter(movement);
            s_Animator.SetBool("walk", walk);
            if (fark.x <= 1)
            {
                s_Animator.SetTrigger("attack");
                if (s_Animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && Time.time > attackTime)
                {
                    attackTime = Time.time + 1;
                    s_Animator.SetTrigger("attack");
                    /*sAttack.SetActive(true);
                    StartCoroutine(sAttackDisable());*/
                    Attack();
                }


            }

        }
        else if (fark.x > -10 && fark.x <= -0.5)
        {
            moveCharachter(movement);
            s_Animator.SetBool("walk", walk);
            if (fark.x <= 1)
            {
                s_Animator.SetTrigger("attack");
                if (s_Animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && Time.time > attackTime)
                {
                    attackTime = Time.time + 1;
                    s_Animator.SetTrigger("attack");
                    /*sAttack.SetActive(true);
                    StartCoroutine(sAttackDisable());*/
                    Attack();
                }


            }


        }
        /*else if(fark.x<=1)
        {
            s_Animator.SetTrigger("attack");
            if (s_Animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && Time.time > attackTime)
            {
                attackTime = Time.time + 1;
                s_Animator.SetTrigger("attack");
                sAttack.SetActive(true);
                StartCoroutine(sAttackDisable());
            }
            
           
        }*/
        else
        {
            s_Animator.SetBool("walk", false);
        }
        
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "attack")
        {
            s_Animator.SetTrigger("hit");
            health = health - 10;
        }
    }*/
    void Attack()
    {
        Collider2D[] hitHercules = Physics2D.OverlapCircleAll(sAttack.transform.position, attackRange, herculesLayer);
        foreach (Collider2D hercules in hitHercules)
        {
            //Debug.Log(hercules.name);
            hercules.GetComponent<Bandit>().TakeDamageSkeleton();
         }
    }
    public void TakeDamage()
    {
        health = health - 10;
        s_Animator.SetTrigger("hit");

    }
    /*IEnumerator sAttackDisable()
    {
        yield return new WaitForSeconds(1);
        sAttack.SetActive(false);
    }*/
    IEnumerator destroyAfterDeath()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    void moveCharachter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private void OnDrawGizmosSelected()
    {
        if (sAttack.transform.position == null)
            return;
        Gizmos.DrawWireSphere(sAttack.transform.position, attackRange);
    }
}
