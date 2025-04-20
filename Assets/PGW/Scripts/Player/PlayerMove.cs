using UnityEngine;
using Define;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D _rb;
    PlayerController _playerController;
    PlayerAnimatorController _playerAnimatorController;
    PlayerSound _playerSound;

    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] float _focusMoveSpeed = 2f;
    Vector2 _moveDir;

    // 음파 가져오기
    [SerializeField] GameObject soundwaveWalk;
    [SerializeField] GameObject soundwaveRun;
    float lastSoundwaveTime = 0f; // 마지막 음파 생성 시간
    float soundwaveInterval = 0.4f; // 음파 생성 간격 (1초)
    public float SoundwaveInterval { get => soundwaveInterval; set => soundwaveInterval = value; }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerController = GetComponent<PlayerController>();
        _playerAnimatorController = GetComponent<PlayerAnimatorController>();
        _playerSound = GetComponent<PlayerSound>();
    }

    public void Move()
    {
        if (_playerController == null || _playerAnimatorController == null) return;

        _moveDir = InputManager.Instance.MoveInput.normalized;

        // 음파 생성 조건
        if (_moveDir != Vector2.zero)
        {
            _playerAnimatorController.SetAnimatorBool("isMoving", true);
            _playerSound.PlayWalkSound();

            if (InputManager.Instance.IsFocusing)
            {
                _rb.linearVelocity = _moveDir.normalized * _focusMoveSpeed * _playerController.RunMultiply;
            }
            else
            {
                _rb.linearVelocity = _moveDir.normalized * _moveSpeed * _playerController.RunMultiply;
                
            }
            // 음파 생성
            if (Time.time - lastSoundwaveTime >= soundwaveInterval) // 음파 생성 간격 확인
            {
                GameObject soundwave = _playerController.CurrentState == PlayerState.Run ? soundwaveRun : soundwaveWalk;
                GameObject newSoundwave = Instantiate(soundwave, transform.position, transform.rotation);

                // 음파 생성 후 마지막 생성 시간 업데이트
                lastSoundwaveTime = Time.time;
            }
        }
        else
        {
            _playerAnimatorController.SetAnimatorBool("isMoving", false);
            _playerSound.StopWalkSound();
            _rb.linearVelocity = Vector2.zero; // 방향 없을 때 속도 0
        }
    }
}