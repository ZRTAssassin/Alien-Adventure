using System;
using UnityEngine;

public abstract class HittableFromAbove : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected GameObject _deathParticlePrefab;
    
    [Tooltip("How much it bounces things when they hit it from above.")][SerializeField] float _bounceVelocity;

    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        var damageable = collision.collider.GetComponent<ITakeDamage>();
        
        
        if (collision.WasHitByPlayer() == false)
            return;
        
        if (collision.WasHitFromAbove())
        {
            player.Bounce(_bounceVelocity);
            
            
            return;
        }

        
    }

    
    
}