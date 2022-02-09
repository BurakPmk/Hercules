using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBallController : MonoBehaviour
{
    private void Update()
    {
        Destroy(gameObject, 3);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "shield" || collision.collider.gameObject.tag == "hercules")
        {
            Destroy(gameObject);
        }
    }
}
