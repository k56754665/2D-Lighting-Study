using UnityEngine;

public class Blood : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        int bloodType = UnityEngine.Random.Range(1, 5);
        _spriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/Blood{bloodType}");
    }
}
