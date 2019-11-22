using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class GameController : MonoBehaviour
{
    private enum State
    {
        Playing,
        ChickensHome,
        TimesUp,
        Paused,
    }

    private enum PickUpState
    {
        NotSpawned,
        SpawnedNotActivated,
        SpawnedActivated,
        Used,
    }

    private State m_CurrentState;
    private PickUpState m_PickUpState;

    public bool isPlaying = true;

    [SerializeField]
    private GameObject m_ChickenPrefab;
    [SerializeField]
    private int NumberOfChickens;
    [SerializeField]
    private GameObject m_EggPrefab;
    [SerializeField]
    private int NumberOfEggs;

    private GameObject chickenObject;
    private GameObject eggObject;
    
    private List<GameObject> m_ChickenList;
    private List<GameObject> m_EggList;

    [SerializeField]
    private float m_MapExtentX;
    [SerializeField]
    private float m_MapExtentZ;

    private Wander wander;
    private GameObject wanderObject;

    public Text chickenText;
    private int chickenNumber;

    public Text timerText;
    [SerializeField]
    public float timer;

    public GameObject chickenHomeText;
    public GameObject timesUpText;
    [SerializeField]
    private Text TimeRemainingText;
    [SerializeField]
    private Text ChickensLeftText;

    public MouseLook m_MouseLook;

    [SerializeField]
    private GameObject m_pauseMenuUI;

    [SerializeField]
    private GameObject FPSController;

    private PickUp m_PickUp;
    private bool isEggSpawned = false;

    //Level Unlock Code
    public LevelUnlock m_LevelUnlock;
    public bool isLevelComplete = false;

    // Use this for initialization
    void Awake ()
    {
        m_ChickenList = new List<GameObject>();

        for (int chickenIndex = 0; chickenIndex < NumberOfChickens; chickenIndex++)
        {
            GameObject chickenObject = GameObject.Instantiate(m_ChickenPrefab,new Vector3(Random.Range(-m_MapExtentX, m_MapExtentX), 0f, Random.Range(-m_MapExtentZ, m_MapExtentZ)), Quaternion.identity);
            m_ChickenList.Add(chickenObject);
        }
        
        UpdateScore();
        UpdateTimer();
        m_CurrentState = State.Playing;
    }

    private void Update()
    {
        if ((timer <= 0.0f) && (m_CurrentState != State.ChickensHome))
        {
            m_CurrentState = State.TimesUp;
        }

        if (timer <= 5.0f)
        {
            timerText.color = Color.red;
        }

        if ((NumberOfChickens == 0) && (m_CurrentState != State.TimesUp))
        {
            m_CurrentState = State.ChickensHome;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_CurrentState == State.Paused)
            {
                Resume();
            }
            else if (m_CurrentState == State.Playing)
            {
                m_CurrentState = State.Paused;
            }
            else
            {
                return;
            }
        }

        if (m_CurrentState != State.Playing)
        {
            FPSController.GetComponent<FirstPersonController>().enabled = false;
            isPlaying = false;
            Time.timeScale = 0f;
        }

        switch (m_CurrentState)
        {
            case State.Playing:
                {
                    FPSController.GetComponent<FirstPersonController>().enabled = true;
                    isPlaying = true;
                    Time.timeScale = 1f;
                    break;
                }

            case State.ChickensHome:
                {
                    ChickenHome();
                    isLevelComplete = true;
                    break;
                }

            case State.TimesUp:
                {
                    TimesUp();
                    break;
                }

            case State.Paused:
                {
                    Paused();
                    break;
                }

            default:
                {
                    break;
                }
        }

        // Pick up state machine, controls timer and egg spawn
        switch (m_PickUpState)
        {
            case PickUpState.NotSpawned:
                {
                    UpdateTimer();
                    break;
                }

            case PickUpState.SpawnedNotActivated:
                {
                    UpdateTimer();
                    break;
                }

            case PickUpState.SpawnedActivated:
                {
                    break;
                }

            case PickUpState.Used:
                {
                    break;
                }

            default:
                {
                    break;
                }
        }

        if (timer >= 15.00f)
        {
            m_PickUpState = PickUpState.NotSpawned;
        }

        if (!isEggSpawned)
        {
            if (timer <= 15.00f)
            {
                EggSpawn();
            }
        }
        
        if (isEggSpawned)
        {
            TimerEffect();
            if (GameObject.Find("Egg(Clone)").GetComponent<PickUp>().isPickUp)
            {
                m_PickUpState = PickUpState.SpawnedActivated;
            }
            else
            {
                m_PickUpState = PickUpState.SpawnedNotActivated;
            }
        }
        
        //TimerEffect();
    }

    void UpdateTimer()
    {
        if (isPlaying == true)
        {
            timer -= Time.deltaTime;
        }
            timerText.text = "Time: 00:" + timer.ToString("00");
    }

    private void TimerEffect()
    {
        if (GameObject.Find("Egg(Clone)").GetComponent<PickUp>().isPickUp)
        {
            timerText.color = Color.blue;
        }
        else
        {
            timerText.color = Color.white;
        }
    }

    private void EggSpawn()
    {
        m_EggList = new List<GameObject>();

        for (int eggIndex = 0; eggIndex < NumberOfEggs; eggIndex++)
        {
            GameObject eggObject = GameObject.Instantiate(m_EggPrefab, new Vector3(Random.Range(-m_MapExtentX, m_MapExtentX), 1.00f, Random.Range(-m_MapExtentZ, m_MapExtentZ)), Quaternion.identity);
            m_EggList.Add(eggObject);
        }

        isEggSpawned = true;
        m_PickUpState = PickUpState.SpawnedNotActivated;
    }

    private void PickUpCheck()
    {
        m_PickUp = GameObject.Find("Egg(Clone)").GetComponent<PickUp>();

        UpdateTimer();
    }

    private void OnEnable()
    {
        ChickenHouse.OnChickenEnterHome += OnChickenEnterHome;
    }

    private void OnDisable()
    {
        ChickenHouse.OnChickenEnterHome -= OnChickenEnterHome;
    }

    void UpdateScore ()
    {
        chickenText.text = "Chickens: " + NumberOfChickens;
    }

    void OnChickenEnterHome()
    {
        NumberOfChickens--;
        UpdateScore();

        Debug.Log("Chicken is home! Number of chickens: " + NumberOfChickens);
    }

    void ChickenHome()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPlaying = false;
        chickenText.enabled = false;
        timerText.enabled = false;
        chickenHomeText.SetActive(true);
        TimeRemainingText.text = "Time Remaining: " + timer.ToString("00") + " secs";
    }

    void TimesUp()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPlaying = false;
        timesUpText.SetActive(true);
        timer = 0.00f;
        timerText.text = "Time: " + timer;
        ChickensLeftText.text = "Chickens Remaining: " + NumberOfChickens;
        chickenText.enabled = false;
        timerText.enabled = false;
    }

    public void Resume()
    {
        m_pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        m_CurrentState = State.Playing;
    }

    void Paused()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        m_pauseMenuUI.SetActive(false);
        m_CurrentState = State.Playing;
        Scene level = SceneManager.GetActiveScene();
        SceneManager.LoadScene(level.name);
    }

    public void Menu()
    {
        m_pauseMenuUI.SetActive(false);
        m_CurrentState = State.Playing;
        SceneManager.LoadScene("Title");
    }

    public void Quit()
    {
        m_pauseMenuUI.SetActive(false);
        Application.Quit();
    }

    public void NextLevel()
    {
        if (m_LevelUnlock.isNextLevelUnlocked)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
