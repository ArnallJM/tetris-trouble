using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float m_Speed = 5f;
    [SerializeField] private bool m_FacingRight = true;
    
    private float m_LeftBoundary;
    private float m_RightBoundary;
    private Rigidbody2D m_Rigidbody2D;
//     private bool m_FacingRight;
    
    // Start is called before the first frame update
    void Awake()
    {
        m_FacingRight = !m_FacingRight;
        Transform parent = gameObject.transform.parent;
        m_LeftBoundary = parent.Find("LeftMarker").position.x;
        m_RightBoundary = parent.Find("RightMarker").position.x;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_FacingRight)
        {
            if (gameObject.transform.position.x >= m_RightBoundary || m_Rigidbody2D.velocity.x == 0)
            {
                m_FacingRight = false;
            }
        }
        else
        {
            if (gameObject.transform.position.x <= m_LeftBoundary || m_Rigidbody2D.velocity.x == 0)
            {
                m_FacingRight = true;
            }
        }
    }
    
    void FixedUpdate()
    {
        Move();
    }
    
    void Move()
    {
        m_Rigidbody2D.velocity = new Vector2(m_Speed*(m_FacingRight? 1 : -1), m_Rigidbody2D.velocity.y);
    }
}
