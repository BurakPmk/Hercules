using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatformR : MonoBehaviour
{
    public Transform p1, p2;
    public float speed;
    public Transform startP;
    private GameObject target = null;
    private Vector3 offset;

    Vector3 nextP;

    // Update is called once per frame
    private void Start()
    {
        nextP = startP.position;
    }
    void Update()
    {

        if(transform.position == p1.position)
        {
            nextP = p2.position;
        }
        if(transform.position == p2.position)
        {
            nextP = p1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextP, speed * Time.deltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "hercules")
        {
            collision.transform.parent = transform;
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="hercules")
        {
            collision.transform.parent = null;
        }
    }

}
