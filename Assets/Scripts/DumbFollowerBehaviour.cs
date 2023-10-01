using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbFollowerBehaviour : MonoBehaviour
{
    [SerializeField] private float m_Speed = 5f;
    [SerializeField] private float m_TriggerRadius = 10f;

    private Transform m_PlayerTransform;
    private Rigidbody2D m_Rigidbody2D;
    private float m_ChaseDirection = 0f;
    private bool m_FacingRight = false;

    const float k_PositionPrecision = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        m_PlayerTransform = GameObject.Find("/Player").transform;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - m_PlayerTransform.position).magnitude <= m_TriggerRadius)
        {
            if (transform.position.x > m_PlayerTransform.position.x + k_PositionPrecision)
            {
                m_ChaseDirection = -1;
            }
            else if (transform.position.x < m_PlayerTransform.position.x - k_PositionPrecision)
            {
                m_ChaseDirection = 1;
            }
            else{
                m_ChaseDirection = 0;
            }
        }
        else
        {
            m_ChaseDirection = 0;
        }
    }

    void FixedUpdate()
    {
        m_Rigidbody2D.velocity = new Vector2(m_Speed*m_ChaseDirection, m_Rigidbody2D.velocity.y);
        if ((m_ChaseDirection < 0 && m_FacingRight) || (m_ChaseDirection > 0 && !m_FacingRight))
        {
            // Flip();
        }
    }

    // private void Flip()
    //     {
    //         // Switch the way the enemy is labelled as facing.
    //         m_FacingRight = !m_FacingRight;

    //         // Multiply the enemy's x local scale by -1.
    //         Vector3 theScale = transform.localScale;
    //         theScale.x *= -1;
    //         transform.localScale = theScale;
    //     }
}
