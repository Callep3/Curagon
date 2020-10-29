using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EddieDragon : Curagon
{
    protected override void Update()
    {
        base.Update();
    }
    
    protected override void Init()
    {
        base.Init();
        
        baseHappinessReductionRate = 1f;
        baseHungerReductionRate = 1.25f;
        baseStaminaReductionRate = 1.5f;
    }
    
    protected override void UpdateHunger()
    {
        base.UpdateHunger();
        // float staminaScale = 1;
        // float staminaProcent = stamina / maxStamina;
        // if (staminaProcent <= 0.4)
        // {
        //     staminaScale = 2;
        // }
        //
        // hunger -= Time.deltaTime * baseHungerReductionRate * staminaScale;
        // hunger = Mathf.Clamp(hunger, 0f, maxHunger);
    }
    
    protected override void UpdateHappiness()
    {
        base.UpdateHappiness();
        // float hungerScale = 1;
        // float hungerProcent = hunger / maxHunger;
        // if (hungerProcent <= 0.5)
        // {
        //     hungerScale = 2;
        // }
        // else if (hungerProcent <= 0.25)
        // {
        //     hungerScale = 3;
        // }
        //
        // //           ( (Tid (1) * Base (1)) * Poop (1.25) ) * Hunger (2) = 2.5 
        // happiness -= Time.deltaTime * baseHappinessReductionRate * poopOnFloor * hungerScale;
        // happiness = Mathf.Clamp(happiness, 0f, maxHappiness);
    }
    
    protected override void UpdateStamina()
    {
        base.UpdateStamina();
        // float workingScale = 1;
        // if (Village.instance.working)
        // {
        //     workingScale = 3.0f;
        // }
        //
        // stamina -= Time.deltaTime * baseStaminaReductionRate * workingScale;
        // stamina = Mathf.Clamp(stamina, 0f, maxStamina);
        // //TODO Add sleep function
    }
    
    protected override void UpdatePoop()
    {
        base.UpdatePoop();
        // poopTimer -= Time.deltaTime;
        //
        // if (poopTimer <= 0)
        // {
        //     poopStored += 1;
        //     poopTimer = poopTimeSeconds;
        //
        //     if (poopStored >= maxPoopStored)
        //     {
        //         Poop();
        //     }
        // }
    }

    public override void Feed(float amount)
    {
        base.Feed(amount);
        // if (numberOfApples > 0)
        // {
        //     numberOfApples--;
        //     hunger += amount;
        //
        //     poopStored += Mathf.FloorToInt(amount / 2);
        //     if (poopStored >= maxPoopStored)
        //     {
        //         Poop();
        //     }
        //     else
        //     {
        //         SoundManager.instance.PlayCuragonSound(audioClips[(int)Curagon_Sounds.Eat]);
        //     }
        //
        //     ClearAnimation();
        //     animator.SetTrigger("Eat");
        //
        //     GameObject apple = Instantiate(applePrefab, appleSpawnTransform);
        //     Destroy(apple, 0.8f);
        //     Village.instance.SetWork(false);
        // }
    }

    public override float GetWorkingCondition()
    {
        return base.GetWorkingCondition();
        // float workingConstant = 1; //How well curagon is able to work
        //
        // //stamina
        // float staminaProcent = stamina / maxStamina;
        // if (staminaProcent <= 0.2)
        // {
        //     return 0;
        // }
        // else if (staminaProcent >= 0.8)
        // {
        //     workingConstant *= 1.5f;
        // }
        //
        // //happiness
        // float happinessProcent = happiness / maxHappiness;
        // if (happinessProcent <= 0.3)
        // {
        //     workingConstant *= 0.5f;
        // }
        // else if (happinessProcent >= 0.8)
        // {
        //     workingConstant *= 1.5f;
        // }
        //
        // //hunger
        // float hungerProcent = hunger / maxHunger;
        // if (hungerProcent <= 0.3)
        // {
        //     workingConstant *= 0.5f;
        // }
        // else if (hungerProcent >= 0.8)
        // {
        //     workingConstant *= 1.5f;
        // }
        //
        // return workingConstant;
    }
}
