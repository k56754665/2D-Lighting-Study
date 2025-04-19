using UnityEngine;

public class SimpleWallMapLoader : MonoBehaviour
{
    public Texture2D mapImage;        // 도트 이미지
    public GameObject wallPrefab;     // 벽 프리팹
    public float tileSize = 1f;       // 타일 크기 (픽셀당 간격)

    void Start()
    {
        LoadWallsFromImage();
    }

    void LoadWallsFromImage()
    {
        for (int y = 0; y < mapImage.height; y++)
        {
            for (int x = 0; x < mapImage.width; x++)
            {
                Color pixelColor = mapImage.GetPixel(x, y);

                if (IsGray(pixelColor))
                {
                    Vector3 spawnPos = new Vector3(x * tileSize, y * tileSize, 0);
                    Instantiate(wallPrefab, spawnPos, Quaternion.identity);
                }
            }
        }
    }

    // 회색 계열 픽셀 필터 (R ≈ G ≈ B)
    bool IsGray(Color color)
    {
        float threshold = 0.1f;
        return Mathf.Abs(color.r - color.g) < threshold &&
               Mathf.Abs(color.g - color.b) < threshold &&
               color.r > 0.5f && color.r < 0.8f; // 회색 톤 범위
    }
}
