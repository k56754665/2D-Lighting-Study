using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance => _instance;
    static EnemyManager _instance;

    Enemies _enemyRoot;
    float _deleteDistance = 40f; // 비활성화 거리

    PlayerController _player;

    public Dictionary<Enemy, bool> EnemyStatus
    {
        get => new Dictionary<Enemy, bool>(_enemyStatus); // 얕은 복사본 반환
        set { _enemyStatus = value; } // 값만 가져와야 함
    }
    Dictionary<Enemy, bool> _enemyStatus = new Dictionary<Enemy, bool>(); // 저장 시점의 적 생존 상태를 저장할 딕셔너리

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public void Init()
    {
        _player = GameObject.FindFirstObjectByType<PlayerController>();
        _enemyRoot = FindAnyObjectByType<Enemies>();
        Enemy[] _enemiesArray = _enemyRoot.GetComponentsInChildren<Enemy>();
        for (int i = 0; i < _enemiesArray.Length; i++)
        {
            if (!_enemyStatus.ContainsKey(_enemiesArray[i]))
                _enemyStatus.Add(_enemiesArray[i], true); // 적 생존 상태를 딕셔너리에 추가
        }
    }

    void Update()
    {
        if (_player == null) return;

        foreach (var enemy in _enemyStatus.Keys)
        {
            if (enemy != null)
            {
                Enemy enemyController = enemy.GetComponent<Enemy>();
                if (enemyController != null)
                {
                    // 적이 파괴되지 않았을 경우
                    if (_enemyStatus[enemyController])
                    {
                        // 적의 생존 상태를 업데이트
                        enemy.NavMeshEnemyOnOff(_player.transform, _deleteDistance);
                    }
                }
            }
        }
    }

    public void ClearDictionary()
    {
        _enemyStatus.Clear();
    }

    /// <summary>
    /// 죽은 적의 상태를 딕셔너리에 반영해주는 함수
    /// </summary>
    public void AddDeadEnemyStatus(Enemy enemy)
    {
        _enemyStatus[enemy] = false;
    }


    public GameObject CheckClosestEnemy()
    {
        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (var enemy in _enemyStatus.Keys)
        {
            if (_enemyStatus[enemy] && _player != null)
            {
                float distance = Vector3.Distance(_player.transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }
        if (closestEnemy != null)
        {
            return closestEnemy.gameObject;
        }
        else
        {
            return null;
        }
    }
}
