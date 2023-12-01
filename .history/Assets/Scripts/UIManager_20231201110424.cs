using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject instructions;
    public GameObject mainCanvas;
    public GameObject instructionsButton;
    public GameObject askRestart;
    public GameObject reverseButton;

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenInstructions()
    {
        instructions.SetActive(true);
        if (mainCanvas != null)
        {
            mainCanvas.SetActive(false);
        }
        
    }

    public void CloseInstructions()
    {
        instructions.SetActive(false);
        if (mainCanvas != null)
        {
            mainCanvas.SetActive(true);
        }
    }

    public void AskRestart()
    {
        askRestart.SetActive(true);
        reverseButton.SetActive(false);
        if (instructionsButton != null)
        {
            instructionsButton.SetActive(false);
        }
    }

    public void CloseAskRestart()
    {
        askRestart.SetActive(false);
        reverseButton.SetActive(true);
        if (instructions != null)
        {
            instructionsButton.SetActive(true);
        }
    }
}
