using UnityEngine;

public class AimLineController : MonoBehaviour
{
    LineRenderer _line;
    PlayerController _playerController;
    PlayerInteraction _playerInteraction;
    Transform _target;

    [SerializeField] LayerMask _fieldOfViewLayer;
    [SerializeField] float _followSpeed = 10f; // 목표 위치로 얼마나 빨리 따라갈지
    [SerializeField] float _maxDistance = 10f; // 조준선 최대 사거리

    Vector2 _targetPosition; // 목표 위치 캐싱

    void Start()
    {
        _target = transform.GetChild(0);
        _line = GetComponent<LineRenderer>();
        _playerController = FindAnyObjectByType<PlayerController>();
        _playerInteraction = FindAnyObjectByType<PlayerInteraction>();
        _target.GetComponent<SpriteRenderer>().enabled = true;
        _line.enabled = true;

        _targetPosition = _target.position;

        _playerInteraction.OnClosetEnterEvent += HideTarget;
        _playerInteraction.OnClosetExitEvent += ShowTarget;
    }

    void LateUpdate()
    {
        if (_playerController == null)
        {
            _line.enabled = false;
            _target.GetComponent<SpriteRenderer>().enabled = false;
            return;
        }

        Vector2 startPosition = _playerController.transform.GetChild(0).position;
        Vector2 pointerPosition = InputManager.Instance.PointerMoveInput;
        Vector2 direction = (pointerPosition - startPosition).normalized;

        // 최대 사거리와 포인터 거리 중 작은 값을 사용
        float distance = Mathf.Min(Vector2.Distance(startPosition, pointerPosition), _maxDistance);

        // 레이캐스트에 최대 사거리 적용
        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, distance, _fieldOfViewLayer);

        Vector2 endPosition = startPosition + direction * distance; // 기본적으로 최대 사거리까지

        if (hit.collider != null)
        {
            endPosition = hit.point; // 충돌 지점으로 설정
        }

        // 목표 위치를 업데이트하고, Lerp로 부드럽게 이동
        _targetPosition = Vector2.Lerp(_targetPosition, endPosition, Time.deltaTime * _followSpeed);

        _line.SetPosition(0, startPosition);
        _line.SetPosition(1, _targetPosition);
        _target.transform.position = _targetPosition;
    }

    void ShowTarget()
    {
        _line.enabled = true;
        _target.GetComponent<SpriteRenderer>().enabled = true;
    }

    void HideTarget()
    {
        _line.enabled = false;
        _target.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnDestroy()
    {
        _playerInteraction.OnClosetEnterEvent -= HideTarget;
        _playerInteraction.OnClosetExitEvent -= ShowTarget;
    }
}
