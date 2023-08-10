using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScnenManager : MonoBehaviour
{
    public static ScnenManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void MoveScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void MoveGameScene()
    {
        SceneManager.LoadScene("Play");
    }

    public void MoveMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
