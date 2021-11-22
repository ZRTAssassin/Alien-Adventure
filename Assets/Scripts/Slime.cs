using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : HittableFromAbove, ITakeDamage
{
    [SerializeField] Transform _leftSensor;
    [SerializeField] Transform _rightSensor;
    [SerializeField] Sprite _deadSprite;
    [SerializeField] float _fadeSpeed = 5f;
    [SerializeField] LayerMask layerMask;

    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    float _direction = -1;
    Animator _animator;
    Collider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
    }

    public void TakeDamage(int amount)
    {
        StartCoroutine(Die());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody2D.velocity = new Vector2(_direction, _rigidbody2D.velocity.y);

        if (_direction < 0)
        {
            ScanSensor(_leftSensor);
        }
        else
        {
            ScanSensor(_rightSensor);
        }
    }

    void ScanSensor(Transform sensor)
    {
        Debug.DrawRay(sensor.position, Vector2.down * 0.1f, Color.red);
        var result = Physics2D.Raycast(sensor.position, Vector2.down, 0.1f, layerMask);
        if (result.collider == null)
            TurnAround();

        Debug.DrawRay(sensor.position, new Vector2(_direction, 0) * 0.1f, Color.red);
        var sideResult = Physics2D.Raycast(sensor.position, new Vector2(_direction, 0), 0.1f, layerMask);
        if (sideResult.collider != null)
            TurnAround();
    }

    void TurnAround()
    {
        _direction *= -1;
        _spriteRenderer.flipX = _direction > 0;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        
        var damageable = collision.collider.GetComponent<ITakeDamage>();
        if (collision.WasHitFromAbove() == false && collision.WasHitByPlayer())
        {
            if (damageable != null)
                damageable.TakeDamage(1);
        }
        if (collision.WasHitByPlayer() && collision.WasHitFromAbove())
        {
            TakeDamage(1);
            Instantiate(_deathParticlePrefab, transform.position, Quaternion.identity);
            _audioSource.Play();
        }

    }

    IEnumerator Die()
    {
        _spriteRenderer.sprite = _deadSprite;
        _animator.enabled = false;
        _collider.enabled = false;
        this.enabled = false; //disables this component
        _rigidbody2D.simulated = false;

        float alpha = 1f;

        while (alpha > 0)
        {
            yield return null;
            alpha -= Time.deltaTime / _fadeSpeed;
            _spriteRenderer.color = new Color(1, 1, 1, alpha);
        }
    }
}