using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Components; // NavMeshPlus용 네임스페이스

public class WeakWall : MonoBehaviour
{
    [SerializeField] private int _hp = 1;
    [SerializeField] private NavMeshSurface navMeshSurface; //NavMeshSurface

    private void Start()
    {
        if (navMeshSurface == null)
        {
            navMeshSurface = FindObjectOfType<NavMeshSurface>();
            if (navMeshSurface == null)
            {
                Debug.LogError("NavMeshSurface2D가 씬에 없습니다.");
            }
        }
    }

    public void TakeDamage(Bullet bullet)
    {
        _hp--;
        if (_hp <= 0)
        {
            UpdateNavMesh();
            Destroy(gameObject);
        }
    }

    private void UpdateNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh(); // 2D NavMesh 업데이트
            Debug.Log("NavMesh가 업데이트되었습니다.");
        }
    }
}