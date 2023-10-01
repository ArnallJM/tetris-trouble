using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPlayerDetectorBehaviour : MonoBehaviour
{
    private FlyingEnemyBehaviour m_FlyingEnemyBehaviour;

    // Start is called before the first frame update
    void Awake()
    {
        m_FlyingEnemyBehaviour = transform.parent.gameObject.GetComponent<FlyingEnemyBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // Debug.Log("Found");
            m_FlyingEnemyBehaviour.TriggerSwoop(other.transform.position);
        }
    }
}
