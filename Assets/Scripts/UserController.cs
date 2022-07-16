using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (CharacterBehaviour))]
    public class UserController : MonoBehaviour
    {
        private CharacterBehaviour m_Character;
        private bool m_Jump;
        private CharacterBehaviour.AttackState m_Attack;


        private void Awake()
        {
            m_Character = GetComponent<CharacterBehaviour>();
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene ("test scene");
            }
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
            if (m_Attack == CharacterBehaviour.AttackState.Idle)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    m_Attack = CharacterBehaviour.AttackState.Slash;
                }
                else if (Input.GetKeyDown(KeyCode.K))
                {
                    m_Attack = CharacterBehaviour.AttackState.Dash;
                }
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
//             bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Attack(m_Attack);
            m_Character.Move(h, m_Jump);
            m_Attack = CharacterBehaviour.AttackState.Idle;
            m_Jump = false;
        }
    }
}
