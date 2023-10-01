using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // This is very important if we want to restart the level
public class EnemyReaction : MonoBehaviour {
  // Use this for initialization
  void Start () {
    
  }
  
  // Update is called once per frame
  void Update () {
    
  }
  // This function is called every time another collider overlaps the trigger collider
  void OnCollisionEnter2D (Collision2D other){
    // Checking if the overlapped collider is an enemy
//     Debug.Log(other.collider.tag);
    if (other.collider.CompareTag ("Enemy")) {
      // This scene HAS TO BE IN THE BUILD SETTINGS!!!
      GameBehaviour.Instance.OnPlayerDeath();
    }
  }
}
