using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject m_MainMenu;
    [SerializeField] GameObject m_GameplayHUD;
    [SerializeField] GameObject m_PauseMenu;
    [SerializeField] GameObject m_GameOverMenu;

    private bool m_IsPaused = false;

    [SerializeField] NextBlock m_NextBlock;

    // Start is called before the first frame update
    void Start()
    {
        m_MainMenu.SetActive(true);    
        m_GameplayHUD.SetActive(false);
        m_PauseMenu.SetActive(false);
        m_GameOverMenu.SetActive(false);
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !m_GameOverMenu.activeSelf && !m_GameOverMenu.activeSelf) 
        {
            Pause();
        }

        if(m_NextBlock.m_GameOver)
        {
            Time.timeScale = 0f;
            m_GameOverMenu.SetActive(true);
            m_GameplayHUD.SetActive(false);
        }
    }

    public void Pause()
    {
        if(m_IsPaused) 
        {
            Time.timeScale = 1.0f;
            m_IsPaused = false;
            m_GameplayHUD.SetActive(true);
            m_PauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0.0f;
            m_IsPaused = true;
            m_GameplayHUD.SetActive(false);
            m_PauseMenu.SetActive(true);
        }
    }

    public void Play()
    {
        m_MainMenu.SetActive(false);
        m_GameplayHUD.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Return()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
