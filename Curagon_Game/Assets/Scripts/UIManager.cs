using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    [SerializeField] TMP_Text happinessText;
    [SerializeField] TMP_Text hungerText;
    [SerializeField] TMP_Text staminaText;
    [SerializeField] TMP_Text villageEXPText;
    [SerializeField] TMP_Text villageLevelText;
    [SerializeField] TMP_Text villageHealthText;
    
    [SerializeField] Curagon curagon;
    
    [SerializeField] Image happinessImage;
    [SerializeField] Image hungerImage;
    [SerializeField] Image staminaImage;
    [SerializeField] Image villageProgressImage;
    [SerializeField] Image villageHealthImage;

    [SerializeField] private ParticleStats[] particleStats;

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

    private Color GetStatusColor(float procent)
    {
        return new Color(1 - procent, procent, 0);
        
        if (procent > 0.7f)
        {
            return new Color(0, 1, 0);
        }
        else if (procent > 0.5f)
        {
            return new Color(1, 1, 0);
        }
        return new Color(1, 0, 0);
    }

    public void Feed()
    {
        curagon.Feed(5f);
        particleStats[(int)Particle_Stats.Hunger].Play(Particle_Material.Plus);
    }

    public void Play()
    {
        curagon.Play(5f);
        particleStats[(int)Particle_Stats.Happiness].Play(Particle_Material.Plus);
    }

    public void Work()
    {
        curagon.Work();
        particleStats[(int)Particle_Stats.Stamina].Play(Particle_Material.Minus);
    }

    public void Sleep()
    {
        curagon.Sleep(5f);
        particleStats[(int)Particle_Stats.Stamina].Play(Particle_Material.Plus);
    }

    public void Clean()
    {
        curagon.Clean();
        particleStats[(int)Particle_Stats.Happiness].Play(Particle_Material.Plus);
    }

    public void UpdateVillage(float health, float exp, int level)
    {
        villageHealthText.text = Mathf.CeilToInt(health * 100) + "%";
        villageEXPText.text = Mathf.CeilToInt(exp * 100) + "%";
        villageLevelText.text = "LEVEL: " + level;
        
        villageProgressImage.fillAmount = exp;
        villageProgressImage.color = GetStatusColor(exp);

        villageHealthImage.fillAmount = health;
        villageHealthImage.color = GetStatusColor(health);
    }
}

public enum Particle_Stats : int
{
    Happiness = 0,
    Hunger,
    Stamina
}
