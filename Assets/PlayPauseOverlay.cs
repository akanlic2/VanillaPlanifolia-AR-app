using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class YouTubePlayPauseOverlay : MonoBehaviour
{
    [Header("Refs")]
    public VideoPlayer videoPlayer;

    [Header("UI")]
    public Button button;       // PlayPauseButton (Button)
    public Image iconImage;     // ista Image na PlayPauseButton-u

    [Header("Icons")]
    public Sprite playIcon;
    public Sprite pauseIcon;
    public VideoSeekBarUI seekUI;


    [Header("Timing")]
    public float showSeconds = 0.9f;

    Coroutine hideRoutine;

    void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        if (iconImage == null) iconImage = GetComponent<Image>();

        // U startu sakrij
        SetVisible(false);
    }

    // UIManager zove kad je otvoren video i autoplay krene
    public void OnVideoAutoplayStarted()
    {
        if (videoPlayer == null) return;
        ShowBriefly(videoPlayer.isPlaying); // video igra -> prikaži pause kratko
        if (seekUI != null) seekUI.ShowBriefly();

    }

    // UIManager zove kad je prikazan tekst / zatvoren panel
    public void ForceHide()
    {
        StopHide();
        SetVisible(false);
        if (seekUI != null) seekUI.ForceHide();

    }

    // Pozovi ovo kad korisnik tapne na video (RawImage)
    public void OnUserTappedVideo()
    {
        if (videoPlayer == null) return;
        ShowBriefly(videoPlayer.isPlaying);
        if (seekUI != null) seekUI.ShowBriefly();

    }

    // Povežeš na Button OnClick
    public void TogglePlayPause()
    {
        if (videoPlayer == null) return;

        if (videoPlayer.isPlaying) videoPlayer.Pause();
        else videoPlayer.Play();

        ShowBriefly(videoPlayer.isPlaying);
        if (seekUI != null) seekUI.ShowBriefly();

    }

    void ShowBriefly(bool isPlaying)
    {
        StopHide();

        // YouTube logika: kad video igra, overlay ikona je PAUSE (što se desi ako klikneš)
        if (iconImage != null)
            iconImage.sprite = isPlaying ? pauseIcon : playIcon;

        SetVisible(true);

        hideRoutine = StartCoroutine(AutoHide());
    }

    IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(showSeconds);
        SetVisible(false);
        hideRoutine = null;
    }

    void StopHide()
    {
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }
    }

    void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
