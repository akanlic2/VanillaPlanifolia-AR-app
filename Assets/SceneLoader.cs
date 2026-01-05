using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadAR()
    {
        SceneManager.LoadScene("ARScene");
    }

    public void LoadQuiz()
    {
        SceneManager.LoadScene("QuizScene");
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
