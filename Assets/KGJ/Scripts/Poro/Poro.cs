using UnityEngine;
using System;

public class Poro : MonoBehaviour
{
    PoroDialog _poroDialog;
    int hp = 4;
    GameObject _blood;
    public Action<int> OnPoroHitEvent;
    UI_KillEnding _killEndingUI;

    void Start()
    {
        _poroDialog = transform.GetChild(0).GetComponent<PoroDialog>();
        _blood = Resources.Load<GameObject>("Prefabs/KGJ/Blood");
        _killEndingUI = FindAnyObjectByType<UI_KillEnding>();
    }

    public void TakeDamage()
    {
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 1f; // 1f �ݰ� �� ���� ���� ��ġ
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
        Instantiate(_blood, spawnPosition, transform.rotation);

        _poroDialog.Shoot();
        hp--;
        OnPoroHitEvent?.Invoke(hp);
        if (hp <= 0)
        {
            // TODO : ����� �α� ��Ŀ� �°� ���� �ʿ�
            _killEndingUI.TurnOn();
            Debug.Log("Kill Ending");
            Destroy(gameObject);
        }
    }

}
