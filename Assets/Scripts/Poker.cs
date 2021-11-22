using System;
using UnityEngine;

public class Poker : MonoBehaviour, ITakeDamage
{
    [SerializeField] int _waitTimer = 5;
    [SerializeField] float _timer;

    bool _up = true;
    int _upToHash;

    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _upToHash = Animator.StringToHash("Up");
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _waitTimer)
        {
            SwapState();
        }
    }

    void SwapState()
    {
        _up = !_up;
        _animator.SetBool(_upToHash, _up);
        _timer = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var damageable = collision.collider.GetComponent<ITakeDamage>();
        if (collision.WasHitByPlayer())
            damageable.TakeDamage(1);
    }

    public void TakeDamage(int amount)
    {
        gameObject.SetActive(false);
    }
}