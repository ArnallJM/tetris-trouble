using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReaction : MonoBehaviour
{
    [SerializeField] private bool m_Invincible = false;
    
    void OnTriggerEnter2D (Collider2D other) 
    {
        if (!m_Invincible && other.CompareTag ("PlayerAttack"))
        {
            Object.Destroy(gameObject);
        }
    }
}
