using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [Header("Componetes")]
    public Rigidbody body;
    [Header("Variáveis")]
    public float speed;
    public bool isCol;
    public Vector2 directionMove;
    public int directionRandom;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        if (isCol) 
        {
            if (directionMove.x == 1) 
            {
                transform.Translate(transform.forward * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(transform.forward * -speed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "StreetPlane" && !isCol)
        {
            isCol = true;
            directionRandom = (int)Random.Range(directionMove.x, directionMove.y);
        }
    }
}
