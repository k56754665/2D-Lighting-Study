using UnityEngine;

public class Poro : MonoBehaviour
{
    PoroDialog _poroDialog;
    int hp = 4;
    GameObject _blood;

    void Start()
    {
        _poroDialog = transform.GetChild(0).GetComponent<PoroDialog>();
        _blood = Resources.Load<GameObject>("Prefabs/KGJ/Blood");
    }

    public void TakeDamage()
    {
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 1f; // 1f 반경 원 안의 랜덤 위치
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
        Instantiate(_blood, spawnPosition, transform.rotation);

        _poroDialog.Shoot();
        hp--;
        if (hp <= 0)
        {
            // TODO : 엔딩 씬 켜기
            Debug.Log("Kill Ending");
            Destroy(gameObject);
        }
    }

}
