using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Components; // NavMeshPlus�� ���ӽ����̽�
using System.Collections;

public class WeakWall : MonoBehaviour
{
    [SerializeField] int _hp = 1;
    [SerializeField] NavMeshSurface _navMeshSurface; //NavMeshSurface

    void Start()
    {
        if (_navMeshSurface == null)
        {
            _navMeshSurface = FindObjectOfType<NavMeshSurface>();
            if (_navMeshSurface == null)
            {
                Debug.LogError("NavMeshSurface2D�� ���� �����ϴ�.");
            }
        }

        // NavMeshModifier �߰�
        if (!GetComponent<NavMeshModifier>())
        {
            NavMeshModifier modifier = gameObject.AddComponent<NavMeshModifier>();
            modifier.overrideArea = true;
            modifier.area = 1; // "Not Walkable"
        }
    }

    public void TakeDamage(Bullet bullet)
    {
        _hp--;
        if (_hp <= 0)
        {
            // ������Ʈ �ı� ����
            Destroy(gameObject);
            // NavMesh ������Ʈ�� ������ �Ŀ� ����
            NavMeshManager.Instance.RequestNavMeshUpdate();
        }
    }
}