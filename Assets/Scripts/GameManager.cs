using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem explosionEffect; // Trascina qui l'effetto esplosione
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject hudUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private AudioSource explosionAudio;

    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        ReturnToMainMenu();
    }

    private void Update()
    {
        // Se le vite sono finite (PANEL Game Over attivo)
        if (lives <= 0)
        {
            
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                PlayGame(); // resetta tutto e fa ripartire il gioco
            }
        }
    }

    public void PlayGame()
    {
        // spegne i menu e accende l'HUD
        mainMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        hudUI.SetActive(true);

        // Reset del Tempo
        Time.timeScale = 1f;

        // elimina tutti gli asteroidi vecchi rimasti in giro
        Asteroid[] asteroids = Object.FindObjectsByType<Asteroid>(FindObjectsSortMode.None);
        for (int i = 0; i < asteroids.Length; i++)
        {
            Destroy(asteroids[i].gameObject);
        }

        // rimette le vite a 3 e il punteggio a 0
        NewGame();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 0f;
        mainMenuUI.SetActive(true);
        hudUI.SetActive(false);
        gameOverUI.SetActive(false);
        player.gameObject.SetActive(false);

        // Pulisce SCENA quando torni al menu
        Asteroid[] asteroids = Object.FindObjectsByType<Asteroid>(FindObjectsSortMode.None);
        foreach (Asteroid a in asteroids) Destroy(a.gameObject);
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        Respawn();
    }

    private void SetScore(int s) { score = s; scoreText.text = s.ToString(); }
    private void SetLives(int l) { lives = l; livesText.text = l.ToString(); }

    public void QuitGame()
    {
        Debug.Log("il gioco si sta chiudendo...");
        Application.Quit();
    }

    private void Respawn()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
    }

    
    public void OnAsteroidDestroyed(Asteroid asteroid)
    {
        if (explosionEffect != null)
        {
            explosionEffect.transform.position = asteroid.transform.position;
            explosionEffect.Play();
        }

        if (asteroid.size < 0.7f) SetScore(score + 100);
        else if (asteroid.size < 1.4f) SetScore(score + 50);
        else SetScore(score + 25);
    }

    public void OnPlayerDeath(Player p)
    {
        p.gameObject.SetActive(false);

        
        if (explosionAudio != null)
        {
            explosionAudio.Play(); 
        }
        

        if (explosionEffect != null)
        {
            explosionEffect.transform.position = p.transform.position;
            explosionEffect.Play();
        }

        SetLives(lives - 1);
        if (lives <= 0) gameOverUI.SetActive(true);
        else Invoke(nameof(Respawn), 2f);
    }
}