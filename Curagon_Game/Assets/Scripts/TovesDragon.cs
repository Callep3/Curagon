using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TovesDragon : Curagon
{
    protected override void Init()
    {
        base.Init();

        baseHappinessReductionRate = 0.50f;
        baseHungerReductionRate = 0.55f;
        baseStaminaReductionRate = 0.30f;
        staminaSleepingIncrease = 8f;
    }
    
    protected override void UpdateHappiness()
    {
        float hungerFactor;
        
        if (playing)
        {
            float playFactor = 8f;
            hungerFactor = 0.5f + hunger / maxHunger;
            happiness += Time.deltaTime * playFactor * hungerFactor / poopOnFloor;
        }
        else
        {
            hungerFactor = 1f + (1f - hunger / maxHunger);
            float workingFactor = (Village.instance.working) ? 4f : 1f;

            happiness -= Time.deltaTime * baseHappinessReductionRate * poopOnFloor * hungerFactor * workingFactor;
        }
        
        happiness = Mathf.Clamp(happiness, 0f, maxHappiness);
    }
    
    protected override void UpdateHunger()
    {
        float staminaFactor = 1f + (1f - stamina / maxStamina);
        float workingFactor = (Village.instance.working) ? 2f : 1f;
        float playFactor = (playing) ? 1.5f : 1f;

        hunger -= Time.deltaTime * baseHungerReductionRate * staminaFactor * workingFactor * playFactor;
        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
    }

    protected override void UpdateStamina()
    {
        float hungerFactor;

        if (sleeping)
        {
            hungerFactor = 1f + (1f - hunger / maxHunger);

            stamina += Time.deltaTime * staminaSleepingIncrease * hungerFactor;

            if (stamina >= maxStamina)
            {
                sleeping = false;
                animator.SetBool("Sleep", false);
                SoundManager.instance.StopCuragonSound();
            }
        }
        else
        {
            hungerFactor = 1f + (1f - hunger / maxHunger);
            float workingFactor = (Village.instance.working) ? 3f : 1f;
            float playFactor = (playing) ? 1.75f : 1f;
            
            stamina -= Time.deltaTime * baseStaminaReductionRate * hungerFactor * workingFactor * playFactor;
        }

        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
    }
    
    protected override void UpdatePoop()
    {
    }

    public override float GetWorkingCondition()
    {
        if (stamina < 20 || happiness < 10 || hunger < 10)
            return 0;

        float happinessFactor = 0.75f + happiness / maxHappiness;
        float hungerFactor = 0.5f + hunger / maxHunger;
        float staminaFactor = 0.25f + stamina / maxStamina;

        return staminaFactor * happinessFactor * hungerFactor;
    }
}
