using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class VideoSeekBarUI : MonoBehaviour
{
    private bool shouldAutoplay;

    [Header("Refs")]
    public VideoPlayer videoPlayer;

    [Header("UI")]
    public Slider slider;
    public TMP_Text currentTimeText;
    public TMP_Text totalTimeText;

    [Header("Visibility")]
    public CanvasGroup group;          // CanvasGroup na VideoControlsBar
    public float autoHideSeconds = 1.2f;

    bool isPrepared;
    bool isDragging;

    public bool IsDragging => isDragging;

    void Awake()
    {
        if (group == null) group = GetComponent<CanvasGroup>();
        if (slider == null) slider = GetComponentInChildren<Slider>();

        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
        }

        SetVisible(false);
        UpdateTimeTexts(0, 0);

        if (videoPlayer != null)
            videoPlayer.prepareCompleted += OnPrepared;
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.prepareCompleted -= OnPrepared;
    }

    // UIManager zove kad otvori video content
    public void OnVideoOpened(VideoClip clip, bool autoplay)
    {
        if (videoPlayer == null || slider == null) return;

        shouldAutoplay = autoplay;

        isPrepared = false;
        isDragging = false;

        CancelInvoke(nameof(AutoHide));
        SetVisible(false);

        slider.value = 0f;
        UpdateTimeTexts(0, 0);

        videoPlayer.Stop();
        videoPlayer.clip = clip;
        videoPlayer.Prepare();
    }

    public void OnVideoClosed()
    {
        isPrepared = false;
        isDragging = false;

        CancelInvoke(nameof(AutoHide));

        if (videoPlayer != null)
            videoPlayer.Stop();

        if (slider != null)
            slider.value = 0f;

        UpdateTimeTexts(0, 0);
        SetVisible(false);
    }

    void OnPrepared(VideoPlayer vp)
    {
        isPrepared = true;

        if (shouldAutoplay)
            vp.Play();

        UpdateTimeTexts(vp.time, vp.length);
    }

    void Update()
    {
        // ažuriraj samo kad je vidljivo
        if (group == null || group.alpha <= 0.001f) return;
        if (videoPlayer == null || slider == null) return;
        if (!isPrepared) return;

        if (!isDragging)
        {
            double len = videoPlayer.length;
            double t = videoPlayer.time;

            slider.value = (len > 0.01) ? (float)(t / len) : 0f;
            UpdateTimeTexts(t, len);
        }
    }

    // EventTrigger BeginDrag
    public void BeginDrag()
    {
        if (!isPrepared) return;

        isDragging = true;
        CancelInvoke(nameof(AutoHide));
        SetVisible(true);
    }

    // EventTrigger EndDrag
    public void EndDrag()
    {
        if (videoPlayer == null || slider == null || !isPrepared)
        {
            isDragging = false;
            return;
        }

        double len = videoPlayer.length;
        double newTime = slider.value * len;

        videoPlayer.time = newTime;
        UpdateTimeTexts(newTime, len);

        isDragging = false;

        // kao YouTube: nakon puštanja slidera, kontrole se sakriju nakon kratko
        ShowBriefly();
    }

    // Slider OnValueChanged (live preview dok vuče)
    public void OnSliderValueChanged(float value)
    {
        if (!isPrepared || videoPlayer == null) return;
        if (!isDragging) return;

        double len = videoPlayer.length;
        double preview = value * len;
        UpdateTimeTexts(preview, len);
    }

    // Overlay zove kad treba pokazati kontrole
    public void ShowBriefly()
    {
        SetVisible(true);
        CancelInvoke(nameof(AutoHide));

        // ne sakrivaj dok korisnik vuče
        if (!isDragging)
            Invoke(nameof(AutoHide), autoHideSeconds);
    }

    public void ForceHide()
    {
        CancelInvoke(nameof(AutoHide));
        SetVisible(false);
    }

    void AutoHide()
    {
        if (!isDragging)
            SetVisible(false);
    }

    void SetVisible(bool visible)
    {
        if (group == null) return;

        group.alpha = visible ? 1f : 0f;
        group.interactable = visible;
        group.blocksRaycasts = visible;
    }

    void UpdateTimeTexts(double current, double total)
    {
        if (currentTimeText != null) currentTimeText.text = FormatTime(current);
        if (totalTimeText != null) totalTimeText.text = FormatTime(total);
    }

    // Format 0:00 (minute bez vodeće nule)
    string FormatTime(double seconds)
    {
        if (seconds < 0 || double.IsNaN(seconds) || double.IsInfinity(seconds))
            seconds = 0;

        int s = Mathf.FloorToInt((float)seconds);
        int m = s / 60;
        s = s % 60;

        return $"{m}:{s:00}";
    }
}
