using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curagon : MonoBehaviour
{
    public float maxHappiness = 100f;
    public float happiness;
    public float maxHunger = 100f;
    public float hunger;
    public float maxStamina = 100f;
    public float stamina;
    public int maxPoop = 10;
    public int poop;
    [SerializeField]
    private float poopOnFloor = 1f;

    public float baseHappinessReductionRate;
    public float baseHungerReductionRate;
    public float baseStaminaReductionRate;


    // Start is called before the first frame update
    void Start()
    {
        happiness = maxHappiness;
        hunger = maxHunger;
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        hunger -= Time.deltaTime * baseHungerReductionRate;
        happiness -= Time.deltaTime * baseHappinessReductionRate * poopOnFloor;
        stamina -= Time.deltaTime * baseStaminaReductionRate;
        UpdateStats();
    }
    private void UpdateStats()
    {
        if (happiness >= maxHappiness)
        {
            happiness = maxHappiness;
        }
        if (stamina >= maxStamina)
        {
            stamina = maxStamina;
        }
        UIManager.instance.UpdateStatsUI((int)happiness, (int)hunger, (int)stamina);
    }
    public void Feed(float amount)
    {
        hunger += amount;
        if (hunger >= maxHunger)
        {
            hunger = maxHunger;
        }
        poop += Mathf.FloorToInt(amount / 2);
        if (poop >= maxPoop)
        {
            Poop();
        }
    }

    private void Poop()
    {
        poop = 0;
        poopOnFloor = 2.0f;
    }

    public void Play(float amount)
    {
        happiness += amount;
        if (happiness >= maxHappiness)
        {
            happiness = maxHappiness;
        }
    }

    public void Work(float amount)
    {
        
    }

    public void Sleep(float amount)
    {
        stamina += amount;
        if (stamina >= maxStamina)
        {
            stamina = maxStamina;
        }
    }

    public void Clean()
    {
        poopOnFloor = 1.0f;
    }
}
