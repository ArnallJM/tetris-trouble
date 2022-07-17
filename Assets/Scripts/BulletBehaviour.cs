using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float m_BulletLifeTime = 0f;
    private float m_DecayTime;
    // Start is called before the first frame update
    void Start()
    {
        m_DecayTime = Time.time + m_BulletLifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= m_DecayTime)
        {
            Object.Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Solid" || other.tag == "Enemy")
        {
            Object.Destroy(gameObject);
        }
    }
}
