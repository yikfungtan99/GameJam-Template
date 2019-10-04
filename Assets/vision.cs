using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vision : MonoBehaviour
{
    public bool detectEnemy = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            detectEnemy = true;
        }
    }
}
