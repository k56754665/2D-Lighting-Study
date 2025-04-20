using UnityEngine;
using NavMeshPlus.Components;
using System.Collections;

public class NavMeshManager : MonoBehaviour
{
    static NavMeshManager _instance; 
    public static NavMeshManager Instance => _instance; // Singleton 인스턴스

    NavMeshSurface navMeshSurface;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // NavMeshSurface가 없으면 씬에서 찾기
        if (navMeshSurface == null)
        {
            navMeshSurface = FindAnyObjectByType<NavMeshSurface>();
            if (navMeshSurface == null)
            {
                Debug.LogError("NavMeshSurface가 씬에 없습니다!");
            }
        }
    }

    public void RequestNavMeshUpdate()
    {
        if (_instance != null && _instance.navMeshSurface != null)
        {
            Debug.Log("not null");
            _instance.StartCoroutine(_instance.UpdateNavMeshDelayed());
        }
    }

    private IEnumerator UpdateNavMeshDelayed()
    {
        // 한 프레임 대기해서 Destroy가 처리될 시간 확보
        yield return new WaitForSeconds(0.1f); // 0.1초 대기
        navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
        Debug.Log("NavMesh 업데이트 완료");
    }
}
