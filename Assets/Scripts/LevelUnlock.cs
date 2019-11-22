using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUnlock : MonoBehaviour
{
    public bool isNextLevelUnlocked = false;

    public GameController m_GameController;
    

    // Update is called once per frame
    void Update ()
    {
		if (m_GameController.isLevelComplete)
        {
            isNextLevelUnlocked = true;
        }
	}

    void UnlockLevels()
    {
        
    }
}
