using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogError($"TNam - Coin collides with {other.transform.name}");
        if (other.CompareTag("Player"))
        {
            Debug.LogError($"TNam - Coin hit player");
            Destroy(gameObject);

        }
    }
}
