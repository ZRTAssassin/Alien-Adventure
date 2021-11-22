using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, ITakeDamage
{
    public static event Action OnPlayerDeath;

    [Header("Player Configuration")] [SerializeField]
    int _playerNumber = 1;

    [SerializeField] AudioClip _playerDeathClip;

    [SerializeField] Transform _startPosition;
    [SerializeField] int _currentHealth;
    [SerializeField] int _maxHealth = 2;
    [SerializeField] float _invincbilityTimer;

    [Header("Movement")] [SerializeField] float _speed = 1;
    [SerializeField] float _slipFactor = 1.0f;
    [Header("Jump")] [SerializeField] float _jumpVelocity = 2f;
    [SerializeField] int _maxJumps = 2;
    [SerializeField] float _downPull = 5.0f;
    [SerializeField] float _maxJumpDuration = 0.1f;


    [Header("Sensor Transforms")] [SerializeField]
    Transform _feet;

    [SerializeField] Transform _leftSensor;
    [SerializeField] Transform _rightSensor;

    [SerializeField] float _wallSlideSpeed = 1.0f;
    [SerializeField] float _acceleration = 1.0f;
    [SerializeField] float _breaking = 1.0f;
    [SerializeField] float _airAcceleration = 1.0f;
    [SerializeField] float _airBreaking = 1.0f;

    [Header("Layer Mask")] [SerializeField]
    LayerMask _layerMask;

    int _jumpsRemaining;
    float _fallTimer;
    float _jumpTimer;
    Rigidbody2D _rigidbody2D;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    float _horizontal;
    bool _isGrounded;
    bool _isOnSlipperySurface;
    string _jumpButton;
    string _horizontalAxis;
    AudioSource _audioSource;
    bool _isFacingLeft;
    bool _canTakeDamage = true;


    public int PlayerNumber => _playerNumber;

    public int CurrentHealth => _currentHealth;
    public bool IsFacingLeft => _isFacingLeft;

    void Start()
    {
        transform.position = _startPosition.position;
        _jumpsRemaining = _maxJumps;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _jumpButton = $"P{_playerNumber}Jump";
        _horizontalAxis = $"P{_playerNumber}Horizontal";
        _audioSource = GetComponent<AudioSource>();
        _currentHealth = _maxHealth;
        
    }



    void Update()
    {
        UpdateIsGrounded();
        ReadHorizontalInput();
       /* if (_isOnSlipperySurface)
            SlipHorizontal();
        else
            MoveHorizontal(); */

        UpdateAnimator();
        UpdateSpriteDirection();
        /* if (ShouldSlide())
        {
            if (ShouldStartJump())
                WallJump();
            else
                Slide();
            return;
        } */

        if (ShouldStartJump())
            Jump();
        else if (ShouldContinueJump())
            ContinueJump(); 

        _jumpTimer += Time.deltaTime;

        /*
        if (_isGrounded && _fallTimer > 0)
        {
            _fallTimer = 0;
            _jumpsRemaining = _maxJumps;
        }
        else
        {
            _fallTimer += Time.deltaTime;
            ApplyGravity();
        } */
    }

    void FixedUpdate()
    {
        if (_isOnSlipperySurface)
            SlipHorizontal();
        else
            MoveHorizontal();
        
        
        if (ShouldSlide())
        {
            if (ShouldStartJump())
                WallJump();
            else
                Slide();
            return;
        }

        
        
        if (_isGrounded && _fallTimer > 0)
        {
            _fallTimer = 0;
            _jumpsRemaining = _maxJumps;
        }
        else
        {
            _fallTimer += Time.deltaTime;
            ApplyGravity();
        }
    }

    void ApplyGravity()
    {
        var gravity = _downPull * _fallTimer; //* _fallTimer; Commented out for testing purposes
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y - gravity);
    }

    void WallJump()
    {
        _rigidbody2D.velocity = new Vector2(-_horizontal * _jumpVelocity, _jumpVelocity * 1.5f);
    }

    void Slide()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -_wallSlideSpeed);
    }

    bool ShouldSlide()
    {
        if (_isGrounded)
            return false;

        if (_rigidbody2D.velocity.y > 0)
            return false;

        if (_horizontal < 0)
        {
            var hit = Physics2D.OverlapCircle(_leftSensor.position, 0.1f);
            if (hit != null && hit.CompareTag("Wall"))
                return true;
        }

        if (_horizontal > 0)
        {
            var hit = Physics2D.OverlapCircle(_rightSensor.position, 0.1f);
            if (hit != null && hit.CompareTag("Wall"))
                return true;
        }

        return false;
    }

    void ContinueJump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpVelocity);
        _fallTimer = 0;
    }

    bool ShouldContinueJump()
    {
        return Input.GetButton(_jumpButton) && _jumpTimer <= _maxJumpDuration;
    }

    void Jump()
    {
        
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpVelocity);
        _jumpsRemaining--;
        // Debug.Log($"Jumps remaining {_jumpsRemaining}");
        _fallTimer = 0;
        _jumpTimer = 0;
        PlayAudio();
    }

    public void Bounce(float _bounceVelocity)
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _bounceVelocity);
        if (_jumpsRemaining == 0)
            _jumpsRemaining++;
        _fallTimer = 0;
        _jumpTimer = 0;
    }

    void PlayAudio()
    {
        if (_audioSource != null)
            _audioSource.Play();
    }

    bool ShouldStartJump()
    {
        return Input.GetButtonDown(_jumpButton) && _jumpsRemaining > 0;
    }

    void MoveHorizontal()
    {
        float smoothnessMultiplier = _horizontal == 0 ? _breaking : _acceleration;
        if (_isGrounded == false)
            smoothnessMultiplier = _horizontal == 0 ? _airBreaking : _airAcceleration;

        var newHorizontal = Mathf.Lerp(
            _rigidbody2D.velocity.x,
            _horizontal * _speed,
            Time.deltaTime * smoothnessMultiplier);

        _rigidbody2D.velocity = new Vector2(newHorizontal, _rigidbody2D.velocity.y);
    }

    void SlipHorizontal()
    {
        var desiredVelocity = new Vector2(_horizontal * _speed, _rigidbody2D.velocity.y);
        var smoothedVelocity = Vector2.Lerp(
            _rigidbody2D.velocity,
            desiredVelocity,
            Time.deltaTime / _slipFactor);
        _rigidbody2D.velocity = smoothedVelocity;
    }

    void ReadHorizontalInput()
    {
        _horizontal = Input.GetAxis(_horizontalAxis) * _speed;
    }

    void UpdateSpriteDirection()
    {
        if (_horizontal != 0)
        {
            _spriteRenderer.flipX = _horizontal < 0;
            _isFacingLeft = _spriteRenderer.flipX;
        }
    }

    void UpdateAnimator()
    {
        bool walking = _horizontal != 0;
        _animator.SetBool("Walk", walking);
        _animator.SetBool("Jump", ShouldContinueJump());
        _animator.SetBool("Slide", ShouldSlide());
        _animator.SetBool("Damaged", !_canTakeDamage);
    }

    void UpdateIsGrounded()
    {
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, _layerMask);
        _isGrounded = hit != null;

        if (hit != null)
            _isOnSlipperySurface = hit.CompareTag("Slippery");
        else
            _isOnSlipperySurface = false;
    }


    [ContextMenu("Take Damage")]
    public void TakeDamage(int amount)
    {
        if (_canTakeDamage == false)
            return;

        _canTakeDamage = false;


        StartCoroutine(InvincibilityFrames());

        _currentHealth -= amount;
        if (_currentHealth <= 0)
            PlayerDie();
    }

    internal void PlayerDie()
    {
        OnPlayerDeath?.Invoke();
        _audioSource.PlayOneShot(_playerDeathClip);
        GameManager.Instance.PauseGame();
        /* _rigidbody2D.position = _startPosition;
         SceneManager.LoadScene("Main Menu");*/
    }

    IEnumerator InvincibilityFrames()
    {
        yield return new WaitForSeconds(_invincbilityTimer);
        _canTakeDamage = true;
    }

    internal void TeleportTo(Vector3 position)
    {
        _rigidbody2D.position = position;
        _rigidbody2D.velocity = Vector2.zero;
    }
    
   

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_leftSensor.position, 0.1f);
        Gizmos.DrawSphere(_rightSensor.position, 0.1f);
    }
}