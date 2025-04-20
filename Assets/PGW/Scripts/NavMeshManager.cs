using UnityEngine;
using NavMeshPlus.Components;
using System.Collections;

public class NavMeshManager : MonoBehaviour
{
    static NavMeshManager _instance; 
    public static NavMeshManager Instance => _instance; // Singleton �ν��Ͻ�

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

        // NavMeshSurface�� ������ ������ ã��
        if (navMeshSurface == null)
        {
            navMeshSurface = FindAnyObjectByType<NavMeshSurface>();
            if (navMeshSurface == null)
            {
                Debug.LogError("NavMeshSurface�� ���� �����ϴ�!");
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
        // �� ������ ����ؼ� Destroy�� ó���� �ð� Ȯ��
        yield return new WaitForSeconds(0.1f); // 0.1�� ���
        navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
        Debug.Log("NavMesh ������Ʈ �Ϸ�");
    }
}
