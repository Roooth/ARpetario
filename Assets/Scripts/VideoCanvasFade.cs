
using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class VideoCanvasFade : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void OnEnable()
    {
        canvasGroup.alpha = 1f;
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
