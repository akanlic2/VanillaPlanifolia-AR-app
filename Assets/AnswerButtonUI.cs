using UnityEngine;
using UnityEngine.UI;

public class AnswerButtonUI : MonoBehaviour
{
    public Image answerColor;   // povuci AnswerColor Image (child)

    void Awake()
    {
        ResetState();
    }

    public void ResetState()
    {
        if (answerColor == null) return;

        // ugasi overlay
        answerColor.gameObject.SetActive(false);
        answerColor.color = new Color(1f, 1f, 1f, 0f);
    }

    public void SetCorrect()
    {
        if (answerColor == null) return;

        answerColor.gameObject.SetActive(true);
        answerColor.color = new Color(0.2f, 0.8f, 0.3f, 1f); // zeleno
    }

    public void SetWrong()
    {
        if (answerColor == null) return;

        answerColor.gameObject.SetActive(true);
        answerColor.color = new Color(0.9f, 0.2f, 0.2f, 1f); // crveno
    }
}
