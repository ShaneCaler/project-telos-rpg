using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHP = 100f;

    [SerializeField] float moveRadius = 10f;
    [SerializeField] float attackRadius = 5f;

    [SerializeField] float damagePerShot = 8f;
    [SerializeField] float secondsBetweenShots = 1f;
    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSocket;

    [Header("Enemy Level")]
    [SerializeField] bool easy = false;
    [SerializeField] bool medium = false;
    [SerializeField] bool hard = false;

    bool isAttacking = false;
    GameObject player = null;
    AICharacterControl aiCharacter = null;
    float currentHP = 100f;

    public float healthAsPercentage
    {
        get
        {
            return currentHP / maxHP;
        }
    }

    public void TakeDamage(float damage)
    { 
        currentHP = Mathf.Clamp(currentHP - damage, 0f, maxHP);
    }

    // Use this for initialization
    void Start () {
        aiCharacter = GetComponent<AICharacterControl>();
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (distance <= attackRadius && !isAttacking)
        {
            InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShots);
        }

        if(distance > attackRadius)
        {
            CancelInvoke();
        }

        if(distance <= moveRadius)
        {
            aiCharacter.SetTarget(player.transform);
            print("Moving to" + player.transform);
        }
        else
        {
            aiCharacter.SetTarget(transform);
        }
    }

    void SpawnProjectile()
    {
        isAttacking = true;
        var adjustedPlayerTransform = new Vector3(player.transform.position.x, player.transform.position.y + 1.7f, player.transform.position.z);
        var projectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        var projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.damageCaused = damagePerShot;

        Vector3 unitVectorToPlayer = (adjustedPlayerTransform - projectileSocket.transform.position).normalized;
        projectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileComponent.projectileSpeed;
    }

    void OnDrawGizmos()
    {
        // draw attack sphere
        Gizmos.color = new Color(255f, 0f, 0f, .5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // draw move sphere
        Gizmos.color = new Color(0f, 0f, 255f, .5f);
        Gizmos.DrawWireSphere(transform.position, moveRadius);
    }

}
