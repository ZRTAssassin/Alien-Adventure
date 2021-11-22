using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    [SerializeField] ToggleDirecion _startingDirection = ToggleDirecion.Center;

    [SerializeField] UnityEvent _onCenter;
    [SerializeField] UnityEvent _onLeft;
    [SerializeField] UnityEvent _onRight;

    [SerializeField] Sprite _centerSprite;
    [SerializeField] Sprite _leftSprite;
    [SerializeField] Sprite _rightSprite;

    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] AudioClip _leftSound;
    [SerializeField] AudioClip _rightSound;


    ToggleDirecion _currentDirection;
    AudioSource _audioSource;

    enum ToggleDirecion
    {
        Left,
        Center,
        Right,
    }

    void Awake()
    {
        //_spriteRenderer = GetComponent<SpriteRenderer>();

        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        SetToggleDirection(_startingDirection, true);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        var playerRigidbody = player.GetComponent<Rigidbody2D>();
        if (playerRigidbody == null)
            return;

        bool wasOnRight = collision.transform.position.x > transform.position.x;
        bool playerWalkingRight = playerRigidbody.velocity.x > 0;
        bool playerWalkingLeft = playerRigidbody.velocity.x < 0;

        if (wasOnRight && playerWalkingRight)
            SetToggleDirection(ToggleDirecion.Right);
        else if (!wasOnRight && playerWalkingLeft)
            SetToggleDirection(ToggleDirecion.Left);
    }

    void SetToggleDirection(ToggleDirecion direction, bool force = false)
    {
        if (force == false && _currentDirection == direction)
            return;

        _currentDirection = direction;

        switch (direction)
        {
            case ToggleDirecion.Left:
                _spriteRenderer.sprite = _leftSprite;
                _onLeft?.Invoke();
                if (_audioSource != null)
                _audioSource.PlayOneShot(_leftSound);
                break;
            case ToggleDirecion.Center:
                _spriteRenderer.sprite = _centerSprite;
                _onCenter?.Invoke();
                break;
            case ToggleDirecion.Right:
                _spriteRenderer.sprite = _rightSprite;
                _onRight?.Invoke();
                if (_audioSource != null)
                    _audioSource.PlayOneShot(_rightSound);
                break;
            default:
                break;
        }
    }

    void OnValidate()
    {
        switch (_startingDirection)
        {
            case ToggleDirecion.Left:
                _spriteRenderer.sprite = _leftSprite;
                break;
            case ToggleDirecion.Center:
                _spriteRenderer.sprite = _centerSprite;
                break;
            case ToggleDirecion.Right:
                _spriteRenderer.sprite = _rightSprite;
                break;
            default:
                break;
        }
    }
}