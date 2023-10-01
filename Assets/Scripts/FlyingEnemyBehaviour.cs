using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlyingEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float m_Speed = 5f;
    
    const float k_VelocityZero = 0.0001f;
    private float m_LeftBoundary;
    private float m_RightBoundary;
    private Rigidbody2D m_Rigidbody2D;
    private SpriteRenderer m_SpriteRenderer;
    private bool m_FacingRight = false;
    private bool m_Swooping = false;
    private Vector2 m_Nest = new Vector2(0,0);
    private Vector2 m_Target = new Vector2(0,0);
    private float m_SlopeFactor = 0f;
    
    // Start is called before the first frame update
    void Awake()
    {
        // m_FacingRight = !m_FacingRight;
        Transform parent = gameObject.transform.parent;
        m_LeftBoundary = parent.Find("LeftMarker").position.x;
        m_RightBoundary = parent.Find("RightMarker").position.x;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_FacingRight)
        {
            if (gameObject.transform.position.x >= m_RightBoundary)
            {
                Flip();
            }
        }
        else
        {
            if (gameObject.transform.position.x <= m_LeftBoundary)
            {
                Flip();
            }
        }
    }
    
    void FixedUpdate()
    {
        Move();
    }
    
    void Move()
    {
        m_Rigidbody2D.velocity = new Vector2(m_Speed*(m_FacingRight? 1 : -1), 0);
        if (m_Swooping)
        {
            ResolveSwoop();
        }
    }

    public void TriggerSwoop(Vector2 target)
    {
        if (!m_Swooping && (transform.position.x <= 2*target.x-m_LeftBoundary) && (2*target.x-transform.position.x <= m_RightBoundary))
        {
            // Debug.Log("Swooping!");
            m_Nest.x = transform.position.x;
            m_Nest.y = transform.position.y;
            m_Target.x = target.x;
            m_Target.y = target.y;
            CalculateSlopeFactor();
            m_Swooping = true;
        }
    }

    void CalculateSlopeFactor()
    {
        m_SlopeFactor = (m_Nest.y-m_Target.y) / (float)Math.Pow(m_Nest.x-m_Target.x, 2f);
    }

    void ResolveSwoop()
    {
        SetPosY(m_SlopeFactor*(float)Math.Pow(transform.position.x-m_Target.x, 2f) + m_Target.y);
        if (transform.position.y > m_Nest.y)
        {
            SetPosY(m_Nest.y);
            m_Swooping = false;
        }
    }

    void SetPosY(float y)
    {
        transform.position = new Vector2(transform.position.x, y);
    }

    private void Flip()
    {
        // Switch the way the enemy is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the enemy's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        m_SpriteRenderer.flipY = !m_SpriteRenderer.flipY;
    }
}
