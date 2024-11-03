using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "MainMenu";
    

    public void switchScene()
    {
        
        StartCoroutine(LoadNextScene(sceneToLoad));
    }
    private System.Collections.IEnumerator LoadNextScene(string name)
    {
        yield return new WaitForSeconds(0.5f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(name);
    }
}
