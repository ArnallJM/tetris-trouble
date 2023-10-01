using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("/Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
