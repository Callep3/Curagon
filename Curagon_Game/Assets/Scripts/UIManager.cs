using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    [SerializeField]
    TMP_Text happinessText;
    [SerializeField]
    TMP_Text hungerText;
    [SerializeField]
    TMP_Text staminaText;
    [SerializeField]
    TMP_Text villageText;
    [SerializeField]
    Curagon curagon;
    [SerializeField]
    Image happinessImage;
    [SerializeField]
    Image villageProgressImage;
    //[SerializeField]
    //Image hungerImage;
    //[SerializeField]
    //Image staminaImage;

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

    public void UpdateStatsUI(float happinessProcent, int hunger, int stamina)
    {
        happinessText.text = Mathf.CeilToInt(happinessProcent * 100).ToString();
        hungerText.text = "hunger: " + hunger;
        staminaText.text = "stamina: " + stamina;
        happinessImage.fillAmount = happinessProcent;
        happinessImage.color = GetStatusColor(happinessProcent);
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
    }

    public void Play()
    {
        curagon.Play(5f);
    }

    public void Work()
    {
        curagon.Work();
    }

    public void Sleep()
    {
        curagon.Sleep(5f);
    }

    public void Clean()
    {
        curagon.Clean();
    }

    public void UpdateVillage(float health, float exp)
    {
        villageText.text = "Village: " + Mathf.CeilToInt(health) + "%";
        villageProgressImage.fillAmount = exp;
        villageProgressImage.color = GetStatusColor(exp);
    }
}
