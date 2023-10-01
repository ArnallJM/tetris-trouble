using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorBehaviour : MonoBehaviour
{
    public bool m_Solid = false;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.layer != LayerMask.NameToLayer("SolidDetectors"))
        {
            Debug.LogError("Detector must have layer SolidDetectors");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // m_Solid = false;
    }

    // MUST BE CALLED EXACTLY ONCE PER FIXED UPDATE
    public bool Probe()
    {
        bool temp = m_Solid;
        m_Solid = false;
        return temp;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Entered wall: " + gameObject);
        m_Solid = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Debug.Log("Remaining in wall: " + gameObject);
        m_Solid = true;
    }
}
