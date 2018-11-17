using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectileSpeed;
    public float damageCaused { get; set; }
    Player player;


    void Start()
    {
        //player = FindObjectOfType<Player>();
    }
    void Update()
    {
        //var adjustedTransform = new Vector3(player.transform.position.x, player.transform.position.y + 1.7f, player.transform.position.z);
        //transform.LookAt(player.transform);
        //gameObject.transform.position = Vector3.Lerp(transform.position, adjustedTransform , projectileSpeed);
        //gameObject.GetComponent<Rigidbody>().velocity = (player.transform.position - transform.position).normalized * projectileSpeed;
    }
    
    void OnTriggerEnter(Collider other)
    {
        Component damageableComponent = other.gameObject.GetComponent(typeof(IDamageable));
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
        Destroy(gameObject, 0.3f);
    }
}
