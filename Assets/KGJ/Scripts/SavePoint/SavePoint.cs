using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private int savePointIndex; // 고유 인덱스 public Vector3 Position => transform.position; // 세이브 포인트 위치 public int Index => savePointIndex;
    GameObject _prefab;

    private void Start()
    {
        _prefab = Resources.Load<GameObject>("Particles/GetYou_Particle");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            SaveManager.Instance.SaveGame(savePointIndex, transform.position);
            Debug.Log($"세이브 포인트 {savePointIndex} 저장됨!");
            Instantiate(_prefab, transform.position, Quaternion.identity);
            Destroy(gameObject); // 세이브 포인트에 들어가면 오브젝트 삭제
        }
    }

}
