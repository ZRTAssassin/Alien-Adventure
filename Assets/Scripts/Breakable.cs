using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour, ITakeDamage
{
    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int amount)
    {
        TakeHit();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.GetComponent<Player>() == null)
            return;

        if (collision.contacts[0].normal.y > 0)
            TakeHit();
    }

    void TakeHit()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();

        var spriteRenderer = GetComponent<SpriteRenderer>();
        var collider2D = GetComponent<Collider2D>();

        spriteRenderer.enabled = false;
        collider2D.enabled = false;
        _audioSource.Play();
    }
}
