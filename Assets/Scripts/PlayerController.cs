using UnityEngine;
using Define;
using System;

public class PlayerController : MonoBehaviour
{
    PlayerState _currentState = PlayerState.Walk;
    [SerializeField] Target _targetType = Target.None;

    [SerializeField] ParticleSystem deathParticle;

    Canvas_Script _canvas;
    GameManager _gameManager;
    [SerializeField] GameObject _target;
    [SerializeField] GameObject _lastTarget;
    PlayerMove _playerMove;
    PlayerInteraction _playerInteraction;
    SpriteRenderer _spriteRenderer;
    PlayerAnimatorController _playerAnimatorController;
    PlayerSound _playerSound;

    [SerializeField] float _playerType = 0; // 0: A테스트   , 1: B테스트
    public float PlayerType => _playerType; // 플레이어 타입

    public int hp;
    public int maxHp;
    float runMultiply = 1;

    Vector3 _startRunPosition;
    Vector3 _endRunPosition;
    float _startRunTime;

    public PlayerState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Target TargetType { get { return _targetType; } set { _targetType = value; } }
    public GameObject CurrentTarget { get { return _target; } set { _target = value; } }
    public float RunMultiply => runMultiply;

    public Action<int> OnHpDownEvent;

    void Start()
    {
        hp = maxHp;
        runMultiply = 1;

        _canvas = GameObject.FindFirstObjectByType<Canvas_Script>();
        _playerMove = GetComponent<PlayerMove>();
        _playerInteraction = GetComponent<PlayerInteraction>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimatorController = GetComponent<PlayerAnimatorController>();
        _gameManager = GameManager.Instance;
        _playerSound = GetComponent<PlayerSound>();
        InputManager.Instance.runAction += PlayerRun;
        InputManager.Instance.stopRunAction += StopRun;
    }

    private void LateUpdate()
    {
        // 이동
        if (_currentState != PlayerState.Interaction)
        {
            _playerMove.Move();
        }
    }
    void Update()
    {

        SetHealth();


        if (!_gameManager.IsGameOver)
        {
            // 플레이어 상태에 따른 속도 배수 변화
            switch (_currentState)
            {
                case PlayerState.Walk:
                    runMultiply = 1f;
                    _playerMove.SoundwaveInterval = 0.4f;
                    break;
                case PlayerState.Run:
                    runMultiply = 2f;
                    _playerMove.SoundwaveInterval = 0.15f;
                    break;
                case PlayerState.Interaction:
                    runMultiply = 0;
                    _playerMove.SoundwaveInterval = 0.4f;
                    break;
            }

            

            // 플레이어 각도
            Vector3 mousePosition = InputManager.Instance.PointerMoveInput;
            Vector2 direction = (mousePosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
            _spriteRenderer.flipX = direction.x < 0;

            // 플레이어 위치, 각도와 Field of View 위치, 각도 동기화
            //fieldOfView.SetAimDirection(UtilsClass.GetVectorFromAngle(angle + 90));
            //fieldOfView.SetOrigin(this.transform.position);

            // 플레이어 체력이 적으면 UI표시
            if (hp == 1)
            {
                _canvas.lowHp_UI.SetActive(true);
            }
            else
            {
                _canvas.lowHp_UI.SetActive(false);
            }

            if (hp < 1)
            {
                _playerAnimatorController.PlayAnimation("PlayerDead");
            }

            if (_playerInteraction.IsInCloset) return;

            if(_target != _lastTarget)
            {
                if (_lastTarget)
                {
                    _lastTarget.GetComponentInChildren<EnemyUIController_Script>().HideUI();
                }
            }
            if (_target)
            {
                _lastTarget = _target;
                _target.GetComponentInChildren<EnemyUIController_Script>().ShowUI();
            }
            else
            {
                _lastTarget = null;
            }
        }

        if(EnemyManager.Instance.CheckClosestEnemy())
        {
            GameObject closestEnemy = EnemyManager.Instance.CheckClosestEnemy();
            if(_playerInteraction.CheckAssassinateCondition(closestEnemy))
            {
                _targetType = Target.Enemy;
                _target = closestEnemy;
                _playerInteraction.ShowEKeyUI(true);
            }
            else if(_target && _targetType == Target.Enemy)
            {
                _targetType = Target.None;
                _playerInteraction.ShowEKeyUI(false);
            }
        }
    }

    void PlayerRun()
    {
        if (!_playerInteraction.IsInCloset)
        {
            _currentState = PlayerState.Run;
            _startRunPosition = transform.position;
            _startRunTime = Time.time;
        }
    }

    void StopRun()
    {
        if (_currentState == PlayerState.Run)
        {
            _currentState = PlayerState.Walk;
            _endRunPosition = transform.position;
            float runDuration = Time.time - _startRunTime;
            Debug.Log("Run|" + runDuration + "|Startx=" + _startRunPosition.x + "|Starty=" + _startRunPosition.y + "|Endx=" + _endRunPosition.x + "|Endy=" + _endRunPosition.y);
        }
    }

    public void ExitDeadAnimation()
    {
        GameManager.Instance.Gameover();
        TriggerDeath();
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    // 체력을 설정하는 메서드
    public void SetHealth()
    {
        // 체력이 최대 체력을 초과하지 않도록 제한
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        // 체력이 0 이하로 떨어지지 않도록 설정
        if (hp < 0)
        {
            hp = 0; 
        }
    }

    void TriggerDeath() 
    {
        if (deathParticle != null)
        {
            ParticleSystem death = Instantiate(deathParticle, transform.position, Quaternion.identity);
            death.Play();
            Destroy(death.gameObject, 2f);
        }
    }

    public void TakeDamage(Bullet bullet)
    {
        if (bullet.BulletColor == BulletColor.Yellow)
        {
            _playerSound.PlayHitSound();
            Destroy(bullet.gameObject);
            hp -= 1;
            OnHpDownEvent?.Invoke(hp);
            Instantiate(deathParticle, transform.position, transform.rotation);
        }
    }
    public void TakeBombDamage()
    {
        _playerSound.PlayHitSound();
        hp -= 3;
        OnHpDownEvent?.Invoke(2);
        OnHpDownEvent?.Invoke(1);
        OnHpDownEvent?.Invoke(0);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        //환자 살리기
        if (_collision.gameObject.CompareTag("Patient"))
        {
            _targetType = Target.Patient;
            _target = _collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _target) 
        {
            _targetType = Target.None;
            _target = null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            _targetType = Target.Object;
            _target = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            _targetType = Target.None;
            _target = null;
        }
    }


    private void OnDestroy()
    {
        InputManager.Instance.runAction -= PlayerRun;
        InputManager.Instance.stopRunAction -= StopRun;
    }
}