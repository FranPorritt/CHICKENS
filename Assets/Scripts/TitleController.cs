using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class TitleController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_TitleChickenPrefab;
    [SerializeField]
    private int NumberOfChickens;

    private GameObject chickenObject;

    private List<GameObject> m_ChickenList;

    [SerializeField]
    private float m_MapExtentX;
    [SerializeField]
    private float m_MapExtentZ;

    private TitleWander wander;
    private GameObject wanderObject;

    public MouseLook m_MouseLook;

    // Use this for initialization
    void Awake()
    {
        Time.timeScale = 1f;

        m_ChickenList = new List<GameObject>();

        for (int chickenIndex = 0; chickenIndex < NumberOfChickens; chickenIndex++)
        {
            GameObject chickenObject = GameObject.Instantiate(m_TitleChickenPrefab, new Vector3(Random.Range(-m_MapExtentX, m_MapExtentX), 2f, Random.Range(-m_MapExtentZ, m_MapExtentZ)), Quaternion.identity);
            m_ChickenList.Add(chickenObject);
        }

        m_MouseLook.SetCursorLock(false);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Level 1 Start");
    }

    public void Continue()
    {
        //how do?
    }

    public void Back()
    {
        SceneManager.LoadScene("Title");
    }

    public void LevelMenu()
    {
        SceneManager.LoadScene("LevelMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LevelOne()
    {
        SceneManager.LoadScene("Level 1 Start");
    }

    public void LevelTwo()
    {
        SceneManager.LoadScene("Level 2 Start");
    }

    public void LevelThree()
    {
        SceneManager.LoadScene("Level 3 Start");
    }

    public void LevelFour()
    {
        SceneManager.LoadScene("Level 4 Start");
    }

    public void LevelFive()
    {
        SceneManager.LoadScene("Level 5 Start");
    }
}

