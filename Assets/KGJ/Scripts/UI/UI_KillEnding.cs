using System.Collections;
using UnityEngine;

public class UI_KillEnding : MonoBehaviour
{
    Canvas _canvas;
    CanvasGroup _canvasGroup;
    [SerializeField] float duration = 3f;

    Coroutine _coroutine;

    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas.enabled = false;
    }

    public void TurnOn()
    {
        Debug.Log("TurnOnKillEnding");
        _canvas.enabled = true;
        if (_coroutine == null)
            StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        _canvasGroup.alpha = 0;
        while (_canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha += Time.deltaTime / duration;
            yield return null;
        }
    }
}
