using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    // private GameBehaviour m_GameBehaviour;
    // Start is called before the first frame update
    void Awake()
    {
        // m_GameBehaviour = GameObject.Find("/GameManager").GetComponent<GameBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            GameBehaviour.Instance.OnVictory();
        }
    }
}
