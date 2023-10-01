using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.localScale = new Vector3(Screen.height/540, Screen.height/540, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void LevelsButton()
    {
        transform.parent.Find("LevelSelectMenu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void CreditsButton()
    {
        gameObject.SetActive(false);
        GameBehaviour.Instance.CreditsButton();
    }
}
