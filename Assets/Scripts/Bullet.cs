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
        Vector3 direction = end - start;  // �Ѿ��� �̵� ����

        RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction.normalized, direction.magnitude);
        // ��� �浹 ������Ʈ�� ��ȸ
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                // �浹�� ������Ʈ ó��
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