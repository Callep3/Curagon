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
        Restart();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();
    }
    private void UpdateStats()
    {
        UpdateHunger();
        UpdateHappiness();
        UpdateStamina();

        UIManager.instance.UpdateStatsUI( happiness / maxHappiness,
                                            hunger / maxHunger,
                                            stamina / maxStamina);
    }

    private void UpdateHunger()
    {
        float staminaScale = 1;
        float staminaProcent = stamina / maxStamina;
        if (staminaProcent <= 0.4)
        {
            staminaScale = 2;
        }

        hunger -= Time.deltaTime * baseHungerReductionRate * staminaScale;

        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
    }


    private void UpdateHappiness()
    {
        float hungerScale = 1;
        float hungerProcent = hunger / maxHunger;
        if (hungerProcent <= 0.5)
        {
            hungerScale = 2;
        }
        else if (hungerProcent <= 0.25)
        {
            hungerScale = 3;
        }

        happiness -= Time.deltaTime * baseHappinessReductionRate * poopOnFloor * hungerScale;

        happiness = Mathf.Clamp(happiness, 0f, maxHappiness);
    }

    private void UpdateStamina()
    {
        float workingScale = 1;
        if (Village.instance.working)
        {
            workingScale = 3.0f;
        }

        stamina -= Time.deltaTime * baseStaminaReductionRate * workingScale;

        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
    }

    public void Feed(float amount)
    {
        hunger += amount;

        poop += Mathf.FloorToInt(amount / 2);
        if (poop >= maxPoop)
        {
            Poop();
        }
        ClearAnimation();
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
        ClearAnimation();
        animator.SetBool("Play", true);

        Village.instance.SetWork(false);
    }

    public void Work()
    {
        ClearAnimation();
        animator.SetBool("Work", true);

        Village.instance.SetWork(true);
    }

    public void Sleep(float amount)
    {
        stamina += amount;
        if (stamina >= maxStamina)
        {
            stamina = maxStamina;
        }
        ClearAnimation();
        animator.SetBool("Sleep", true);

        Village.instance.SetWork(false);
    }

    public void Clean()
    {
        poopOnFloor = 1.0f;
    }

    public float GetWorkingCondition()
    {
        float workingConstant = 1;
        float staminaProcent = stamina / maxStamina;
        float happinessProcent = happiness / maxHappiness;
        float hungerProcent = hunger / maxHunger;

        if (staminaProcent <= 0.2)
        {
            return 0;
        }
        else if (staminaProcent >= 0.8)
        {
            workingConstant *= 1.5f;
        }

        if (happinessProcent <= 0.3)
        {
            workingConstant *= 0.5f;
        }
        else if (happinessProcent >= 0.8)
        {
            workingConstant *= 1.5f;
        }

        if (hungerProcent <= 0.3)
        {
            workingConstant *= 0.5f;
        }
        else if (hungerProcent >= 0.8)
        {
            workingConstant *= 1.5f;
        }

        return workingConstant;
    }

    public void Restart()
    {
        happiness = maxHappiness;
        hunger = maxHunger;
        stamina = maxStamina;
        poopOnFloor = 1;
        poop = 0;
    }

    void ClearAnimation()
    {
        animator.SetBool("Play", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Sleep", false);
    }
}
