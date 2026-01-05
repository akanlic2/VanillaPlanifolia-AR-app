using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class QuizQuestion
{
    public string question;
    public string[] answers;   // 4 odgovora
    public int correctIndex;   // 0-3

    public QuizQuestion(string q, string a0, string a1, string a2, string a3, int correct)
    {
        question = q;
        answers = new string[4] { a0, a1, a2, a3 };
        correctIndex = correct;
    }
}

public class QuizManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject quizPanel;
    public GameObject resultPanel;

    [Header("Question UI")]
    public TMP_Text questionText;
    public TMP_Text progressText;

    [Header("Answers UI (4)")]
    public Button[] answerButtons;   // 4
    public TMP_Text[] answerTexts;   // 4

    [Header("Result UI")]
    public TMP_Text resultText;
    public Button retryButton;
    public Button mainMenuButton;

    [Header("Navigation")]
    public SceneLoader sceneLoader;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip correctSfx;
    public AudioClip wrongSfx;

    [Header("Timing")]
    public float nextDelay = 0.8f;

    private List<QuizQuestion> questions = new();
    private int currentIndex = 0;
    private int score = 0;
    private bool answered = false;

    void Start()
    {
        BuildQuestions();

        if (quizPanel != null) quizPanel.SetActive(true);
        if (resultPanel != null) resultPanel.SetActive(false);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int idx = i;
            answerButtons[i].onClick.AddListener(() => OnAnswerClicked(idx));
        }

        if (retryButton != null) retryButton.onClick.AddListener(RestartQuiz);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(GoMainMenu);

        ShowQuestion();
    }

    void BuildQuestions()
    {
    questions.Clear();

    // --- VANILLA EXTRACT ---

    questions.Add(new QuizQuestion(
        "HOW MANY VANILLA BEANS ARE USED FOR EXTRACT?",
        "12", "5", "3", "20", 0));

    questions.Add(new QuizQuestion(
        "WHY ARE VANILLA BEANS SPLIT?",
        "STRONGER FLAVOR", "FASTER DRYING", "BETTER COLOR", "LESS AROMA", 0));

    questions.Add(new QuizQuestion(
        "WHICH ALCOHOL IS RECOMMENDED?",
        "VODKA", "RAKIJA", "WHISKEY", "LIQUEURS", 0));

    questions.Add(new QuizQuestion(
        "HOW LONG SHOULD THE EXTRACT SIT?",
        "A FEW MONTHS", "1 DAY", "1 WEEK", "1 HOUR", 0));

    questions.Add(new QuizQuestion(
        "WHAT IS USED TO MAKE VANILLA EXTRACT?",
        "JAR", "BOX", "POT", "FOIL", 0));

    // --- VANILLA ICE CREAM ---

    questions.Add(new QuizQuestion(
        "HOW MANY INGREDIENTS DOES THE ICE CREAM HAVE?",
        "5", "3", "6", "8", 0));

    questions.Add(new QuizQuestion(
        "WHICH PART OF THE EGG IS USED?",
        "YOLKS", "WHITES", "WHOLE EGG", "SHELL", 0));

    questions.Add(new QuizQuestion(
        "TO WHAT TEMPERATURE IS THE CUSTARD COOKED?",
        "80°C", "60°C", "100°C", "40°C", 0));

    questions.Add(new QuizQuestion(
        "WHAT IS DONE BEFORE THE ICE CREAM MACHINE?",
        "CHILLING", "BAKING", "MIXING", "FREEZING", 0));

    questions.Add(new QuizQuestion(
        "HOW LONG DOES ICE CREAM CHURNING TAKE?",
        "30 MINUTES", "5 MINUTES", "1 HOUR", "10 SECONDS", 0));

    // SHUFFLE ANSWERS
    ShuffleAnswersForAllQuestions();
    }


    void ShuffleAnswersForAllQuestions()
    {
        for (int i = 0; i < questions.Count; i++)
        {
            ShuffleOneQuestion(questions[i]);
        }
    }

    void ShuffleOneQuestion(QuizQuestion q)
    {
        // (tekst, jeTacno)
        var list = new List<(string text, bool correct)>(4);
        for (int i = 0; i < 4; i++)
            list.Add((q.answers[i], i == q.correctIndex));

        // Fisher–Yates shuffle
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }

        for (int i = 0; i < 4; i++)
        {
            q.answers[i] = list[i].text;
            if (list[i].correct) q.correctIndex = i;
        }
    }

    void ShowQuestion()
    {
        answered = false;

        if (questions == null || questions.Count == 0)
        {
            Debug.LogError("QuizManager: nema pitanja u listi!");
            return;
        }

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = true;

            var ui = answerButtons[i].GetComponent<AnswerButtonUI>();
            if (ui != null) ui.ResetState();
        }

        if (progressText != null)
            progressText.text = $"{currentIndex + 1}/{questions.Count}";

        var q = questions[currentIndex];

        if (questionText != null) questionText.text = q.question;

        for (int i = 0; i < 4; i++)
            answerTexts[i].text = q.answers[i];
    }

    void OnAnswerClicked(int chosenIndex)
    {
        if (answered) return;
        answered = true;

        var q = questions[currentIndex];
        bool isCorrect = (chosenIndex == q.correctIndex);

        for (int i = 0; i < answerButtons.Length; i++)
            answerButtons[i].interactable = false;

        var clickedUI = answerButtons[chosenIndex].GetComponent<AnswerButtonUI>();
        if (clickedUI != null)
        {
            if (isCorrect) clickedUI.SetCorrect();
            else clickedUI.SetWrong();
        }

        if (!isCorrect)
        {
            var correctUI = answerButtons[q.correctIndex].GetComponent<AnswerButtonUI>();
            if (correctUI != null) correctUI.SetCorrect();
        }

        if (isCorrect) score++;

        if (sfxSource != null)
            sfxSource.PlayOneShot(isCorrect ? correctSfx : wrongSfx);

        Invoke(nameof(NextQuestion), nextDelay);
    }

    void NextQuestion()
    {
        currentIndex++;

        if (currentIndex >= questions.Count)
        {
            ShowResult();
            return;
        }

        ShowQuestion();
    }

    void ShowResult()
    {
        if (quizPanel != null) quizPanel.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(true);

        if (resultText != null)
            resultText.text = $"RESULT:\n{score}/{questions.Count}";
    }

    public void RestartQuiz()
    {
        score = 0;
        currentIndex = 0;

        if (resultPanel != null) resultPanel.SetActive(false);
        if (quizPanel != null) quizPanel.SetActive(true);

        // Možeš i opet izmiješati redoslijed odgovora na restartu:
        ShuffleAnswersForAllQuestions();

        ShowQuestion();
    }

    public void GoMainMenu()
    {
        if (sceneLoader != null) sceneLoader.LoadMainMenu();
        else Debug.LogWarning("QuizManager: SceneLoader nije povezan!");
    }
}
