using UnityEngine;

public class ARTopBarUI : MonoBehaviour
{
    [Header("Refs")]
    public SceneLoader sceneLoader;   // povuci svoj SceneLoader ovdje
    public GameObject infoPanel;      // povuci InfoPanel ovdje

    void Start()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    public void OnBackPressed()
    {
        // koristi tvoj postojeći SceneLoader
        if (sceneLoader != null)
            sceneLoader.LoadMainMenu();
        else
            Debug.LogWarning("SceneLoader nije povezan na ARTopBarUI!");
    }

    public void OnInfoPressed()
    {
        if (infoPanel == null)
        {
            Debug.LogWarning("InfoPanel nije povezan na ARTopBarUI!");
            return;
        }

        infoPanel.SetActive(!infoPanel.activeSelf);
    }

    // (Opcionalno) da imaš posebno dugme X na panelu
    public void CloseInfo()
    {
        if (infoPanel != null) infoPanel.SetActive(false);
    }
}
