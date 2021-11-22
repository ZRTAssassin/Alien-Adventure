using System;
using UnityEngine;

public class Fly : HittableFromAbove, ITakeDamage
{
    Vector2 _startingPosition;
    [SerializeField] Vector2 _direction = Vector2.up;
    [SerializeField] float _maxDistance = 2f;
    [SerializeField] float _speed = 2f;
    [SerializeField] AudioClip _deathSound;



    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
        _audioSource.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction.normalized * (Time.deltaTime * _speed));
        var distance = Vector2.Distance(_startingPosition, transform.position);
        if (distance >= _maxDistance)
        {
            transform.position = _startingPosition + (_direction.normalized * _maxDistance);
            _direction *= -1;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        var damageable = collision.collider.GetComponent<ITakeDamage>();
        if (!collision.WasHitFromAbove() && collision.WasHitByPlayer())
        {
            if (damageable != null)
                damageable.TakeDamage(1);
        }
        else if (collision.WasHitByPlayer() && collision.WasHitFromAbove())
        {
            Instantiate(_deathParticlePrefab, transform.position, Quaternion.identity);
            TakeDamage(1);
        }
        
        
    }

    public void TakeDamage(int amount)
    {
        if(_deathSound != null)
            _audioSource.PlayOneShot(_deathSound);
        _audioSource.Stop();
        gameObject.SetActive(false);
    }
}
