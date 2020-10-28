using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    
    Curagon curagon;
    
    ParticleStats[] particleStats;
    
    // UI
    TMP_Text happinessText;
    TMP_Text hungerText;
    TMP_Text staminaText;
    TMP_Text villageEXPText;
    TMP_Text villageLevelText;
    TMP_Text villageHealthText;

    Image happinessImage;
    Image hungerImage;
    Image staminaImage;
    Image villageEXPImage;
    Image villageHealthImage;

    public bool gamePaused;

    GameObject gamePanel;
    GameObject pausePanel;
    GameObject titlePanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init()
    {
        GetUIComponents();

        titlePanel.SetActive(true);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);

        gamePaused = true;
    }

    private void GetUIComponents()
    {
        Debug.Log("UIManager init start");
        gamePanel = transform.GetChild(0).gameObject;
        pausePanel = transform.GetChild(1).gameObject;
        titlePanel = transform.GetChild(2).gameObject;

        titlePanel.SetActive(true);
        gamePanel.SetActive(true);
        pausePanel.SetActive(true);

        happinessText = GameObject.Find("Happiness_NumberText").GetComponent<TMP_Text>();
        hungerText = GameObject.Find("Hunger_NumberText").GetComponent<TMP_Text>();
        staminaText = GameObject.Find("Stamina_NumberText").GetComponent<TMP_Text>();
        villageEXPText = GameObject.Find("Village-EXP_NumberText").GetComponent<TMP_Text>();
        villageLevelText = GameObject.Find("Village-EXP_LevelText").GetComponent<TMP_Text>();
        villageHealthText = GameObject.Find("Village-Health_NumberText").GetComponent<TMP_Text>();
        
        happinessImage = GameObject.Find("Happiness_BarFilling").GetComponent<Image>();
        hungerImage = GameObject.Find("Hunger_BarFilling").GetComponent<Image>();
        staminaImage = GameObject.Find("Stamina_BarFilling").GetComponent<Image>();
        villageEXPImage = GameObject.Find("Village-EXP_BarFilling").GetComponent<Image>();
        villageHealthImage = GameObject.Find("Village-Health_BarFilling").GetComponent<Image>();

        curagon = GameObject.FindGameObjectWithTag("Player").GetComponent<Curagon>();

        particleStats = new ParticleStats[3];
        particleStats[0] = GameObject.Find("Happiness-Particle-Effect_Stats").GetComponent<ParticleStats>();
        particleStats[1] = GameObject.Find("Hunger-Particle-Effect_Stats").GetComponent<ParticleStats>();
        particleStats[2] = GameObject.Find("Stamina-Particle-Effect_Stats").GetComponent<ParticleStats>();

        titlePanel.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);

        Debug.Log("UIManager init End");
    }

    public void UpdateStatsUI(float happiness, float hunger, float stamina)
    {
        happinessText.text = Mathf.CeilToInt(happiness * 100).ToString() + "%";
        hungerText.text = Mathf.CeilToInt(hunger * 100).ToString() + "%";
        staminaText.text = Mathf.CeilToInt(stamina * 100).ToString() + "%";
        
        happinessImage.fillAmount = happiness;
        happinessImage.color = GetStatusColor(happiness);
        
        hungerImage.fillAmount = hunger;
        hungerImage.color = GetStatusColor(hunger);
        
        staminaImage.fillAmount = stamina;
        staminaImage.color = GetStatusColor(stamina);
    }
    
    public void UpdateVillage(float health, float exp, int level)
    {
        villageHealthText.text = Mathf.CeilToInt(health * 100) + "%";
        villageEXPText.text = Mathf.CeilToInt(exp * 100) + "%";
        villageLevelText.text = "LEVEL: " + level;
        
        villageEXPImage.fillAmount = exp;
        villageEXPImage.color = GetStatusColor(exp);

        villageHealthImage.fillAmount = health;
        villageHealthImage.color = GetStatusColor(health);
    }

    private Color GetStatusColor(float procent)
    {
        return new Color(1 - procent, procent, 0);
        
        // if (procent > 0.7f)
        // {
        //     return new Color(0, 1, 0);
        // }
        // else if (procent > 0.5f)
        // {
        //     return new Color(1, 1, 0);
        // }
        // return new Color(1, 0, 0);
    }

    // Buttons onClick();
    public void Feed()
    {
        curagon.Feed(5f);
        particleStats[(int)Particle_Stats.Hunger].Play(Particle_Material.Plus);
        SoundManager.instance.ButtonSound();
    }

    public void Play()
    {
        curagon.Play(5f);
        particleStats[(int)Particle_Stats.Happiness].Play(Particle_Material.Plus);
        SoundManager.instance.ButtonSound();
    }

    public void Work()
    {
        curagon.Work();
        particleStats[(int)Particle_Stats.Stamina].Play(Particle_Material.Minus);
        SoundManager.instance.ButtonSound();
    }

    public void Sleep()
    {
        curagon.Sleep(5f);
        particleStats[(int)Particle_Stats.Stamina].Play(Particle_Material.Plus);
        SoundManager.instance.ButtonSound();
    }

    public void Clean()
    {
        curagon.Clean();
        particleStats[(int)Particle_Stats.Happiness].Play(Particle_Material.Plus);
        SoundManager.instance.ButtonSound();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Village.instance.Restart();
        curagon.Restart();
        GetUIComponents();
        SoundManager.instance.ButtonSound();
        Resume();
    }

    public void PauseButton()
    {
        gamePanel.SetActive(false);
        pausePanel.SetActive(true);

        gamePaused = true;
    }

    public void Resume()
    {
        gamePanel.SetActive(true);
        pausePanel.SetActive(false);

        gamePaused = false;
    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        //Application.Quit();
    }

    public void SetCuragon(Curagon newCuragon)
    {
        curagon = newCuragon;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Village.instance.Restart();
        curagon.Restart();
        SoundManager.instance.ButtonSound();
        
        Init();

        titlePanel.SetActive(false);
        gamePanel.SetActive(true);
        gamePaused = false;
    }

    public void HowToPlay()
    {

    }
}

public enum Particle_Stats : int
{
    Happiness = 0,
    Hunger,
    Stamina
}