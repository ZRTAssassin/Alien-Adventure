using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Fireball : MonoBehaviour
{
    [SerializeField] float _launchForce = 5f;
    [SerializeField] float _bounceForce = 5f;
    [SerializeField] List<AudioClip> _clips;
    [SerializeField] ParticleSystem _particle;
    [SerializeField] int _damageAmount = 1;


    AudioSource _audioSource;
    SpriteRenderer _spriteRenderer;
    CircleCollider2D _circleCollider2D;
    Rigidbody2D _rigidbody2D;
    int _bouncesRemaining = 3;

    public float Direction { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _rigidbody2D.velocity = new Vector2(_launchForce * Direction, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        TryPlaySoundEffect();

        ITakeDamage damageable = collision.collider.GetComponent<ITakeDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damageAmount);
            StartCoroutine(FinishBeforeDeath());
            _particle.Play();
            return;
        }


        _bouncesRemaining--;
        if (_bouncesRemaining < 0)
            StartCoroutine(FinishBeforeDeath());
        else
            _rigidbody2D.velocity = new Vector2(_launchForce * Direction, _bounceForce);
    }


    IEnumerator FinishBeforeDeath()
    {
        _spriteRenderer.enabled = false;
        _circleCollider2D.enabled = false;
        _rigidbody2D.simulated = false;
        TryPlaySoundEffect();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void TryPlaySoundEffect()
    {
        if (_clips.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, _clips.Count);
            AudioClip clip = _clips[randomIndex];
            _audioSource.PlayOneShot(clip);
        }
    }
}