using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Texture2D easyHealthBar;
    [SerializeField] Texture2D mediumHealthBar;
    [SerializeField] Texture2D hardHealthBar;

    RawImage healthBarRawImage = null;
    Enemy enemy = null;

    // Use this for initialization
    void Start()
    {
        enemy = GetComponentInParent<Enemy>(); // Different to way player's health bar finds player
        healthBarRawImage = GetComponent<RawImage>();

        switch (enemy.GetEnemyLevel())
        {
            case EnemyType.Easy:
                healthBarRawImage.texture = easyHealthBar;
                break;
            case EnemyType.Medium:
                healthBarRawImage.texture = mediumHealthBar;
                break;
            case EnemyType.Hard:
                healthBarRawImage.texture = hardHealthBar;
                break;
            default:
                Debug.LogError("Error in enemy health bar");
                return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float xValue = -(enemy.healthAsPercentage / 2f) - 0.5f;
        healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }
}
