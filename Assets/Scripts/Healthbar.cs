using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public PlayerHealth playerHealth;
    void Update()
    {
        float healthProzent;
        if (playerHealth.getHealthProzent() <= 0)
        {
            healthProzent = 0;
        }
        else {
            healthProzent = playerHealth.getHealthProzent();
        }
        transform.localScale = new Vector3(healthProzent, 1);
    }
}
