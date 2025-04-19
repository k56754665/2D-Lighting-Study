using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Components; // NavMeshPlus�� ���ӽ����̽�

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
                Debug.LogError("NavMeshSurface2D�� ���� �����ϴ�.");
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
            navMeshSurface.BuildNavMesh(); // 2D NavMesh ������Ʈ
            Debug.Log("NavMesh�� ������Ʈ�Ǿ����ϴ�.");
        }
    }
}