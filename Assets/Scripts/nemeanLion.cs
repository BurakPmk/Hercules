using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nemeanLion : MonoBehaviour
{
    private Animator l_Animator;
    public GameObject hercules;
    public GameObject lionAttackPoint;
    public GameObject healthBar;
    public GameObject ifBossDead;
    public Transform Bar;
    public AudioSource lionAttack;
    public LayerMask herculesLayer;
    private Rigidbody2D rb;
    public float fireTime = 0;
    public float attackRange = 0.5f;
    private int health  = 100;
    public float moveSpeed;
    private bool walk = true;
    private bool stunControl = false;
    Vector3 fark;
    Vector2 direction;
    Vector2 movement;

    void Start()
    {
        l_Animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        fark = transform.position - hercules.transform.position;
        direction = new Vector2(hercules.transform.position.x - transform.position.x, transform.position.y);
        direction.Normalize();
        movement = direction;
        if (hercules.transform.position.x < transform.position.x)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (hercules.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(1f, 1f, 1f);

        if (health <= 0)
        {
            l_Animator.SetBool("dead", true);
            ifBossDead.SetActive(true);
            PlayerPrefs.SetInt("level2",1);
           
        }
        else if (health > 0)
        {
            ifBossDead.SetActive(false);
            if (fark.x > 1 && fark.x <= 24)
            {

                moveCharachter(movement);
                l_Animator.SetBool("walk", walk);
                if (fark.x <= 3 && !stunControl)
                {
                    if (Time.time > fireTime)

                    {
                        fireTime = Time.time + 3;
                        l_Animator.SetTrigger("attack");
                        Attack();
                    }
                }

            }
            else if (fark.x > -24 && fark.x <= -1)
            {
                moveCharachter(movement);
                l_Animator.SetBool("walk", walk);
                if (fark.x >= -3 && !stunControl)
                {
                    Debug.Log("Saldýrý");
                    if (Time.time > fireTime)
                    { 
                        
                        fireTime = Time.time + 3;
                        l_Animator.SetTrigger("attack");
                        Attack();
                    }
                }

            }
            else
            {
                l_Animator.SetBool("walk", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(health>0)
        {
            if (collision.gameObject.tag == "stun")
            {
                health = health - 10;
                SetSize(health * 0.01f);
                //Debug.Log(health);
                l_Animator.SetBool("stun", true);
                stunControl = true;
                Destroy(collision.gameObject);
                StartCoroutine(backFromStun());
            }
        }
    }
    void Attack()
    {
       
        
        Collider2D[] hitHercules = Physics2D.OverlapCircleAll(lionAttackPoint.transform.position, attackRange, herculesLayer);
            foreach (Collider2D hercules in hitHercules)
            {
                //Debug.Log(hercules.name);
                hercules.GetComponent<Bandit>().TakeDamageLion();
                lionAttack.Play();


            }
        
    }
    void SetSize(float sizeNormalized)
    {
        Bar.localScale = new Vector3(sizeNormalized, 1f);
    }
    public void TakeDamage()
    {
        if(health>0)
        {
            health = health - 5;
            SetSize(health * 0.01f);
        }
        

    }
    IEnumerator backFromStun()
    {
        yield return new WaitForSeconds(5);
        l_Animator.SetBool("stun", false);
        stunControl = false;

    }
    void moveCharachter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private void OnDrawGizmosSelected()
    {
        if (lionAttackPoint.transform.position == null)
            return;
        Gizmos.DrawWireSphere(lionAttackPoint.transform.position, attackRange);
    }
}
