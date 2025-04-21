using UnityEngine;
using Define;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    public Vector3 direction;
    public string from;

    Vector3 previousPosition;

    [SerializeField] BulletColor bulletColor;
    public BulletColor BulletColor => bulletColor;

    GameObject soundwave;
    Rigidbody2D rb;
    PlayerController _playerController;

    private void Start()
    {
        soundwave = Resources.Load<GameObject>("Prefabs/Soundwaves/SoundwaveWalk");
        rb = GetComponent<Rigidbody2D>();
        previousPosition = transform.position;
        _playerController = GameObject.FindFirstObjectByType<PlayerController>();
    }

    void FixedUpdate()
    {

        // 총알이 이동한 현재 위치
        Vector3 currentPosition = transform.position;

        // 레이캐스트로 충돌 확인
        if (gameObject != null)
        {
            CheckCollision(previousPosition, currentPosition);
        }

        // 이전 위치를 현재 위치로 갱신
        previousPosition = currentPosition;

        // 총알 이동
        rb.linearVelocity = transform.up * speed;
    }

    void CheckCollision(Vector3 start, Vector3 end)
    {
        // 이전 위치에서 현재 위치로 레이캐스트 발사
        Vector3 direction = end - start;  // 총알의 이동 방향

        RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction.normalized, direction.magnitude);
        // 모든 충돌 오브젝트를 순회
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                // 충돌한 오브젝트 처리
                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(this);
                }
                else if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.gameObject.GetComponent<PlayerController>().TakeDamage(this);
                }
                else if (hit.collider.CompareTag("WeakWall"))
                {
                    hit.collider.gameObject.GetComponent<WeakWall>().TakeDamage(this);
                }
                else if (hit.collider.CompareTag("Poro"))
                {
                    hit.collider.gameObject.GetComponent<Poro>().TakeDamage();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Field Of View Object"))
        {
            if(_playerController.PlayerType == 0)
                Instantiate(soundwave, transform.position, Quaternion.identity);
            //Debug.Log("Hit Wall");
            Destroy(gameObject);
        }
        
    }
}