using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIQuitButton : MonoBehaviour
{
    TMP_Text _text;
    // Start is called before the first frame update
    void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _text.SetText("Quit Game");

        #if true
        gameObject.SetActive(false);
        #endif
    }

    public void QuitGame()
    {
        Application.Quit();
        
        
    }


}
