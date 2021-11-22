using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLauncher : MonoBehaviour
{
    [SerializeField] Fireball _fireballPrefab;
    [SerializeField] float _fireRate = 0.25f;

    Player _player;
    int _playerNumber;
    string _fireButton;
    string _horizontalAxis;
    float _nextFireTime;

    void Awake()
    {
        _player = GetComponent<Player>();
        _playerNumber = _player.PlayerNumber;
        _fireButton = $"P{_playerNumber}Fire";
        _horizontalAxis = $"P{_playerNumber}Horizontal";
    }

    void Update()
    {
        if (Input.GetButtonDown(_fireButton) && Time.time >= _nextFireTime)
        {
           // var horizontal = Input.GetAxis(_horizontalAxis);
            var fireball = Instantiate(_fireballPrefab, transform.position, Quaternion.identity);
            //Old way to do fireball direction. keeping in case I broke something 8/02/21 4:14 PM
            //fireball.Direction = horizontal >= 0 ? 1f : -1f;
            if (_player.IsFacingLeft) 
                fireball.Direction = -1f; 
            else
                fireball.Direction = 1f;
            
            _nextFireTime = Time.time + _fireRate;
        }
    }
}
