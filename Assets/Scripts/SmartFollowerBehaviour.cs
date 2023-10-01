using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartFollowerBehaviour : MonoBehaviour
{
    [SerializeField] private float m_Speed = 5f;
    [SerializeField] private float m_TriggerRadius = 10f;
    [SerializeField] private float m_JumpForce = 1200f;

    private Transform m_PlayerTransform;
    private Rigidbody2D m_Rigidbody2D;
    private SpriteRenderer m_SpriteRenderer;
    private DetectorBehaviour m_GroundDetector;
    private DetectorBehaviour m_WallDetector;
    private DetectorBehaviour m_HoleDetector;

    private float m_ChaseDirection = 0f;
    private bool m_FacingRight = false;
    const float k_JumpDelay = 0.1f;
    private float m_JumpAllowed = 0f;
    private bool m_Floor = false;
    private bool m_Wall = false;
    private bool m_Hole = false;

    const float k_PositionPrecision = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        m_PlayerTransform = GameObject.Find("/Player").transform;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_GroundDetector = transform.Find("GroundDetector").gameObject.GetComponent<DetectorBehaviour>();
        m_WallDetector = transform.Find("WallDetector").gameObject.GetComponent<DetectorBehaviour>();
        m_HoleDetector = transform.Find("HoleDetector").gameObject.GetComponent<DetectorBehaviour>();
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
        m_Floor = m_GroundDetector.Probe();
        m_Hole = m_HoleDetector.Probe();
        m_Wall = m_WallDetector.Probe();
        m_Rigidbody2D.velocity = new Vector2(m_Speed*m_ChaseDirection, m_Rigidbody2D.velocity.y);
        if ((transform.position - m_PlayerTransform.position).magnitude <= m_TriggerRadius)
        {
            Jump();
        }
        if ((m_ChaseDirection < 0 && m_FacingRight) || (m_ChaseDirection > 0 && !m_FacingRight))
        {
            Flip();
        }
    }

    private void Jump()
    {
        if ((m_Floor && (m_Hole || m_Wall)) && Time.time >= m_JumpAllowed)
        {
            // Debug.Log(""+m_Floor + m_Hole + m_Wall);
            m_JumpAllowed = Time.time + k_JumpDelay;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    private void Flip()
    {
        // Switch the way the enemy is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the enemy's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        m_SpriteRenderer.flipX = !m_SpriteRenderer.flipX;
    }
}
