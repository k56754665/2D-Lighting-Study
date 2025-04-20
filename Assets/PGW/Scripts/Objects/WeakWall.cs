using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Components; // NavMeshPlus용 네임스페이스
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
                Debug.LogError("NavMeshSurface2D가 씬에 없습니다.");
            }
        }

        // NavMeshModifier 추가
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
            // 오브젝트 파괴 예약
            Destroy(gameObject);
            // NavMesh 업데이트를 프레임 후에 실행
            NavMeshManager.Instance.RequestNavMeshUpdate();
        }
    }
}