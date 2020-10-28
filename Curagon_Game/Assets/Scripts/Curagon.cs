using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curagon : MonoBehaviour
{
    Animator animator;
    [SerializeField] GameObject ball;
    AudioClip[] audioClips;
    
    float happiness;
    float hunger;
    float stamina;
    const float maxHappiness = 100f;
    const float maxHunger = 100f;
    const float maxStamina = 100f;
    
    int poop;
    const int maxPoop = 10;
    
    int numberOfApples;
    const float appleTimeSeconds = 5f;
    float appleTimer;

    float baseHappinessReductionRate;
    float baseHungerReductionRate;
    float baseStaminaReductionRate;
    float poopOnFloor;
    float poopScale;
    
    void Awake()
    {
        Init();
    }

    void Update()
    {
        AddApple();
        UpdateStats();
    }
    
    void Init()
    {
        GetAllComponents();
        
        happiness = maxHappiness;
        hunger = maxHunger;
        stamina = maxStamina;
        poop = 0;
        numberOfApples = 5;

        appleTimer = appleTimeSeconds;

        baseHappinessReductionRate = 1f;
        baseHungerReductionRate = 1.25f;
        baseStaminaReductionRate = 1.5f;
        poopOnFloor = 1f;


    }

    void GetAllComponents()
    {
        animator = GetComponent<Animator>();

        audioClips = new AudioClip[6];
        audioClips[0] = Resources.Load<AudioClip>("Audio/SFX/Curagon/kitten-meowing_0");
        audioClips[1] = Resources.Load<AudioClip>("Audio/SFX/Curagon/kitten-meowing_1");
        audioClips[2] = Resources.Load<AudioClip>("Audio/SFX/Curagon/Broom");
        audioClips[3] = Resources.Load<AudioClip>("Audio/SFX/Curagon/kitten-meowing_3");
        audioClips[4] = Resources.Load<AudioClip>("Audio/SFX/Curagon/cute-snore");
        audioClips[5] = Resources.Load<AudioClip>("Audio/SFX/Curagon/Poop");
    }
    
    private void UpdateStats()
    {
        UpdateHunger();
        UpdateHappiness();
        UpdateStamina();
        UpdatePoop();

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

    private void UpdatePoop()
    {
        poopScale += Time.deltaTime;

        if (poopScale >= 10)
        {
            poop += 1;
            poopScale = 0;

            if (poop >= maxPoop)
            {
                Poop();
            }
        }
    }

    public void Feed(float amount)
    {
        if(numberOfApples > 0)
        {
            numberOfApples -= 1;
            hunger += amount;

            poop += Mathf.FloorToInt(amount / 2);
            if (poop >= maxPoop)
            {
                Poop();
            }
            else
            {
                SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Eat]);
            }

            ClearAnimation();
            animator.SetTrigger("Eat");

            Village.instance.SetWork(false);
        }
    }

    private void Poop()
    {
        poop = 0;
        poopOnFloor = 2.0f;
        SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Poop]);
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
        ball.SetActive(true);

        Village.instance.SetWork(false);
        SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Play]);
    }

    public void Work()
    {
        ClearAnimation();
        animator.SetBool("Work", true);

        Village.instance.SetWork(true);
        SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Work]);
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
        SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Sleep]);
    }

    public void Clean()
    {
        poopOnFloor = 1.0f;
        SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Clean]);
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
        Init();
    }

    void ClearAnimation()
    {
        animator.SetBool("Play", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Sleep", false);
        
        ball.SetActive(false);
    }

    void AddApple()
    {
        appleTimer -= Time.deltaTime;
        if (appleTimer <= 0.0f)
        {
            appleTimer = appleTimeSeconds;
            numberOfApples++;
            // Debug.Log($"numOfApples: {numberOfApples}");
        }
    }
}
public enum Curagon_Sounds : int
{
    Eat,
    Work,
    Clean,
    Play,
    Sleep,
    Poop
}
