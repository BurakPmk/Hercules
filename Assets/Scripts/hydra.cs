using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hydra : MonoBehaviour
{
    private Animator h_Animator;
    public GameObject fireBall;
    public GameObject ifBossDead;
    GameObject fireBallClone;
    GameObject fireBallClone1;
    GameObject fireBallClone2;
    GameObject fireBallClone3;
    GameObject fireBallClone4;
    GameObject fireBallClone5;
    public GameObject hercules;
    public GameObject healthBar;
    public Transform Bar;
    public Transform fireBallInstantiateP;
    public Transform fireBallInstantiateP1; 
    public Transform fireBallInstantiateP2;

    
    float fireTime = 0;
    Vector2 direction;
    Vector3 fark;
    public float speed;
    private int health = 100;
    private bool stunControl = false;
    private bool CoroutineControll = false;
    float f = 0.01f;
    void Start()
    {
        h_Animator = GetComponent<Animator>();
    }

    
    void Update()
    {

        fark = transform.position - hercules.transform.position;
        direction = new Vector2(hercules.transform.position.x - transform.position.x, (hercules.transform.position.y + f) - transform.position.y);
        if (health <= 0)
        {
            h_Animator.SetBool("dead", true);
            ifBossDead.SetActive(true);
        }
        else if (health > 0)
        {
            ifBossDead.SetActive(false);
            if (!stunControl)
            {
                if (fark.x > 0 && fark.x <= 23 && Time.time > fireTime)
                {
                    StartCoroutine(attack2());
                    fireTime = Time.time + 4;
                }
            }
        }
    }

    IEnumerator attack2()
    {
        if (CoroutineControll)
            yield break;

        CoroutineControll = true;
        h_Animator.SetTrigger("attack2");
        yield return new WaitForSeconds(1);
        fireBallClone3 = Instantiate(fireBall, fireBallInstantiateP1.position, Quaternion.identity);
        fireBallClone3.GetComponent<Rigidbody2D>().velocity = direction * speed;
        yield return new WaitForSeconds(0.4f);
        fireBallClone4 = Instantiate(fireBall, fireBallInstantiateP1.position, Quaternion.identity);
        fireBallClone4.GetComponent<Rigidbody2D>().velocity = direction * speed;
        yield return new WaitForSeconds(1.1f);
        fireBallClone5 = Instantiate(fireBall, fireBallInstantiateP2.position, Quaternion.identity);
        fireBallClone5.GetComponent<Rigidbody2D>().velocity = direction * speed;
        yield return new WaitForSeconds(2);
        h_Animator.SetTrigger("attack");
        yield return new WaitForSeconds(1);
        fireBallClone = Instantiate(fireBall, fireBallInstantiateP.position, Quaternion.identity);
        fireBallClone.GetComponent<Rigidbody2D>().velocity = direction * speed;
        fireBallClone1 = Instantiate(fireBall, fireBallInstantiateP1.position, Quaternion.identity);
        fireBallClone1.GetComponent<Rigidbody2D>().velocity = direction * speed;
        fireBallClone2 = Instantiate(fireBall, fireBallInstantiateP2.position, Quaternion.identity);
        fireBallClone2.GetComponent<Rigidbody2D>().velocity = direction * speed;
        CoroutineControll = false;
    }

    void SetSize(float sizeNormalized)
    {
        Bar.localScale = new Vector3(sizeNormalized, 1f);
    }
    public void TakeDamage()
    {
        if (health > 0)
        {
            health = health - 5;
            SetSize(health * 0.01f);
        }
    }
    IEnumerator backFromStun()
    {
        yield return new WaitForSeconds(5);
        h_Animator.SetBool("stun", false);
        stunControl = false;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (health > 0)
        {
            if (collision.gameObject.tag == "stun")
            {
                health = health - 10;
                SetSize(health * 0.01f);
                //Debug.Log(health);
                h_Animator.SetBool("stun", true);
                stunControl = true;
                Destroy(collision.gameObject);
                StartCoroutine(backFromStun());
            }
        }
    }
}

