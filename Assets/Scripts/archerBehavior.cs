using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerBehavior : MonoBehaviour
{
    private Animator a_Animator;
    public float speed;
    private bool attack = true;
    public GameObject arrow;
    GameObject arrowClone;
    public AudioSource archerAttack;
    public AudioSource hit;
    public GameObject hercules;
    public Transform arrowInstantiatePlace;
    float fireTime = 0;
    //bool arrowControl = false;
    Vector2 direction;
    Vector3 fark;
    //int counter=0;
    float f = 0.50f;
    int health = 30;
    private bool isDead = false;


    void Start()
    {
        a_Animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fark = transform.position - hercules.transform.position;
        //direction = hercules.transform.position - transform.position;
        direction = new Vector2(hercules.transform.position.x - transform.position.x, (hercules.transform.position.y + f)- transform.position.y);
        
        if(health<=0)
        {
            if (isDead)
                a_Animator.SetTrigger("Death");
            StartCoroutine(destroyAfterDeath());
            isDead = !isDead;
        }
        
        if (fark.x>0 && fark.x<=5 && Time.time>fireTime)
        {
            
            a_Animator.SetBool("Attack", attack);
            archerAttack.Play();

            if (a_Animator.GetCurrentAnimatorStateInfo(0).IsName("Archer1_Attack2") && Time.time > fireTime)
            {
                a_Animator.SetBool("Attack", attack);
                
                fireTime = Time.time + 4;
                arrowClone = Instantiate(arrow, arrowInstantiatePlace.position, Quaternion.identity);
                arrowClone.GetComponent<Rigidbody2D>().velocity = direction * speed;
            }

        }
        else if(fark.x<0 && fark.x>=-5 && Time.time > fireTime)
        {
            a_Animator.SetBool("Attack", attack);
            archerAttack.Play();
            if (a_Animator.GetCurrentAnimatorStateInfo(0).IsName("Archer1_Attack2") && Time.time > fireTime)
            {
                for (int i = 0; i < 1; i++)
                {
                    
                    fireTime = Time.time + 2;
                    arrowClone = Instantiate(arrow, arrowInstantiatePlace.position, Quaternion.identity);
                    arrowClone.GetComponent<Rigidbody2D>().velocity = direction * speed;

                }
            }
            
        }
        else
        {
            a_Animator.SetBool("Attack", false);
            
            
        }
        
        

        if (hercules.transform.position.x < transform.position.x)
            transform.localScale = new Vector3(-0.3f, 0.5f, 1.0f);
        else if (hercules.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(0.3f, 0.5f, 1.0f);
       
    }

    IEnumerator arrowInstantiate()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 1; i++)
        {

            Instantiate(arrow, arrowInstantiatePlace.position, Quaternion.identity);
            
            
        }
    }
    
    
    IEnumerator destroyAfterDeath()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        health = health - 10;
        hit.Play();
        a_Animator.SetTrigger("hit");
        

    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "attack")
        {
            a_Animator.SetTrigger("hit");
            health = health - 10;
        }
        //Debug.Log(health);
    }*/

}
