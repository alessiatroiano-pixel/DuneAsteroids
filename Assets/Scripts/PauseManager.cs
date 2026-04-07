using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    [SerializeField] private Slider volumeSlider;

    
    void Update()
    {
        // Se premi ESC, attiva o disattiva la pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    
    public void TogglePause()
    {
        bool isPaused = !pauseMenuUI.activeSelf;
        pauseMenuUI.SetActive(isPaused);

        // Ferma o fa ripartire il tempo
        Time.timeScale = isPaused ? 0f : 1f;

        // Mostra o nascondi il mouse per poter usare lo slider
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void Resume() => TogglePause();

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void BackToMainMenu()
    {
        pauseMenuUI.SetActive(false);
        GameManager.Instance.ReturnToMainMenu();
    }
}