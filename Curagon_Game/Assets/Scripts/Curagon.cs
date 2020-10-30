using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curagon : MonoBehaviour
{
    protected Animator animator;

    ParticleSystem smokeParticle;

    GameObject ball; // Curagon's ball playing animation

    GameObject applePrefab;
    GameObject chickenPrefab;
    Transform foodSpawnTransform;

    GameObject poopPrefab;
    Transform poopSpawnTransform_01;
    Transform poopSpawnTransform_02;
    Transform poopSpawnTransform_03;

    GameObject[] poopInGame;

    AudioClip[] audioClips;
    
    protected float happiness;
    protected float hunger;
    protected float stamina;
    protected const float maxHappiness = 100f;
    protected const float maxHunger = 100f;
    protected const float maxStamina = 100f;
    
    // When poopStored reaches maxPoopStored [Make a Poop();]
    int poopStored;
    const int maxPoopStored = 10;
    
    int numberOfApples;
    const int maxNumberOfApples = 10;
    float appleTimer;
    const float appleTimeSeconds = 5f;

    int numberOfChickens;
    const int maxNumberOfChickens = 5;
    float chickenTimer;
    const float chickenTimeSeconds = 10f;

    float poopTimer;
    const float poopTimeSeconds = 10f;
    
    protected bool sleeping;
    protected bool playing;

    protected float baseHappinessReductionRate;
    protected float baseHungerReductionRate;
    protected float baseStaminaReductionRate;
    protected float staminaSleepingIncrease;
    protected float poopOnFloor;

    void Awake()
    {
        GetAllComponents();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (!UIManager.instance.gamePaused)
        {
            AddApple();
            AddChicken();
            UpdateStats();
        }
    }
    
    void GetAllComponents()
    {
        animator = GetComponent<Animator>();

        ball = transform.Find("GFX").Find("Curagon-Ball").gameObject;

        poopPrefab = Resources.Load<GameObject>("Prefabs/Poop");
        applePrefab = Resources.Load<GameObject>("Prefabs/Apple");
        chickenPrefab = Resources.Load<GameObject>("Prefabs/Chicken");

        foodSpawnTransform = GameObject.Find("FoodSpawnLocation").transform;
        poopSpawnTransform_01 = GameObject.Find("PoopSpawnLocation_01").transform;
        poopSpawnTransform_02 = GameObject.Find("PoopSpawnLocation_02").transform;
        poopSpawnTransform_03 = GameObject.Find("PoopSpawnLocation_03").transform;

        smokeParticle = GameObject.Find("CuragonSmokeParticle").GetComponent<ParticleSystem>();

        string clipPath = "Audio/SFX/Curagon/";
        audioClips = new AudioClip[6];
        audioClips[0] = Resources.Load<AudioClip>(clipPath + "Curagon-Eat");
        audioClips[1] = Resources.Load<AudioClip>(clipPath + "Curagon-Work");
        audioClips[2] = Resources.Load<AudioClip>(clipPath + "Curagon-Clean");
        audioClips[3] = Resources.Load<AudioClip>(clipPath + "Curagon-Play");
        audioClips[4] = Resources.Load<AudioClip>(clipPath + "Curagon-Sleep");
        audioClips[5] = Resources.Load<AudioClip>(clipPath + "Curagon-Poop");
    }
    
    // Initialize
    protected virtual void Init()
    {
        happiness = maxHappiness;
        hunger = maxHunger;
        stamina = maxStamina;
        poopStored = 0;
        numberOfApples = 10;

        appleTimer = appleTimeSeconds;
        poopTimer = poopTimeSeconds; 

        baseHappinessReductionRate = 1f;
        baseHungerReductionRate = 1.25f;
        baseStaminaReductionRate = 1.5f;
        staminaSleepingIncrease = 5;
        poopOnFloor = 1f;
        
        poopInGame = new GameObject[3];
        
        UIManager.instance.SetCuragon(this);
        Village.instance.SetCuragon(this);
    }

    private void UpdateStats()
    {
        UpdateHappiness();
        UpdateHunger();
        UpdateStamina();
        UpdatePoop();
        
        UIManager.instance.UpdateStatsUI( happiness / maxHappiness,
                                            hunger / maxHunger,
                                            stamina / maxStamina);

        UIManager.instance.UpdateFoodStorageUI(numberOfApples, numberOfChickens);
    }
    
    protected virtual void UpdateHappiness()
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

        if (playing)
        {
            happiness += Time.deltaTime * baseHappinessReductionRate * hungerScale;
        }
        else
        {   // ( (Tid (1) * Base (1)) * Poop (1.25) ) * Hunger (2) = 2.5 
            happiness -= Time.deltaTime * baseHappinessReductionRate * poopOnFloor * hungerScale;
        }

        happiness = Mathf.Clamp(happiness, 0f, maxHappiness);
    }

    protected virtual void UpdateHunger()
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

    protected virtual void UpdateStamina()
    {
        float workingScale = 1;
        if (Village.instance.working)
        {
            workingScale = 3.0f;
        }
        
        var staminaChange = Time.deltaTime * baseStaminaReductionRate *  workingScale;
        if (sleeping)
        {
            stamina += staminaChange * staminaSleepingIncrease;
            if (stamina >= maxStamina)
            {
                animator.SetBool("Idle", true);
                sleeping = false;
            }
        }
        else 
        {
            stamina -= staminaChange;
        }
        
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
    }

    protected virtual void UpdatePoop()
    {
        poopTimer -= Time.deltaTime;

        if (poopTimer <= 0)
        {
            poopStored += 1;
            poopTimer = poopTimeSeconds;

            if (poopStored >= maxPoopStored)
            {
                Poop();
            }
        }
    }

    public void Feed(float amount, bool isApple)
    {
        if(isApple == true)
        {
            if (numberOfApples > 0)
            {
                happiness += 2;
                numberOfApples--;
                hunger += amount;

                poopStored += Mathf.FloorToInt(amount / 2);
                if (poopStored >= maxPoopStored)
                {
                    Poop();
                }
                else
                {
                    SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Eat]);
                }

                ClearAnimation();
                animator.SetTrigger("Eat");

                GameObject apple = Instantiate(applePrefab, foodSpawnTransform);
                Destroy(apple, 0.8f);
                Village.instance.SetWork(false);
            }
        }
        else
        {
            if(numberOfChickens > 0)
            {
                happiness += 2;
                numberOfChickens--;
                hunger += amount;

                poopStored += Mathf.FloorToInt(amount / 2);
                if (poopStored >= maxPoopStored)
                {
                    Poop();
                }
                else
                {
                    SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Eat]);
                }

                ClearAnimation();
                animator.SetTrigger("Eat");

                GameObject chicken = Instantiate(chickenPrefab, foodSpawnTransform);
                Destroy(chicken, 0.8f);
                Village.instance.SetWork(false);
            }
        }

        sleeping = false;
        playing = false;
    }

    private void Poop()
    {
        poopStored = 0;

        //Spawn poop graphic
        for (int i = 0; i < poopInGame.Length; i++)
        {
            if (poopInGame[i] == null)
            {
                if (i == 0)
                {
                    poopOnFloor = 2.0f;
                    GameObject poopClone = Instantiate(poopPrefab, poopSpawnTransform_01);
                    poopInGame[i] = poopClone;
                }
                else if (i == 1)
                {
                    poopOnFloor = 3.0f;
                    GameObject poopClone = Instantiate(poopPrefab, poopSpawnTransform_02);
                    poopInGame[i] = poopClone;
                }
                else if (i == 2)
                {
                    poopOnFloor = 4.0f;
                    GameObject poopClone = Instantiate(poopPrefab, poopSpawnTransform_03);
                    poopInGame[i] = poopClone;
                }
                break;
            }
        }
        
        SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Poop]);
    }

    public void Play()
    {  
        if (playing)
        {
            StopWorking();
            playing = false;
        }
        else
        {
            ClearAnimation();
            animator.SetTrigger("Play");
            playing = true;
        }
        
        ball.SetActive(playing);

        Village.instance.SetWork(false);

        SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Play]);
        
        sleeping = false;
    }

    public void Work()
    {
        if (Village.instance.working)
        {
            StopWorking();
            Village.instance.working = false;
        }
        else
        {
            ClearAnimation();
            smokeParticle.Play();
            animator.SetTrigger("Work");
            Village.instance.working = true;
        }
    
        Village.instance.SetWork(Village.instance.working);

        SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Work]);

        sleeping = false;
        playing = false;
    }

    public void Sleep()
    {
        if (sleeping)
        {
            StopWorking();
            sleeping = false;
            SoundManager.instance.StopCuragonSound();
        }
        else
        {
            ClearAnimation();
            sleeping = true;
            animator.SetTrigger("Sleep");
            SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Sleep]);
        }

        Village.instance.SetWork(false);

        playing = false;
    }

    public void Clean()
    {
        for (int i = 0; i < poopInGame.Length; i++)
        {
            if (poopInGame[i] != null)
            {
                Destroy(poopInGame[i]);
                happiness += 5;
            }
        }
        poopOnFloor = 1.0f;
        
        SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Clean]);
    }

    public virtual float GetWorkingCondition()
    {
        float workingConstant = 1; //How well curagon is able to work
        
        //stamina
        float staminaProcent = stamina / maxStamina;
        if (staminaProcent <= 0.2)
        {
            return 0;
        }
        else if (staminaProcent >= 0.8)
        {
            workingConstant *= 1.5f;
        }
        
        //happiness
        float happinessProcent = happiness / maxHappiness;
        if (happinessProcent <= 0.3)
        {
            workingConstant *= 0.5f;
        }
        else if (happinessProcent >= 0.8)
        {
            workingConstant *= 1.5f;
        }

        //hunger
        float hungerProcent = hunger / maxHunger;
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

    void ClearAnimation()
    {
        animator.SetBool("Idle", false);
        smokeParticle.Stop();
        ball.SetActive(false);
    }

    void AddApple()
    {
        appleTimer -= Time.deltaTime;
        if (appleTimer <= 0.0f)
        {
            appleTimer = appleTimeSeconds;
            numberOfApples = Mathf.Clamp(++numberOfApples, 0, maxNumberOfApples);
        }
    }

    void AddChicken()
    {
        chickenTimer -= Time.deltaTime;
        if (chickenTimer <= 0.0f)
        {
            chickenTimer = chickenTimeSeconds;
            numberOfChickens = Mathf.Clamp(++numberOfChickens, 0, maxNumberOfChickens);
        }
    }

    public void StopWorking()
    {
        animator.SetBool("Idle", true);
        smokeParticle.Stop();
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
