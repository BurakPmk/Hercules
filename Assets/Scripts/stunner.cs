using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stunner : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag =="stun")
        {
            Destroy(collision.gameObject);
        }
    }


}
