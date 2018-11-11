using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
public class Enemy : MonoBehaviour {

    [SerializeField] float maxHP = 100f;
    [SerializeField] float attackRadius = 5f;
    [Header("Enemy Level")]
    [SerializeField] bool easy = false;
    [SerializeField] bool medium = false;
    [SerializeField] bool hard = false;

    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    public GameObject player = null;
    AICharacterControl aiCharacter = null;
    float currentHP = 100f;

    public float healthAsPercentage
    {
        get
        {
            return currentHP / maxHP;
        }
    }

	// Use this for initialization
	void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        aiCharacter = GetComponent<AICharacterControl>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent.updateRotation = false;
        agent.updatePosition = true;
    }
	
    public EnemyType GetEnemyLevel()
    {
        if (easy)
        {
           return EnemyType.Easy;
        }
        else if (medium)
        {
            return EnemyType.Medium;
        }
        else
        {
            return EnemyType.Hard;
        }
    }
	// Update is called once per frame
	void Update () {

        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (player != null && distance <= attackRadius)
        {
            // agent.SetDestination(player.transform.position);
            aiCharacter.SetTarget(player.transform);
            print("Moving to" + player.transform);
        }
        else
        {
            aiCharacter.SetTarget(transform);
        }
        // this code was my response to the challenge, the above is the instructor's solution
        //if (agent.remainingDistance > agent.stoppingDistance && distance < attackRadius)
        //{
        //    // aiCharacter.Move(agent.desiredVelocity, false, false);
        //    print("Moving at velocity of " + agent.desiredVelocity);

        //}
        //else
        //{
        //    print("Doing nothing ");
        //    // aiCharacter.Move(Vector3.zero, false, false);
        //}
    }
}
