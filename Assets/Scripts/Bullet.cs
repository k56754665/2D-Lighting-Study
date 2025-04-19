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

        // �Ѿ��� �̵��� ���� ��ġ
        Vector3 currentPosition = transform.position;

        // ����ĳ��Ʈ�� �浹 Ȯ��
        if (gameObject != null)
        {
            CheckCollision(previousPosition, currentPosition);
        }

        // ���� ��ġ�� ���� ��ġ�� ����
        previousPosition = currentPosition;

        // �Ѿ� �̵�
        rb.linearVelocity = transform.up * speed;
    }


    void CheckCollision(Vector3 start, Vector3 end)
    {
        // ���� ��ġ���� ���� ��ġ�� ����ĳ��Ʈ �߻�
        RaycastHit2D hit;
        Vector3 direction = end - start;  // �Ѿ��� �̵� ����

        hit = Physics2D.Raycast(start, direction, direction.magnitude);
        if (hit.collider != null)
        {
            // �浹�� ������Ʈ�� �ִٸ�
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