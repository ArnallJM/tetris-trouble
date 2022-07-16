using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using CustomClasses;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (CharacterBehaviour))]
    public class UserController : MonoBehaviour
    {
        private CharacterBehaviour m_Character;
        private bool m_Jump;
        private bool m_Attack;
        private bool m_Hold;


        private void Awake()
        {
            m_Character = GetComponent<CharacterBehaviour>();
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Quitting...");
                Application.Quit();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene ("test scene");
            }
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
            if (!m_Attack)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    m_Attack = true;
                }
                else if (Input.GetKeyDown(KeyCode.K))
                {
                    m_Hold = true;
                }
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
//             bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.UsePreparedAttack(m_Attack);
            m_Character.HoldPreparedAttack(m_Hold);
            m_Character.Move(h, m_Jump);
            m_Attack = false;
            m_Jump = false;
            m_Hold = false;
        }
    }
}
