using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomClasses;

public class IconContainerBehaviour : MonoBehaviour
{
    private Camera m_Camera;
    private Transform m_IconTransform;
    private float m_CurrentAspect;
    
    private GameObject m_HeldAttack;
    private GameObject m_NextAttack;
    private GameObject m_PreparedAttack;
    
    private GameObject m_HeldSlash;
    private GameObject m_HeldDash;
    private GameObject m_HeldBoost;
    private GameObject m_HeldShoot;
    private GameObject m_HeldBomb;
    private GameObject m_HeldBeam;
    
    private GameObject m_NextSlash;
    private GameObject m_NextDash;
    private GameObject m_NextBoost;
    private GameObject m_NextShoot;
    private GameObject m_NextBomb;
    private GameObject m_NextBeam;
    
    private GameObject m_PreparedSlash;
    private GameObject m_PreparedDash;
    private GameObject m_PreparedBoost;
    private GameObject m_PreparedShoot;
    private GameObject m_PreparedBomb;
    private GameObject m_PreparedBeam;
    // Start is called before the first frame update
    void Start()
    {
        m_IconTransform = gameObject.transform;
        m_Camera = m_IconTransform.parent.gameObject.GetComponent<Camera>();
        
        UpdatePosition();
    }
    
    void Awake()
    {
        Transform heldAttackTransform = GameObject.Find("/Main Camera/IconContainer/ScreenCorner/HeldAttack").transform;
        Transform nextAttackTransform = GameObject.Find("/Main Camera/IconContainer/ScreenCorner/NextAttack").transform;
        Transform currentAttackTransform = GameObject.Find("/Main Camera/IconContainer/ScreenCorner/PreparedAttack").transform;
        
        m_HeldSlash = heldAttackTransform.Find("Slash").gameObject;
        m_HeldDash = heldAttackTransform.Find("Dash").gameObject;
        m_HeldBoost = heldAttackTransform.Find("Boost").gameObject;
        m_HeldShoot = heldAttackTransform.Find("Shoot").gameObject;
        m_HeldBomb = heldAttackTransform.Find("Bomb").gameObject;
        m_HeldBeam = heldAttackTransform.Find("Beam").gameObject;
        
        m_NextSlash = nextAttackTransform.Find("Slash").gameObject;
        m_NextDash = nextAttackTransform.Find("Dash").gameObject;
        m_NextBoost = nextAttackTransform.Find("Boost").gameObject;
        m_NextShoot = nextAttackTransform.Find("Shoot").gameObject;
        m_NextBomb = nextAttackTransform.Find("Bomb").gameObject;
        m_NextBeam = nextAttackTransform.Find("Beam").gameObject;
        
        m_PreparedSlash = currentAttackTransform.Find("Slash").gameObject;
        m_PreparedDash = currentAttackTransform.Find("Dash").gameObject;
        m_PreparedBoost = currentAttackTransform.Find("Boost").gameObject;
        m_PreparedShoot = currentAttackTransform.Find("Shoot").gameObject;
        m_PreparedBomb = currentAttackTransform.Find("Bomb").gameObject;
        m_PreparedBeam = currentAttackTransform.Find("Beam").gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (m_Camera.aspect != m_CurrentAspect)
        {
            UpdatePosition();
        }
    }
    
    void UpdatePosition()
    {
        float OrthoWidth = m_Camera.orthographicSize * m_Camera.aspect;
//         Debug.Log(OrthoWidth);
        m_IconTransform.localPosition = new Vector3 (OrthoWidth, -m_Camera.orthographicSize, 10.0F);
    }
    
    public void SetHeldAttack(AttackState attack)
    {
        if (m_HeldAttack != null)
        {
            m_HeldAttack.SetActive(false);
        }
        switch (attack)
        {
            case AttackState.Idle:
                m_HeldAttack = null;
                break;
            case AttackState.Slash:
                m_HeldAttack = m_HeldSlash;
                break;
            case AttackState.Dash:
                m_HeldAttack = m_HeldDash;
                break;
            case AttackState.Boost:
                m_HeldAttack = m_HeldBoost;
                break;
            case AttackState.Shoot:
                m_HeldAttack = m_HeldShoot;
                break;
            case AttackState.Bomb:
                m_HeldAttack = m_HeldBomb;
                break;
            case AttackState.Beam:
                m_HeldAttack = m_HeldBeam;
                break;
            default:
                m_HeldAttack = null;
                Debug.LogError("Invalid attack state sent to icon container");
                break;
        }
        if (m_HeldAttack != null)
        {
            m_HeldAttack.SetActive(true);
        }
    }
    
    public void SetNextAttack(AttackState attack)
    {
        if (m_NextAttack != null)
        {
            m_NextAttack.SetActive(false);
        }
        switch (attack)
        {
            case AttackState.Idle:
                m_NextAttack = null;
                break;
            case AttackState.Slash:
                m_NextAttack = m_NextSlash;
                break;
            case AttackState.Dash:
                m_NextAttack = m_NextDash;
                break;
            case AttackState.Boost:
                m_NextAttack = m_NextBoost;
                break;
            case AttackState.Shoot:
                m_NextAttack = m_NextShoot;
                break;
            case AttackState.Bomb:
                m_NextAttack = m_NextBomb;
                break;
            case AttackState.Beam:
                m_NextAttack = m_NextBeam;
                break;
            default:
                m_NextAttack = null;
                Debug.LogError("Invalid attack state sent to icon container");
                break;
        }
        if (m_NextAttack != null)
        {
            m_NextAttack.SetActive(true);
        }
    }
    
    public void SetPreparedAttack(AttackState attack)
    {
        if (m_PreparedAttack != null)
        {
            m_PreparedAttack.SetActive(false);
        }
        switch (attack)
        {
            case AttackState.Idle:
                m_PreparedAttack = null;
                break;
            case AttackState.Slash:
                m_PreparedAttack = m_PreparedSlash;
                break;
            case AttackState.Dash:
                m_PreparedAttack = m_PreparedDash;
                break;
            case AttackState.Boost:
                m_PreparedAttack = m_PreparedBoost;
                break;
            case AttackState.Shoot:
                m_PreparedAttack = m_PreparedShoot;
                break;
            case AttackState.Bomb:
                m_PreparedAttack = m_PreparedBomb;
                break;
            case AttackState.Beam:
                m_PreparedAttack = m_PreparedBeam;
                break;
            default:
                m_PreparedAttack = null;
                Debug.LogError("Invalid attack state sent to icon container");
                break;
        }
        if (m_PreparedAttack != null)
        {
            m_PreparedAttack.SetActive(true);
        }
    }
}
