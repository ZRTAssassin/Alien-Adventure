using System;
using UnityEngine;

public class FadingCloud : HittableFromBelow
{

}


public abstract class HittableFromBelow : MonoBehaviour
{
    [SerializeField] protected Sprite _usedSprite;
    Animator _animator;
    AudioSource _audioSource;

    protected virtual bool CanUse => true;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (CanUse == false)
            return;


        var player = collision.collider.GetComponent<Player>();
        if (player == null)
            return;

        if (collision.contacts[0].normal.y > 0)
        {
            PlayAnimation();
            PlayAudio();
            Use();
            if (CanUse == false)
                    GetComponent<SpriteRenderer>().sprite = _usedSprite;
        }
    }

    private void PlayAudio()
    {
        if (_audioSource != null)
            _audioSource.Play();
    }

    private void PlayAnimation()
    {
        if (_animator != null)
            _animator.SetTrigger("Use");
    }

    protected virtual void Use()
    {
        Debug.Log($"Used {gameObject.name}");
    }
}
