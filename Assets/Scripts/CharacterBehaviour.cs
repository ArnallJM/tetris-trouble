using System;
using UnityEngine;
using CustomClasses;

#pragma warning disable 649
namespace UnityStandardAssets._2D
{
    public class CharacterBehaviour : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 800f;                  // Amount of force added when the player jumps.
//         [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        [SerializeField] private float m_SlashTime = 0.2f;                  // Length of time slash is active
        [SerializeField] private float m_DashTime = 0.1f;
        [SerializeField] private float m_DashSpeed = 50f;
        [SerializeField] private float m_BoostTime = 0.2f;
        [SerializeField] private float m_BoostForce = 800f;
        [SerializeField] private bool m_BoostResetSpeed = true;
        [SerializeField] private GameObject m_BulletPrefab;
        [SerializeField] private float m_ShootTime = 0.1f;
        [SerializeField] private float m_BulletSpeed = 20f;
        [SerializeField] private float m_BulletLifeTime = 1f;
        [SerializeField] private GameObject m_BombPrefab;
        [SerializeField] private float m_BombTime = 0.1f;
        [SerializeField] private float m_BombCountDown = 0.5f;
        [SerializeField] private float m_ExplosionTime = 0.1f;
        [SerializeField] private float m_BeamTime = 0.5f;
        [SerializeField] private float m_AttackCooldownTime = 0.5f;               // Cooldown time between attacks

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = 1f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
//         private Transform m_CeilingCheck;   // A position marking where to check for ceilings
//         const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
//         private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
//         public enum AttackState { Idle, Cooldown, Slash, Dash }
        private AttackState m_CurrentAttackState = AttackState.Idle;
        private float m_AttackToggleTime;      // Time at which attacks become possible again
        private System.Random m_RandomGenerator = new System.Random();
        
        const int k_FunctionalAttacks = 6;
        private GameObject m_SlashObject;   // Gameobject of slash attack
        private GameObject m_DashObject;   // Box collider of player
        private GameObject m_BoostObject;
        private Vector3 m_BulletSpawnPosition = new Vector3(1.0f, 0f, 0f);
        private GameObject m_BeamObject;
        
        private AttackState m_HeldAttack = AttackState.Idle;
        private AttackState m_NextAttack = AttackState.Idle;
        private AttackState m_PreparedAttack = AttackState.Idle;
        private IconContainerBehaviour m_IconContainerBehaviour;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
//             m_CeilingCheck = transform.Find("CeilingCheck");
//             m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_IconContainerBehaviour = GameObject.Find("/Main Camera/IconContainer").GetComponent<IconContainerBehaviour>();
            
            m_SlashObject = (transform.Find("Attacks/Slash")).gameObject;
            m_DashObject = (transform.Find("Attacks/Dash")).gameObject;
            m_BoostObject = transform.Find("Attacks/Boost").gameObject;
            m_BeamObject = transform.Find("Attacks/Beam").gameObject;
        }
        
        private void Start()
        {
            m_PreparedAttack = RollAttack();
            m_NextAttack = RollAttack();
            RefreshIcons();
        }


        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
//             m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
//             m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }


        public void Move(float move, bool jump)
        {
            // If crouching, check to see if the character can stand up
//             if (!crouch && m_Anim.GetBool("Crouch"))
//             {reparedDash;
//     private GameObject m_PreparedBoost;
//     private GameObject m_PreparedShoot;
                // If the character has a ceiling preventing them from standing up, keep them crouching
//                 if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
//                 {
//                     crouch = true;
//                 }
//             }

            // Set whether or not the character is crouching in the animator
//             m_Anim.SetBool("Crouch", crouch);
            
            if (m_CurrentAttackState == AttackState.Dash)
            {
                if (m_FacingRight)
                {
                    m_Rigidbody2D.velocity = new Vector2(m_DashSpeed, m_Rigidbody2D.velocity.y);
                }
                else
                {
                    m_Rigidbody2D.velocity = new Vector2(-m_DashSpeed, m_Rigidbody2D.velocity.y);
                }
            }
            //only control the player if grounded or airControl is turned on
            else if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
//                 move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
//                 m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
//                 m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }
        
        public void RefreshIcons()
        {
//             Debug.Log(m_HeldAttack);
//             Debug.Log(m_NextAttack);
//             Debug.Log(m_PreparedAttack);
            m_IconContainerBehaviour.SetHeldAttack(m_HeldAttack);
            m_IconContainerBehaviour.SetNextAttack(m_NextAttack);
            m_IconContainerBehaviour.SetPreparedAttack(m_PreparedAttack);
        }
        
        public AttackState RollAttack()
        {
            int number = m_RandomGenerator.Next(2, 2+k_FunctionalAttacks);
            return (AttackState)number;
        }
        
        public AttackState PopPreparedAttack()
        {
            AttackState attack = m_PreparedAttack;
            m_PreparedAttack = m_NextAttack;
            m_NextAttack = RollAttack();
            m_IconContainerBehaviour.SetPreparedAttack(m_PreparedAttack);
            m_IconContainerBehaviour.SetNextAttack(m_NextAttack);
            return attack;
        }
        
        public void HoldPreparedAttack(bool hold)
        {
            if (hold)
            {
                AttackState tempAttack = m_PreparedAttack;
                if (m_HeldAttack == AttackState.Idle)
                {
                    m_PreparedAttack = m_NextAttack;
                    m_NextAttack = RollAttack();
                }
                else
                {
                    m_PreparedAttack = m_HeldAttack;
                }
                m_HeldAttack = tempAttack;
                RefreshIcons();
            }
        }
        
        public void UsePreparedAttack(bool attack)
        {
            if (attack && m_CurrentAttackState == AttackState.Idle)
            {
                AttackState tempAttack = PopPreparedAttack();
                Attack(tempAttack);
            }
            else
            {
                Attack(AttackState.Idle);
            }
        }
        
        public void Attack(AttackState attack)
        {   
            switch(m_CurrentAttackState)
            {
                case AttackState.Idle:
                    switch (attack)
                    {
                        case AttackState.Idle:
                            break;
                        case AttackState.Slash:
                            m_SlashObject.SetActive(true);
                            m_CurrentAttackState = AttackState.Slash;
                            m_AttackToggleTime = Time.time + m_SlashTime;
                            break;
                        case AttackState.Dash:
                            m_DashObject.SetActive(true);
                            m_CurrentAttackState = AttackState.Dash;
                            m_AttackToggleTime = Time.time + m_DashTime;
                            break;
                        case AttackState.Boost:
                            m_BoostObject.SetActive(true);
                            m_CurrentAttackState = AttackState.Boost;
                            m_AttackToggleTime = Time.time + m_BoostTime;
                            if (m_BoostResetSpeed)
                            {
                                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
                            }
                            m_Rigidbody2D.AddForce(new Vector2(0f, m_BoostForce));
                            break;
                        case AttackState.Shoot:
                            GameObject newBullet = Instantiate(m_BulletPrefab, transform.position + m_BulletSpawnPosition, transform.rotation) as GameObject;
                            newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(m_BulletSpeed * (m_FacingRight ? 1: -1), 0f);
                            newBullet.GetComponent<BulletBehaviour>().m_BulletLifeTime = m_BulletLifeTime;
                            m_AttackToggleTime = Time.time + m_ShootTime;
                            break;
                        case AttackState.Bomb:
                            GameObject newBomb = Instantiate(m_BombPrefab, transform.position, transform.rotation) as GameObject;
                            BombBehaviour bombBehaviour = newBomb.GetComponent<BombBehaviour>();
                            bombBehaviour.m_ExplosionTime = m_ExplosionTime;
                            bombBehaviour.m_CountDownTimer = m_BombCountDown;
                            m_AttackToggleTime = Time.time + m_BombTime;
                            break;
                        case AttackState.Beam:
                            m_BeamObject.SetActive(true);
                            m_CurrentAttackState = AttackState.Beam;
                            m_AttackToggleTime = Time.time + m_BeamTime;
                            break;
                        default:
                            Debug.LogError("Attack command not implemented");
                            break;
                    }
                    break;
                case AttackState.Cooldown:
                    if (Time.time >= m_AttackToggleTime)
                    {
                        m_CurrentAttackState = AttackState.Idle;
                    }
                    break;
                case AttackState.Slash:
                    if (Time.time >= m_AttackToggleTime)
                    {
                        m_SlashObject.SetActive(false);
                        m_CurrentAttackState = AttackState.Cooldown;
                        m_AttackToggleTime = m_AttackToggleTime + m_AttackCooldownTime;
                    }
                    break;
                case AttackState.Dash:
                    if (Time.time >= m_AttackToggleTime)
                    {
                        m_DashObject.SetActive(false);
                        m_CurrentAttackState = AttackState.Cooldown;
                        m_AttackToggleTime = m_AttackToggleTime + m_AttackCooldownTime;
                    }
                    break;
                case AttackState.Boost:
                    if (Time.time >= m_AttackToggleTime)
                    {
                        m_BoostObject.SetActive(false);
                        m_CurrentAttackState = AttackState.Cooldown;
                        m_AttackToggleTime = m_AttackToggleTime + m_AttackCooldownTime;
                    }
                    break;
                case AttackState.Shoot:
                    if (Time.time >= m_AttackToggleTime)
                    {
                        m_CurrentAttackState = AttackState.Cooldown;
                        m_AttackToggleTime = m_AttackToggleTime + m_AttackCooldownTime;
                    }
                    break;
                case AttackState.Bomb:
                    if (Time.time >= m_AttackToggleTime)
                    {
                        m_CurrentAttackState = AttackState.Cooldown;
                        m_AttackToggleTime = m_AttackToggleTime + m_AttackCooldownTime;
                    }
                    break;
                case AttackState.Beam:
                    if (Time.time >= m_AttackToggleTime)
                    {
                        m_BeamObject.SetActive(false);
                        m_CurrentAttackState = AttackState.Cooldown;
                        m_AttackToggleTime = m_AttackToggleTime + m_AttackCooldownTime;
                    }
                    break;
                default:
                    Debug.LogError("Attack state not implemented");
                    break;
            }
        }
                        


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
