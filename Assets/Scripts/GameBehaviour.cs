using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameBehaviour : MonoBehaviour
{
    public static GameBehaviour Instance;
    public int m_LevelCount;
    private int m_CurrentLevelIndex;
    // private bool m_IsPaused;
    // private bool m_IsLevelComplete;
    private GameObject m_PauseMenu;
    private GameObject m_LevelCompleteMenu;
    private GameObject m_CreditsMenu;
    private TextMeshProUGUI m_StatsText;
    const string k_StatsFormatString = "Completion Time: {0:2}s\nEnemies Defeated: {1}/{2}\nDeaths: {3}";
    private float m_CurrentLevelStartTime = 0f;
    private int m_CurrentLevelEnemies = 0;
    private int m_CurrentLevelDeaths = 0;
    private int m_CurrentLevelKills = 0;
    public float[] m_BestLevelTimes;
    private int[] m_LevelEnemies;
    private int[] m_BestLevelDeaths;
    public float[] m_BestAllClearTimes;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;
        SceneManager.sceneLoaded += OnSceneLoaded;

        m_LevelCount = SceneManager.sceneCountInBuildSettings-1;
        m_BestLevelTimes = new float[m_LevelCount];
        m_LevelEnemies = new int[m_LevelCount];
        m_BestLevelDeaths = new int[m_LevelCount];
        m_BestAllClearTimes = new float[m_LevelCount];
        float defaultValue = 999.99f;
        for(int i = 0; i < m_LevelCount; i++)
        {
            m_BestLevelTimes[i] = defaultValue;
            m_BestAllClearTimes[i] = defaultValue;
        }

        m_PauseMenu = transform.Find("Canvas/PauseMenu").gameObject;
        m_LevelCompleteMenu = transform.Find("Canvas/LevelCompleteMenu").gameObject;
        m_CreditsMenu = transform.Find("Canvas/CreditsMenu").gameObject;
        // m_LevelCompleteMenu.SetActive(true);
        m_StatsText = transform.Find("Canvas/LevelCompleteMenu/StatsText").gameObject.GetComponent<TextMeshProUGUI>();
        // m_LevelCompleteMenu.SetActive(false);
        m_PauseMenu.transform.localScale = new Vector3(Screen.height/540, Screen.height/540, 1);
        m_LevelCompleteMenu.transform.localScale = new Vector3(Screen.height/540, Screen.height/540, 1);
        m_CreditsMenu.transform.localScale = new Vector3(Screen.height/540, Screen.height/540, 1);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0)
        {
            Time.timeScale = 1f;
            if (scene.buildIndex != m_CurrentLevelIndex+1)
            {
                OnNewLevel();
            }
            

            m_CurrentLevelStartTime = Time.time;
            m_CurrentLevelKills = 0;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    void OnNewLevel()
    {
        // m_CurrentLevelEnemies = 0;
        m_CurrentLevelDeaths = 0;
        m_CurrentLevelIndex = SceneManager.GetActiveScene().buildIndex-1;
        // Transform enemyContainerTransform = GameObject.Find("Enemies").transform;
        // for (int i = 0; i < enemyContainerTransform.childCount; i++)
        // {
        //     Transform enemyTypeTransform = enemyContainerTransform.GetChild(i);
        //     for(int j=0; j < enemyTypeTransform.childCount; j++)
        //     {
        //         m_CurrentLevelEnemies++;
        //     }
        // }
        // if (m_LevelEnemies[m_CurrentLevelIndex] == 0)
        // {
        //     m_LevelEnemies[m_CurrentLevelIndex] = m_CurrentLevelEnemies;
        // }
        EnemySearch();
    }

    private void EnemySearch()
    {
        m_CurrentLevelEnemies = 0;
        Transform enemyContainerTransform = GameObject.Find("/Enemies").transform;
        for (int i = 0; i < enemyContainerTransform.childCount; i++)
        {
            Transform enemyTypeTransform = enemyContainerTransform.GetChild(i);
            for(int j=0; j < enemyTypeTransform.childCount; j++)
            {
                m_CurrentLevelEnemies++;
            }
        }
        if (m_LevelEnemies[m_CurrentLevelIndex] == 0)
        {
            m_LevelEnemies[m_CurrentLevelIndex] = m_CurrentLevelEnemies;
        }
    }

    public void OnPlayerDeath()
    {
        m_CurrentLevelDeaths++;
        RestartLevel();
    }

    public void OnEnemyDeath()
    {
        if (m_CurrentLevelEnemies == 0){
            EnemySearch();
            m_CurrentLevelEnemies++;
            m_LevelEnemies[m_CurrentLevelIndex]++;
        }
        m_CurrentLevelKills++;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0f)
        {
            m_PauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && m_PauseMenu.activeSelf)
        {
            ClearMenus();
        }
        if (Input.GetKeyDown(KeyCode.R) && Time.timeScale != 0f)
        {
            RestartLevel();
        }
    }

    public void Resume()
    {
        ClearMenus();
    }

    public void OnVictory()
    {
        float currentLevelTime = Time.time-m_CurrentLevelStartTime;
        if (currentLevelTime < m_BestLevelTimes[m_CurrentLevelIndex])
        {
            m_BestLevelTimes[m_CurrentLevelIndex] = currentLevelTime;
        }
        if (currentLevelTime < m_BestAllClearTimes[m_CurrentLevelIndex] && m_CurrentLevelKills == m_CurrentLevelEnemies)
        {
            m_BestAllClearTimes[m_CurrentLevelIndex] = currentLevelTime;
        }
        // Debug.Log(m_BestLevelTimes[m_CurrentLevelIndex]);
        // Debug.Log(m_BestAllClearTimes[m_CurrentLevelIndex]);
        // Debug.Log("Level complete!");
        // LoadNextLevel();

        // m_IsLevelComplete = true;
        m_LevelCompleteMenu.SetActive(true);
        m_StatsText.SetText(k_StatsFormatString, currentLevelTime, m_CurrentLevelKills, m_CurrentLevelEnemies, m_CurrentLevelDeaths);
        Time.timeScale = 0f;
        if (m_CurrentLevelIndex == m_LevelCount-1)
        {
            m_LevelCompleteMenu.transform.Find("NextLevelButton").gameObject.SetActive(false);
            m_LevelCompleteMenu.transform.Find("CreditsButton").gameObject.SetActive(true);
        }
        else
        {
            m_LevelCompleteMenu.transform.Find("NextLevelButton").gameObject.SetActive(true);
            m_LevelCompleteMenu.transform.Find("CreditsButton").gameObject.SetActive(false);
        }
    }

    public void LoadNextLevel()
    {
        ClearMenus();
        if (m_CurrentLevelIndex == m_LevelCount-1)
        {
            OnGameComplete();
        }
        else
        {
            SceneManager.LoadScene (m_CurrentLevelIndex + 2);
        }
    }

    public void MainMenu()
    {
        ClearMenus();
        m_CurrentLevelDeaths = 0;
        Time.timeScale = 0f;
        SceneManager.LoadScene(0);
    }

    public void OnGameComplete()
    {
        CreditsButton();
    }

    public void RestartLevel()
    {
        ClearMenus();
        SceneManager.LoadScene(m_CurrentLevelIndex+1);
    }

    public void CreditsButton()
    {
        ClearMenus();
        Time.timeScale = 0f;
        m_CreditsMenu.SetActive(true);
    }

    private void ClearMenus()
    {
        // m_IsPaused = false;
        // m_IsLevelComplete = false;
        m_PauseMenu.SetActive(false);
        m_LevelCompleteMenu.SetActive(false);
        m_CreditsMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
