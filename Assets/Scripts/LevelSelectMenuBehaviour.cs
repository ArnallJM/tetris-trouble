using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectMenuBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(Screen.height/540, Screen.height/540, 1);
        string bestTimePath = "LevelContainer/Level {0}/BestTime";
        string bestAllClearTimePath = "LevelContainer/Level {0}/BestAllClearTime";
        string path;
        TextMeshProUGUI text;
        for(int i=0; i < GameBehaviour.Instance.m_LevelCount; i++)
        {
            path = string.Format(bestTimePath, i+1);
            text = transform.Find(path).gameObject.GetComponent<TextMeshProUGUI>();
            text.SetText("{0:2}s", GameBehaviour.Instance.m_BestLevelTimes[i]);

            path = string.Format(bestAllClearTimePath, i+1);
            text = transform.Find(path).gameObject.GetComponent<TextMeshProUGUI>();
            text.SetText("{0:2}s", GameBehaviour.Instance.m_BestAllClearTimes[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenuButton()
    {
        GameBehaviour.Instance.MainMenu();
    }

    public void Level1Button()
    {
        SceneManager.LoadScene(1);
    }

    public void Level2Button()
    {
        SceneManager.LoadScene(2);
    }

    public void Level3Button()
    {
        SceneManager.LoadScene(3);
    }

    public void Level4Button()
    {
        SceneManager.LoadScene(4);
    }

    public void Level5Button()
    {
        SceneManager.LoadScene(5);
    }
}
