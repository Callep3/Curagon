using System;
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
    TMP_Text appleCountText;
    TMP_Text chickenCountText;

    //Fill bars
    Image happinessImage;
    Image hungerImage;
    Image staminaImage;
    Image villageEXPImage;
    Image villageHealthImage;

    [SerializeField] private bool showPercentNumbers;
    public bool gamePaused;

    GameObject gamePanel;
    GameObject pausePanel;
    GameObject titlePanel;
    GameObject gameOverPanel;
    GameObject foodPanel;
    GameObject howToPlay;

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
        
        GetUIComponents();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        titlePanel.SetActive(true);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        howToPlay.SetActive(false);
        foodPanel.SetActive(false);

        gamePaused = true;
    }

    private void GetUIComponents()
    {
        gamePanel = transform.GetChild(0).gameObject;
        pausePanel = transform.GetChild(1).gameObject;
        titlePanel = transform.GetChild(2).gameObject;
        gameOverPanel = transform.GetChild(3).gameObject;
        howToPlay = transform.GetChild(2).GetChild(2).gameObject;

        titlePanel.SetActive(true);
        gamePanel.SetActive(true);
        pausePanel.SetActive(true);
        gameOverPanel.SetActive(true);

        foodPanel = GameObject.Find("FoodAlternativeButtons").gameObject;

        happinessText = GameObject.Find("Happiness_NumberText").GetComponent<TMP_Text>();
        hungerText = GameObject.Find("Hunger_NumberText").GetComponent<TMP_Text>();
        staminaText = GameObject.Find("Stamina_NumberText").GetComponent<TMP_Text>();
        villageEXPText = GameObject.Find("Village-EXP_NumberText").GetComponent<TMP_Text>();
        villageLevelText = GameObject.Find("Village-EXP_LevelText").GetComponent<TMP_Text>();
        villageHealthText = GameObject.Find("Village-Health_NumberText").GetComponent<TMP_Text>();
        appleCountText = foodPanel.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        chickenCountText = foodPanel.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();

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
    }

    public void UpdateStatsUI(float happiness, float hunger, float stamina)
    {
        if (showPercentNumbers)
        {
            //Stats text
            happinessText.text = Mathf.CeilToInt(happiness * 100) + "%";
            hungerText.text = Mathf.CeilToInt(hunger * 100) + "%";
            staminaText.text = Mathf.CeilToInt(stamina * 100) + "%";
        }
        else
        {
            happinessText.text = "";
            hungerText.text = "";
            staminaText.text = "";
        }

        //Happiness bar
        happinessImage.fillAmount = happiness;
        happinessImage.color = GetStatusColor(happiness);

        //Hunger bar
        hungerImage.fillAmount = hunger;
        hungerImage.color = GetStatusColor(hunger);
        
        //Stamina bar
        staminaImage.fillAmount = stamina;
        staminaImage.color = GetStatusColor(stamina);
    }

    public void UpdateFoodStorageUI(int apples,int chickens)
    {
        appleCountText.text = apples.ToString();
        chickenCountText.text = chickens.ToString();
    }


    public void UpdateVillage(float health, float exp, int level)
    {
        if (showPercentNumbers)
        {
            villageHealthText.text = Mathf.CeilToInt(health * 100) + "%";
            villageEXPText.text = Mathf.CeilToInt(exp * 100) + "%";
        }
        else
        {
            villageHealthText.text = "";
            villageEXPText.text = "";
        }

        villageLevelText.text = "LEVEL: " + level;
        
        //Exp bar
        villageEXPImage.fillAmount = exp;
        villageEXPImage.color = GetStatusColor(exp);

        //Health bar
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

    public void AppleButton()
    {
        Feed(5, true);
    }

    public void ChickenButton()
    {
        Feed(10, false);
    }

    public void OpenFoodPanel()
    {
        foodPanel.SetActive(!foodPanel.activeInHierarchy);
    }

    // Buttons onClick();
    public void Feed(float amount, bool isApple)
    {
        curagon.Feed(amount, isApple);
        particleStats[(int)Particle_Stats.Hunger].Play(Particle_Material.Plus);
        SoundManager.instance.ButtonSound();
    }

    public void Play()
    {
        curagon.Play();
        particleStats[(int)Particle_Stats.Happiness].Play(Particle_Material.Plus);
        SoundManager.instance.ButtonSound();
        foodPanel.SetActive(false);
    }

    public void Work()
    {
        curagon.Work();
        particleStats[(int)Particle_Stats.Stamina].Play(Particle_Material.Minus);
        SoundManager.instance.ButtonSound();
        foodPanel.SetActive(false);
    }

    public void Sleep()
    {
        curagon.Sleep();
        particleStats[(int)Particle_Stats.Stamina].Play(Particle_Material.Plus);
        SoundManager.instance.ButtonSound();
        foodPanel.SetActive(false);
    }

    public void Clean()
    {
        curagon.Clean();
        particleStats[(int)Particle_Stats.Happiness].Play(Particle_Material.Plus);
        SoundManager.instance.ButtonSound();
        foodPanel.SetActive(false);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        Village.instance.Restart();
        // curagon.Restart();

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
        gameOverPanel.SetActive(false);
        foodPanel.SetActive(false);

        gamePaused = false;
    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void SetCuragon(Curagon newCuragon)
    {
        curagon = newCuragon;
    }

    public void StartGame()
    {
        Village.instance.Restart();
        // curagon.Restart();
        
        SoundManager.instance.ButtonSound();
        
        Init();

        titlePanel.SetActive(false);
        gamePanel.SetActive(true);
        gamePaused = false;
    }

    public void HowToPlay()
    {
        howToPlay.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlay.SetActive(false);
    }

    public void GameOver()
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gamePaused = true;
    }

    public void MainMenu()
    {
        titlePanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }
}

public enum Particle_Stats : int
{
    Happiness, // = 0
    Hunger, // = 1
    Stamina // = 2
}