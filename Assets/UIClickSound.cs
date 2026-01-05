using UnityEngine;
using UnityEngine.SceneManagement;

public class UIClickSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    [Header("Delay prije akcije")]
    public float delay = 0.25f; // 0.2–0.3s je idealno

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    // SAMO ZVUK (ako treba)
    public void PlayClick()
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }

    // START AR
    public void StartAR()
    {
        PlayClick();
        Invoke(nameof(LoadAR), delay);
    }

    // QUIZ
    public void StartQuiz()
    {
        PlayClick();
        Invoke(nameof(LoadQuiz), delay);
    }

    // EXIT
    public void ExitApp()
    {
        PlayClick();
        Invoke(nameof(QuitApp), delay);
    }

    void LoadAR()
    {
        SceneManager.LoadScene("ARScene");
    }

    void LoadQuiz()
    {
        SceneManager.LoadScene("QuizScene");
    }

    void QuitApp()
    {
        Application.Quit();
    }
}
