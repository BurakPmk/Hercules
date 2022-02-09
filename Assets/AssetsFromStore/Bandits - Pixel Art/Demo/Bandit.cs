using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Bandit : MonoBehaviour {

    
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;

    public int health = 100;
    public GameObject healthBar;
    public Transform Bar;
    public GameObject cam;
    public GameObject shield;
    public GameObject shieldLayer;
    public GameObject attack;
    public GameObject ifDeath;
    public LayerMask enemyLayers;
    public float attackRange = 0.5f;
    float fireTime = 0;
    Vector3 fark;
    Vector3 rfncs;
    //float shieldTime = 0;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    public AudioSource swordAttack;
    public AudioSource hurt;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;
    public bool falling = false;


    // Use this for initialization
    void Start() {
        
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        fark = cam.transform.position - transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        camcontrol(rfncs, fark);
        if (transform.position.y < -18)
        {
            health = 0;
            SetSize(health*0.1f);
            falling = true;
        }
        //Death
        if (health == 0)
        {
            if (!m_isDead)
                m_animator.SetTrigger("Death");
            ifDeath.SetActive(true);
            m_isDead = !m_isDead;
            
        }
        else if (health > 0)
        {
            ifDeath.SetActive(false);
            //Check if character just landed on the ground
            if (!m_grounded && m_groundSensor.State())
            {
                m_grounded = true;
                m_animator.SetBool("Grounded", m_grounded);
            }

            //Check if character just started falling
            if (m_grounded && !m_groundSensor.State())
            {
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
            }

            // -- Handle input and movement --
            //float inputX = Input.GetAxis("Horizontal"); //FOR WİNDOWS CONTROL
            float inputX = CrossPlatformInputManager.GetAxis("Horizontal");
            // Swap direction of sprite depending on walk direction
            if (inputX > 0)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            else if (inputX < 0)
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            // Move
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);


            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

            // -- Handle Animations --
            //Death
            /*if (health == 0)
            {
                if (!m_isDead)
                    m_animator.SetTrigger("Death");



                m_isDead = !m_isDead;
            }*/


            //if (Input.GetKeyDown("e")) //This for Windows!!
            if (CrossPlatformInputManager.GetButtonDown("Shield"))
            {

                m_animator.SetTrigger("Recover");
                shield.SetActive(true);
                shieldLayer.SetActive(true);
                StartCoroutine(shieldDisabled());

            }


            //Hurt
            else if (Input.GetKeyDown("q"))
                m_animator.SetTrigger("Hurt");

            //Attack
            //else if (Input.GetMouseButtonDown(0) && Time.time > fireTime) //THİS FOR WİNDOWS TOO!!
            else if (CrossPlatformInputManager.GetButtonDown("Attack") && Time.time > fireTime)
            {
                
                fireTime = Time.time + 1;
                Attack();
                /*m_animator.SetTrigger("Attack");
                attack.SetActive(true);
                StartCoroutine(attackDisabled());*/
            }

            //Change between idle and combat idle
            else if (Input.GetKeyDown("f"))
                m_combatIdle = !m_combatIdle;

            //Jump
            //else if (Input.GetKeyDown("space") && m_grounded) //THİS FOR WİNDOWS!!
            else if (CrossPlatformInputManager.GetButtonDown("Jump") && m_grounded)
            {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }

            //Run
            else if (Mathf.Abs(inputX) > Mathf.Epsilon)
                m_animator.SetInteger("AnimState", 2);

            //Combat Idle
            else if (m_combatIdle)
                m_animator.SetInteger("AnimState", 1);

            //Idle
            else
                m_animator.SetInteger("AnimState", 0);


        }
      
       
    }

    public void SetSize(float sizeNormalized)
    {
        Bar.localScale = new Vector3(sizeNormalized, 1f);
    }



    



    private void OnCollisionEnter2D(Collision2D c)
    {
        
        if (c.collider.gameObject.tag == "arrow")
        {
            m_animator.SetTrigger("Hurt");
            hurt.Play();
            if (health >= 0)
            {
                health = health - 5;
            }
            SetSize(health * 0.01f);
            //Debug.Log(health);
        }
        
        if (c.collider.gameObject.tag == "fireBall")
        {
            m_animator.SetTrigger("Hurt");
            hurt.Play();
            if (health >= 0)
            {
                health = health - 20;
            }
            SetSize(health * 0.01f);
            //Debug.Log(health);
        }

        if (c.collider.gameObject.tag == "sAttack")
        {
            m_animator.SetTrigger("Hurt");
            hurt.Play();
            if (health >= 0)
            { 
                health = health - 10; 
            }
            SetSize(health * 0.01f);
            //Debug.Log(health);
        }

        if (c.collider.gameObject.tag == "healthPoint" && health < 80)
        {
            health = health + 30;
            SetSize(health * 0.01f);
            Destroy(c.gameObject);
        } else if (c.collider.gameObject.tag == "healthPoint" && health > 70 && health < 90)
        {
            health = health + 20;
            SetSize(health * 0.01f);
            Destroy(c.gameObject);

        }
        else if (c.collider.gameObject.tag == "healthPoint" && health > 80 && health < 100)
        {
            health = health + 10;
            SetSize(health * 0.01f);
            Destroy(c.gameObject);

        }
        else if (c.collider.gameObject.tag == "healthPoint")
        {
            Destroy(c.gameObject);
        }
    }
    IEnumerator shieldDisabled()
    {
        yield return new WaitForSeconds(1);
        shield.SetActive(false);
        shieldLayer.SetActive(false);

    }
    IEnumerator attackDisabled()
    {
        yield return new WaitForSeconds(1);
        attack.SetActive(false);

    }
    public void TakeDamageLion() 
    {
        
        if (health >= 0)
        { 
            health = health - 25; 
        }
        else if(health <= 0)
        {
            health = 0;
        }
        SetSize(health * 0.01f);
        m_animator.SetTrigger("Hurt");
        hurt.Play();

    }
    public void TakeDamageSkeleton() 
    {
        
        if (health >= 0)
        { 
            health = health - 10; 
        }
        else if(health <= 0)
        {
            health = 0;
        }
        SetSize(health * 0.01f);
        m_animator.SetTrigger("Hurt");
        hurt.Play();

    }
    void Attack()
    {
        m_animator.SetTrigger("Attack");
        swordAttack.Play();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attack.transform.position, attackRange,enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.tag == "archer")
            {
                enemy.GetComponent<archerBehavior>().TakeDamage();
            }
            else if (enemy.tag == "skeleton")
            {
                enemy.GetComponent<skeletonBehavior>().TakeDamage();
            }
            else if (enemy.tag == "lion")
            {
                enemy.GetComponent<nemeanLion>().TakeDamage();
            }
            else if (enemy.tag == "hydra")
            {
                enemy.GetComponent<hydra>().TakeDamage();
            }
        }
    }

    void camcontrol(Vector3 rfrncs, Vector3 fark)
    {
        rfncs = transform.position + fark;
        cam.transform.position = new Vector3(rfncs.x, cam.transform.position.y, cam.transform.position.z);
    }

    private void OnDrawGizmosSelected()
    {
        if (attack.transform.position == null)
            return;
        Gizmos.DrawWireSphere(attack.transform.position, attackRange);
    }
}
