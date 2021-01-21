using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]private GameObject pauseOverlay;

    private void Start()
    {
        pauseOverlay.SetActive(false);
    }

    public void tryAgain()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    
    public void pause()
    {
        pauseOverlay.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void unpause()
    {
        pauseOverlay.SetActive(false);
        Time.timeScale = 1;
    }
}
