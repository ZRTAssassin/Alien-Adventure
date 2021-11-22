using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UIGameStatePanel : MonoBehaviour
{

    [SerializeField] GameObject _losePanel;
    // Start is called before the first frame update
    void Start()
    {
        Player.OnPlayerDeath += TurnOnPanel;
    }

    void TurnOnPanel()
    {
        _losePanel.SetActive(true);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        Player.OnPlayerDeath -= TurnOnPanel;
    }
}
