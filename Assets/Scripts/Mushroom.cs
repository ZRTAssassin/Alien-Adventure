using System;
using UnityEngine;

public class Mushroom : HittableFromAbove
{
    
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.WasHitFromAbove() && collision.WasHitByPlayer())
            PlaySoundEffect();
        
        
    }

    void PlaySoundEffect()
    {
        _audioSource.Play();
    }
}