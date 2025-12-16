using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CollisionCheck : MonoBehaviour
{
    public bool isColliding = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
        {
            isColliding = false;
        }
    }
}