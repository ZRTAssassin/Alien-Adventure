using UnityEngine;

public class KillOnEnter : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        var damageable = collision.collider.GetComponent<ITakeDamage>();
        
        if (damageable != null)
            damageable.TakeDamage(10);
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        var damageable = collision.GetComponent<ITakeDamage>();
        
        if (damageable != null)
            damageable.TakeDamage(10);


    }

    void OnParticleCollision(GameObject collision)
    {
        var damageable = collision.GetComponent<ITakeDamage>();
        
        if (damageable != null)
            damageable.TakeDamage(10);
    }
}
