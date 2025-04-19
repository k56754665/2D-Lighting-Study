using UnityEngine;
using System.Collections;

public class HpShake : MonoBehaviour
{
    RectTransform target; // 흔들고 싶은 RectTransform
    public float duration = 0.5f; // 흔들리는 시간
    public float magnitude = 10f; // 흔들림 강도 (픽셀 단위)

    void Start()
    {
        target = GetComponent<RectTransform>();
    }

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        Vector3 originalPos = target.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            target.anchoredPosition = originalPos + new Vector3(offsetX, offsetY);

            elapsed += Time.deltaTime;
            yield return null;
        }

        target.anchoredPosition = originalPos; // 원래 위치로 복귀
    }
}
