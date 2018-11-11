using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] float maxHP = 100f;

    float currentHP = 100f;

    public float healthAsPercentage
    {
        get
        {
            return currentHP / (float)maxHP;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
