using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private LouiseController player;

    public static PauseManager Instance;
    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }
    private void Start()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        IsPaused = false;
    }
    public void TogglePause()
    {
        if (IsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;

        pausePanel.SetActive(true);

        player.SetController(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
        
        AudioManager.Instance.PlayClickSfx();
        AudioManager.Instance.PauseGameMusic();
    }
    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        player.SetController(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        AudioManager.Instance.PlayClickSfx();
        AudioManager.Instance.ResumeGameMusic();
    }
    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);

        AudioManager.Instance.PlayClickSfx();
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);

        AudioManager.Instance.PlayClickSfx();
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlayClickSfx();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame()
    {
        AudioManager.Instance.PlayClickSfx();
        Application.Quit();
    }
}
