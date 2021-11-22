using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    [SerializeField] string _sceneName;
    [SerializeField] GameObject _panel;
    [SerializeField] AudioClip _audioClip;
    
    AudioSource _audioSource;
    Animator _animator;
    
    int _currentScene;
    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.WasTriggeredByPlayer())
            return;

         
        _animator.SetTrigger("Raise");
        
        StartCoroutine(LoadAfterDelay());
    }

     IEnumerator LoadAfterDelay()
     {
         _audioSource.PlayOneShot(_audioClip);
         yield return new WaitForSeconds(_audioClip.length);
         _panel.SetActive(true);
         GameManager.Instance.PauseGame();

         
         /*PlayerPrefs.SetInt(_sceneName + "Unlocked", 1);
        yield return new WaitForSeconds(1.5f);
        HandleSceneTransition(); */
         //SceneManager.LoadScene(_sceneName);
     }

     void Awake()
     {
         _currentScene = SceneManager.GetActiveScene().buildIndex;
         _audioSource = GetComponent<AudioSource>();
         _animator = GetComponent<Animator>();
     }

     void HandleSceneTransition()
     {
         
         _currentScene++;
         if (_currentScene == 4)
         {
             SceneManager.LoadScene(0);
         }
         else
         {
             SceneManager.LoadScene(_currentScene);
         }
     }
}