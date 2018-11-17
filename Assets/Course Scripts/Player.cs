using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

    [SerializeField] float maxHP = 100f;

    float currentHP = 100f;

    public float healthAsPercentage
    {
        get { return currentHP / (float)maxHP; }
    }

    public void TakeDamage(float damage) // or void Idamageable.TakeDamage(float damage);
    {
        currentHP = Mathf.Clamp(currentHP - damage, 0f, maxHP);

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
