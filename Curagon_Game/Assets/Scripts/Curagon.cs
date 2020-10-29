using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curagon : MonoBehaviour
{
    Animator animator;

    ParticleSystem smokeParticle;

    GameObject ball; // Curagon's ball playing animation

    GameObject applePrefab;
    Transform appleSpawnTransform;

    GameObject poopPrefab;
    Transform poopSpawnTransform_01;
    Transform poopSpawnTransform_02;
    Transform poopSpawnTransform_03;

    GameObject[] poopInGame;

    AudioClip[] audioClips;
    
    float happiness;
    float hunger;
    float stamina;
    const float maxHappiness = 100f;
    const float maxHunger = 100f;
    const float maxStamina = 100f;
    
    // When poop reaches maxPoop (Make a poop.)
    int poopStored;
    const int maxPoopStored = 10;
    
    int numberOfApples;
    float appleTimer;
    const float appleTimeSeconds = 5f;

    int numberOfChickens;
    float chickenTimer;
    const float chickenTimeSeconds = 2f;
    
    float poopTimer;
    const float poopTimeSeconds = 10f;
    
    public bool sleeping;

    float baseHappinessReductionRate;
    float baseHungerReductionRate;
    float baseStaminaReductionRate;
    float staminaSleepingIncrease;
    float poopOnFloor;

    void Awake()
    {
        Init();
    }

    void Update()
    {
        if (!UIManager.instance.gamePaused)
        {
            AddApple();
            UpdateStats();
        }
    }
    
    // Initialize
    void Init()
    {
        Debug.Log("Curagon init start");
        GetAllComponents();
        
        happiness = maxHappiness;
        hunger = maxHunger;
        stamina = maxStamina;
        poopStored = 0;
        numberOfApples = 5;

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
        Debug.Log("Curagon init end");
    }

    void GetAllComponents()
    {
        animator = GetComponent<Animator>();
        ball = transform.Find("GFX").Find("Curagon-Ball").gameObject;
        applePrefab = Resources.Load<GameObject>("Prefabs/Apple");
        appleSpawnTransform = GameObject.Find("AppleSpawnLocation").transform;

        poopPrefab = Resources.Load<GameObject>("Prefabs/Poop");
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

        //           ( (Tid (1) * Base (1)) * Poop (1.25) ) * Hunger (2) = 2.5 
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
        
        var staminaChange = Time.deltaTime * baseStaminaReductionRate *  workingScale;
        if (sleeping)
        {
            stamina += staminaChange * staminaSleepingIncrease;
            if (stamina >= maxStamina)
            {
                sleeping = false;
            }
            
            SoundManager.instance.StopCuragonSound();
            
            ClearAnimation();
            animator.SetBool("Sleep", sleeping);
        }
        else 
        {
            stamina -= staminaChange;
        }
        
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
    }

    private void UpdatePoop()
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

    public void Feed(float amount)
    {
        if (numberOfApples > 0)
        {
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

            GameObject apple = Instantiate(applePrefab, appleSpawnTransform);
            Destroy(apple, 0.8f);
            Village.instance.SetWork(false);
        }
        sleeping = false;
    }

    private void Poop()
    {
        poopStored = 0;
        //poopOnFloor = 2.0f;
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

    public void Play(float amount)
    {
        happiness = Mathf.Clamp(happiness + amount, 0, maxHappiness);        
        
        ClearAnimation();
        animator.SetBool("Play", true);
        ball.SetActive(true);

        Village.instance.SetWork(false);

        SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Play]);
        
        sleeping = false;
    }

    public void Work()
    {
        ClearAnimation();
        animator.SetBool("Work", true);

        smokeParticle.Play();
        
        Village.instance.SetWork(true);
        
        SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Work]);
        
        sleeping = false;
    }

    public void Sleep(float amount)
    {
        if (sleeping)
        {
            sleeping = false;
            SoundManager.instance.StopCuragonSound();
        }
        else
        {
            sleeping = true;
            SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Sleep]);
        }

        ClearAnimation();
        animator.SetBool("Sleep", sleeping);

        Village.instance.SetWork(false);
    }

    public void Clean()
    {
        for(int i = 0; i < poopInGame.Length; i++)
        {
            if(poopInGame[i] != null)
            {
                Destroy(poopInGame[i]);
            }
        }
        poopOnFloor = 1.0f;
        
        SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Clean]);
    }

    public float GetWorkingCondition()
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

    public void Restart()
    {
        Init();
    }

    void ClearAnimation()
    {
        animator.SetBool("Play", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Sleep", false);
        animator.SetBool("Work", false);
        smokeParticle.Stop();
        
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

    /*void AddChicken()
    {
        chickenTimer -= Time.deltaTime;
        if (chickenTimer <= 0.0f)
        {
            chickenTimer = chickenTimeSeconds;
            numberOfChickens++;
            // Debug.Log($"numOfApples: {numberOfApples}");
        }
    }*/
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