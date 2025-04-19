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

    private void Start()
    {
        soundwave = Resources.Load<GameObject>("Prefabs/Soundwaves/SoundwaveWalk");
        rb = GetComponent<Rigidbody2D>();
        previousPosition = transform.position;
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
        RaycastHit2D hit;
        Vector3 direction = end - start;  // 총알의 이동 방향

        hit = Physics2D.Raycast(start, direction, direction.magnitude);
        if (hit.collider != null)
        {
            // 충돌한 오브젝트가 있다면
            if (hit.collider.CompareTag("Field Of View Object"))
            {
                Instantiate(soundwave, transform.position, Quaternion.identity);
                //Debug.Log("Hit Wall");W
                Destroy(gameObject);
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(this);
            }
            else if(hit.collider.CompareTag("Player"))
            {
                hit.collider.gameObject.GetComponent<PlayerController>().TakeDamage(this);
            }
        }
    }
}