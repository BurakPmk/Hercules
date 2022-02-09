using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowController : MonoBehaviour
{
    
    
    GameObject hercules;
    GameObject archer;
    Vector2 direction;
    Vector3 fark;
    public int speed;
    
    void Start()
    {
        
        hercules = GameObject.Find("Hercules");
        archer = GameObject.Find("FantasyArcher_01");
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        fark = hercules.transform.position - archer.transform.position;

        if (fark.x > 0 && fark.x <= 5)
        {

            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            Destroy(gameObject, 3);

        }else
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            Destroy(gameObject, 3);
        }
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.tag=="shield" || collision.collider.gameObject.tag=="hercules")
        {
            Destroy(gameObject);
        }
        
    }

    


}
