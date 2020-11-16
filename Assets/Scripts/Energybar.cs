using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energybar : MonoBehaviour
{
    public PlayerEnergie playerEnergie;
    void Update()
    {
        float energieProzent;
        if (playerEnergie.getEnergieProzent() <= 0)
        {
            energieProzent = 0;
        }
        else
        {
            energieProzent = playerEnergie.getEnergieProzent();
        }
        transform.localScale = new Vector3(energieProzent, 1);

    }
}
