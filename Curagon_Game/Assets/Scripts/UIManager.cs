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

    public void UpdateStatsUI(int happiness, int hunger, int stamina)
    {
        happinessText.text = "happiness: " + happiness;
        hungerText.text = "hunger: " + hunger;
        staminaText.text = "stamina: " + stamina;
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
        curagon.Work(5f);
    }

    public void Sleep()
    {
        curagon.Sleep(5f);
    }

    public void Clean()
    {
        curagon.Clean();
    }

    public void UpdateVillage(int health)
    {
        villageText.text = "Village: " + health + "%";
    }
}
