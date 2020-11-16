
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using System.IO;
using System.Collections;

public class EnemyController : MonoBehaviour
{

    public NavMeshAgent agent;
    private World world; 
    private Transform player;
    public ThirdPersonCharacter character;
    private float timer;
    public float wanderTimer = 50;
    public bool alive = true;
    public Settings settings;
    private GameObject m_RightFist;
    bool freez = false;
    enum AgentDestination {Player, Nothing};
    AgentDestination currentDestination = AgentDestination.Nothing; 


    void Start()
    {
        world = GameObject.Find("World").GetComponent<World>();
        player = GameObject.Find("Player").transform;
        agent.updateRotation = false;
        m_RightFist = GameObject.Find("LowManRightHandMiddle1");
        string jsonImport = File.ReadAllText(Application.dataPath + "/settings.cfg");
        settings = JsonUtility.FromJson<Settings>(jsonImport);


    }


    public void activateFist()
    {
        if (m_RightFist != null)
        {
            m_RightFist.GetComponent<Collider>().enabled = true;
        }
    }

    public void deactivateFist()
    {
        if (m_RightFist != null)
        {
            m_RightFist.GetComponent<Collider>().enabled = false;
        }
        
    }

    void Update()
    {
        if (alive)
        {
            timer += Time.deltaTime;
            if (!freez)
            {
                if (Vector3.Distance(transform.position, player.position) <= 15)
                {
                    agent.SetDestination(player.position);
                    currentDestination = AgentDestination.Player;

                }
                else
                {
                    if (timer >= wanderTimer)
                    {
                        Vector3 newPos = RandomNavSphere(transform.position, 25, -1);
                        agent.SetDestination(newPos);
                        timer = 0;
                    }
                    currentDestination = AgentDestination.Nothing;

                }

                if (agent.remainingDistance > agent.stoppingDistance)
                {

                    character.Move(agent.desiredVelocity, false, false);

                }
                else
                {
                    character.Move(Vector3.zero, false, false);
                    if (agent.remainingDistance < 1.5f && currentDestination == AgentDestination.Player)
                    {
                        GetComponent<Animator>().SetTrigger("Attack");
                    }
                }
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
           

            if (Vector3.Distance(transform.position, player.position) > ((settings.viewDistance+1) * VoxelData.ChunkWidth))
            {
                Destroy(gameObject);
            }
  
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }
        

    }
    IEnumerator IEnumFreezEnemy()
    {
        freez = true;
        yield return new WaitForSeconds(3);
        freez = false;
    }
    public void freezEnemy()
    {
        Debug.Log("Freez enemy");
        StartCoroutine(IEnumFreezEnemy());
    }


    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

}
