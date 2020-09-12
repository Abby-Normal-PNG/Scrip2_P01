using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] KeyCode _reloadKey = KeyCode.Backspace;
    [SerializeField] KeyCode _quitKey = KeyCode.Escape;
    void Update()
    {
        CheckForReload();
        CheckForQuit();
    }

    private void CheckForReload()
    {
        if (Input.GetKeyDown(_reloadKey))
        {
            Scene _currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(_currentScene.name);
        }
    }

    private void CheckForQuit()
    {
        if (Input.GetKeyDown(_quitKey))
        {
            Application.Quit();
        }
    }

}
