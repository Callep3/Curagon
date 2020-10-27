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
    [SerializeField] private float poopOnFloor = 1f;
    private Animator animator;

    public float baseHappinessReductionRate;
    public float baseHungerReductionRate;
    public float baseStaminaReductionRate;

    void Awake()
    {//Get the animator
        animator = GetComponent<Animator>();
    }

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
        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
        happiness = Mathf.Clamp(happiness, 0f, maxHappiness);
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);

        UIManager.instance.UpdateStatsUI( happiness / maxHappiness,
                                            hunger / maxHunger,
                                            stamina / maxStamina);
    }
    public void Feed(float amount)
    {
        hunger += amount;

        poop += Mathf.FloorToInt(amount / 2);
        if (poop >= maxPoop)
        {
            Poop();
        }

        animator.SetTrigger("Eat");

        Village.instance.SetWork(false);
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
        Village.instance.SetWork(false);
    }

    public void Work()
    {
        Village.instance.SetWork(true);
    }

    public void Sleep(float amount)
    {
        stamina += amount;
        if (stamina >= maxStamina)
        {
            stamina = maxStamina;
        }
        Village.instance.SetWork(false);
    }

    public void Clean()
    {
        poopOnFloor = 1.0f;
    }
}
