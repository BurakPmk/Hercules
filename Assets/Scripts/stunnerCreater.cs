using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stunnerCreater : MonoBehaviour
{
    public Transform stunnerP;
    
    public GameObject stunner;
    float createTime = 0;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Time.time > createTime)
        {
            createTime = Time.time + 15;
            Instantiate(stunner, stunnerP.position, Quaternion.identity);
            
        }
    }

   
}
