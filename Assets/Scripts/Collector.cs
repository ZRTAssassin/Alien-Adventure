using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using System;

public class Collector : MonoBehaviour
{
    [SerializeField] List<Collectible> _collectibles;
    [SerializeField] UnityEvent _onCollectionComplete;
    

    TMP_Text _remainingText;
    int _countCollected;

    static Color _gizmoColor = new Color(0.61f, 0.61f, 0.61f, 1);

    void Start()
    {
        _remainingText = GetComponentInChildren<TMP_Text>();

        foreach (var collectible in _collectibles)
        {
            collectible.OnPickedUp += ItemPickedUp;
        }

        int countRemaining = _collectibles.Count - _countCollected;

        _remainingText?.SetText(countRemaining.ToString());
    }

    public void ItemPickedUp()
    {
        _countCollected++;
        int countRemaining = _collectibles.Count - _countCollected;

        _remainingText?.SetText(countRemaining.ToString());

        if (countRemaining > 0)
            return;
        Debug.Log("Got all gems!");
        _onCollectionComplete.Invoke();
    }


    void OnValidate()
    {
        _collectibles = _collectibles.Distinct().ToList();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        foreach (var collectible in _collectibles)
        {
            Gizmos.DrawLine(transform.position, collectible.transform.position);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        foreach (var collectible in _collectibles)
        {
            Gizmos.DrawLine(transform.position, collectible.transform.position);
        }
    }
}
