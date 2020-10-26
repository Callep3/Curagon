using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curagon : MonoBehaviour
{
    public float maxHappiness = 100;
    public float happiness;
    public float maxHunger = 100;
    public float hunger;
    public float maxStamina = 100;
    public float stamina;
    private bool feed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (feed)
        {
            hunger += 10;
            if (hunger >= maxHunger)
            {
                hunger = maxHunger;
            }
        }
    }
}
