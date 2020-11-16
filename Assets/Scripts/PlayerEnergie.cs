using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergie : MonoBehaviour
{
    float energie = 200f;
    float maxEnergie = 200f;
    float sleepTime = 200000f;


    public float getEnergieProzent()
    {
        return energie / maxEnergie;
    }

    public void subtractEnergie(float en)
    {
        this.energie -= en;
        if (this.energie - en <= 0)
        {
            this.energie = 0;
        }
        else
        {
            this.energie -= en;
        }
    }
    void FixedUpdate()
    {
        if (sleepTime <= 0)
        {
            if (energie + 1 < maxEnergie)
            {
                addEnergie(0.8f);
                sleepTime = 200000;
            }
        }
        else
        {
            sleepTime = -Time.deltaTime;
        }
        
    }

    private void addEnergie(float energieToAdd)
    {
       
        if ((energie + energieToAdd) <= maxEnergie)
        {
            energie += energieToAdd;

        }
        else
        {
            energie = maxEnergie;
        }

        
    }

    public float getEnergie()
    {
        return energie;
    }
}
