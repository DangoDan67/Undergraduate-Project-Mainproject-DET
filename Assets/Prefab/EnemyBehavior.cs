using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public float health = 600f;
    public float hitForce = 20f;

    GameObject score;
    // Start is called before the first frame update
    void Start()
    {
        this.score = GameObject.FindGameObjectWithTag("Score");
    }




    private IEnumerator TakeContinuouslyDemage(int demage)
    {
        TakeDamage(20);
        yield return new WaitForSeconds(2);
        TakeDamage(20);
        yield return new WaitForSeconds(2);
        TakeDamage(20);
        yield return new WaitForSeconds(2);
        TakeDamage(20);
        yield return new WaitForSeconds(2);
        TakeDamage(20);
    }

    public void TakeDemageContinously(int demage)
    {
        StartCoroutine(TakeContinuouslyDemage(demage));
    }



    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (gameObject.GetComponent<EnemyController>().alive)
            {
                Die();
            }

        }

    }

    void Die()
    {
        gameObject.GetComponent<Animator>().SetBool("DeathTrigger", true);
        gameObject.GetComponent<EnemyController>().alive = false;
        score.GetComponent<score>().addScore();
        Destroy(gameObject, 8f);
    }

}
