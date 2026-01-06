using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public enum UIContentType
{
    None,
    Perfume,
    IceCream,
    VanillaExtract
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private UIContentType currentContent = UIContentType.None;

    public GameObject panel;
    public GameObject perfumeContent;
    public GameObject iceCreamContent;
    public GameObject vanillaExtractContent;

    public VideoPlayer videoPlayer;
    public YouTubePlayPauseOverlay ytOverlay;
    public VideoSeekBarUI seekUI;



    private CanvasGroup canvasGroup;

    void Awake()
    {
        Instance = this;
        canvasGroup = panel.GetComponent<CanvasGroup>();
    }

    void Start()
    {
        HideAllInstant();
    }

    void Update()
    {
        if (panel.activeSelf)
        {
            panel.transform.LookAt(Camera.main.transform);
            panel.transform.Rotate(0, 180, 0);
        }
    }

    // ===================== FADE METHODS =====================

    IEnumerator Fade(float from, float to, float duration)
    {
        float time = 0f;
        canvasGroup.alpha = from;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, time / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    void ShowPanel()
    {
        panel.SetActive(true);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        StopAllCoroutines();
        StartCoroutine(Fade(0f, 1f, 0.25f));
    }

    void HidePanel()
    {
        StopAllCoroutines();
        StartCoroutine(HideAfterFade());
    }

    IEnumerator HideAfterFade()
    {
        yield return Fade(1f, 0f, 0.25f);
        panel.SetActive(false);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    // ===================== CONTENT =====================

    public void ShowPerfume()
    {
        if (currentContent == UIContentType.Perfume)
        {
            HideAll();
            return;
        }

        ShowPanel();

        perfumeContent.SetActive(true);
        iceCreamContent.SetActive(false);
        vanillaExtractContent.SetActive(false);

        videoPlayer.Stop();
        if (ytOverlay != null) ytOverlay.ForceHide();
        if (seekUI != null) seekUI.OnVideoClosed();


        currentContent = UIContentType.Perfume;
    }

    public void ShowIceCream(VideoClip clip)
    {
        if (currentContent == UIContentType.IceCream)
        {
            HideAll();
            return;
        }

        ShowPanel();

        perfumeContent.SetActive(false);
        iceCreamContent.SetActive(true);
        vanillaExtractContent.SetActive(false);

        if (seekUI != null)
        {
            seekUI.videoPlayer = videoPlayer;
            seekUI.OnVideoOpened(clip, autoplay: true);
        }
        else
        {
            videoPlayer.clip = clip;
            videoPlayer.Play();
        }

        if (ytOverlay != null)
        {
            ytOverlay.videoPlayer = videoPlayer; // za svaki slučaj
            ytOverlay.OnVideoAutoplayStarted();
        }


        currentContent = UIContentType.IceCream;
    }

    public void ShowVanillaExtract(VideoClip clip)
    {
        if (currentContent == UIContentType.VanillaExtract)
        {
            HideAll();
            return;
        }

        ShowPanel();

        perfumeContent.SetActive(false);
        iceCreamContent.SetActive(false);
        vanillaExtractContent.SetActive(true);

        if (seekUI != null)
        {
            seekUI.videoPlayer = videoPlayer;
            seekUI.OnVideoOpened(clip, autoplay: true);
        }
        else
        {
            videoPlayer.clip = clip;
            videoPlayer.Play();
        }
        if (ytOverlay != null)
        {
            ytOverlay.videoPlayer = videoPlayer; // za svaki slučaj
            ytOverlay.OnVideoAutoplayStarted();
        }

        currentContent = UIContentType.VanillaExtract;
    }

    public void HideAll()
    {
        perfumeContent.SetActive(false);
        iceCreamContent.SetActive(false);
        vanillaExtractContent.SetActive(false);

        videoPlayer.Stop();
        if (ytOverlay != null) ytOverlay.ForceHide();
        if (seekUI != null) seekUI.OnVideoClosed();

        currentContent = UIContentType.None;

        HidePanel();
    }

    // koristi se samo na Startu
    void HideAllInstant()
    {
        panel.SetActive(false);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
