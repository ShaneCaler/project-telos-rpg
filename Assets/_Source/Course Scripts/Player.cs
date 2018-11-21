using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

    [SerializeField] float maxHP = 100f;
    [SerializeField] float playerMeleeDamage = 10f;
    [SerializeField] float minTimeBetweenHits = .5f;
    [SerializeField] int enemyLayer = 9;
    [SerializeField] float maxAttackRange = 5f;

    GameObject currentTarget;
    CameraRaycaster cameraRaycaster;
    float currentHP;
    float lastHitTIme = 0f;

    void Start()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        currentHP = maxHP;
    }

    public float healthAsPercentage
    {
        get { return currentHP / (float)maxHP; }
    }

    public void TakeDamage(float damage) // or void Idamageable.TakeDamage(float damage);
    {
        currentHP = Mathf.Clamp(currentHP - damage, 0f, maxHP);

    }

    void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        // todo implement UI for targeting enemies
        if(layerHit == enemyLayer)
        {
            GameObject enemy = raycastHit.collider.gameObject;

            if((enemy.transform.position - transform.position).magnitude > maxAttackRange)
            {
                return;
            }

            currentTarget = enemy;

            if (Time.time - lastHitTIme > minTimeBetweenHits)
            {
                currentTarget.GetComponent<Enemy>().TakeDamage(playerMeleeDamage);
                lastHitTIme = Time.time;
            }
        }
    }
}
