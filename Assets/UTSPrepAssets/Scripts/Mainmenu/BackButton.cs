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
        Debug.Log($"Current PlayerPrefs values: Level_0_Completed = {PlayerPrefs.GetInt("Level_0_Completed", 0)}, Level_1_Completed = {PlayerPrefs.GetInt("Level_1_Completed", 0)}");
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
