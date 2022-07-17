using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    public float m_CountDownTimer = 0f;
    private float m_Alarm;
    public float m_ExplosionTime = 0f;
    private GameObject m_Explosion;
    // Start is called before the first frame update
    void Start()
    {
        m_Alarm = Time.time + m_CountDownTimer;
        m_Explosion = gameObject.transform.Find("Explosion").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= m_Alarm)
        {
            if (m_Explosion.activeSelf)
            {
                Object.Destroy(gameObject);
            }
            else
            {
                m_Explosion.SetActive(true);
                m_Alarm = m_Alarm + m_ExplosionTime;
            }
        }
    }
}
