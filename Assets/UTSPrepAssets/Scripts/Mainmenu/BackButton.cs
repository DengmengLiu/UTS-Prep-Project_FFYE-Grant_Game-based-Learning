using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "MainMenu";
    [SerializeField] private int LevelNum;
    

    public void switchScene()
    {
        LevelCompletionManager.Instance.MarkLevelAsCompleted(LevelNum);
        StartCoroutine(LoadNextScene(sceneToLoad));
    }
    private System.Collections.IEnumerator LoadNextScene(string name)
    {
        yield return new WaitForSeconds(0.2f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(name);
    }
}
