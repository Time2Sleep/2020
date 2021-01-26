using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        FindObjectOfType<GameManager>().newGame();
        Time.timeScale = 1;
    }
    
    public void pause()
    {
        pauseOverlay.GetComponent<Image>().color = Camera.main.backgroundColor;
        pauseOverlay.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void unpause()
    {
        pauseOverlay.SetActive(false);
        Time.timeScale = 1;
    }
}
