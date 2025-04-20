using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private int savePointIndex; // 고유 인덱스 public Vector3 Position => transform.position; // 세이브 포인트 위치 public int Index => savePointIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            SaveManager.Instance.SaveGame(savePointIndex, transform.position);
            Debug.Log($"세이브 포인트 {savePointIndex} 저장됨!");
        }
    }

}
