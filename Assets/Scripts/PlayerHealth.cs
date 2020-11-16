using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    float health = 300f;
    float maxHealth = 300f;
    int respawn = 0;
    int maxRespawn = 5;
    public score score;

    float sleepTime = 3f; 

    public float getHealthProzent()
    {
        return health / maxHealth;
    }

    public void addHealth(float healthToAdd)
    {
        if ((health + healthToAdd) <= maxHealth)
        {
            health += healthToAdd;
        }
        else {
            health = maxHealth;
        }
    }

    public void addRespawn()
    {
        if ((respawn + 1) <= maxRespawn)
        {
            respawn += 1;
        }
        else {
            respawn = maxRespawn;
        }
    }

    public void subtractHealth(float hit)
    {
        if (sleepTime <= 0)
        {
            this.health -= hit;
            if (this.health - hit <= 0)
            {
                this.health = 0;
            }
            else
            {
                this.health -= hit;
            }
            sleepTime = 3;
        }
        else {
            sleepTime = -Time.deltaTime;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyFist")
        {
            TakeDamage(50);
        }
    }

     private void TakeDamage(float demage)
     {
         health -= demage;
         if (health <= 0)
         {
             if (respawn <= 0)
             {
                passParameter.score = score.getIntScore();
                FindObjectOfType<GameManager>().EndGame();
             }
             else
             {
                 respawn -= 1;
                 //Player respawn
                 health = maxHealth;
                 Debug.Log("Player respawn");
             }
         }
     }

}
