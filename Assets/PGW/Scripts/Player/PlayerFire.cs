using UnityEngine;
using Define;
using System.Collections;

public class PlayerFire : MonoBehaviour
{
    Canvas_Script _canvas;
    public GunType currentGunType;
    PlayerAnimatorController _playerAnimatorController;
    PlayerSound _playerSound;

    [SerializeField] GameObject gun;
    [SerializeField] GameObject soundwaveBlueGun;
    [SerializeField] GameObject soundwaveRedGun;
    GameObject _can; // Can Prefab
    GameObject _canObject;
    GameObject _smokeBomb;

    public int blueGunNumber = 20;
    public int redGunNumber = 20;
    public int smokeBombNumber;

    bool _canFire; // 총 발사 가능 여부
    public bool CanFire { get { return _canFire; } set { _canFire = value; } }

    void Start()
    {
        _playerAnimatorController = GetComponent<PlayerAnimatorController>();
        _playerSound = GetComponent<PlayerSound>();

        _canvas = GameObject.FindFirstObjectByType<Canvas_Script>();
        _canvas.UpdateGunNumber(_canvas.blueGunUINum, blueGunNumber);
        _canvas.UpdateGunNumber(_canvas.redGunUINum, redGunNumber);

        _can = Resources.Load<GameObject>("Prefabs/KGJ/Can"); // Can Prefab 로드
        _smokeBomb = Resources.Load<GameObject>("Prefabs/KGJ/SmokeBomb"); // 연막탄 Prefab 로드

        InputManager.Instance.fireAction += PlayerGunFire; // 총 발사
        InputManager.Instance.changeWeaponAction += CheckMouseWheel; // 총 변경

        currentGunType = GunType.BlueGun; // 초기 총 종류 설정
        _canFire = true; // 총 발사 가능 상태로 초기화
    }

    public void CheckMouseWheel(Vector2 vector)
    {
        if (currentGunType == GunType.Can) // 오브젝트를 들고 있는 상태
        {
            return;
        }

        if (vector.y > 0f) // 위로 스크롤
        {
            SwitchGun(1);
            //Debug.Log("Current Gun: " + currentGunType);
        }
        else if (vector.y < 0f) // 아래로 스크롤
        {
            SwitchGun(-1);
            //Debug.Log("Current Gun: " + currentGunType);
        }
    }

    void SwitchGun(int WheelDirection)
    {
        // 총 종류를 변경 direction은 휠 방향을 의미
        int gunCount = System.Enum.GetValues(typeof(GunType)).Length;
        if (currentGunType == GunType.BlueGun)
        {
            currentGunType = GunType.RedGun;
        }
        else if (currentGunType == GunType.RedGun)
        {
            currentGunType = GunType.SmokeBomb;
        }
        else if (currentGunType == GunType.SmokeBomb)
        {
            currentGunType = GunType.BlueGun;
        }

        // 현재 총 HUD 변경
        if (currentGunType == GunType.BlueGun)
        {
            _canvas.HideCanImage();
            _canvas.TurnOff(_canvas.redGunUI);
            _canvas.TurnOff(_canvas.smokeBombUI);
            _canvas.TurnOn(_canvas.blueGunUI);
            _canvas.UpdateGunNumber(_canvas.blueGunUINum, blueGunNumber);
        }
        else if (currentGunType == GunType.RedGun)
        {
            _canvas.HideCanImage();
            _canvas.TurnOff(_canvas.blueGunUI);
            _canvas.TurnOff(_canvas.smokeBombUI);
            _canvas.TurnOn(_canvas.redGunUI);
            _canvas.UpdateGunNumber(_canvas.redGunUINum, redGunNumber);
        }
        else if (currentGunType == GunType.SmokeBomb)
        {
            _canvas.HideCanImage();
            _canvas.TurnOff(_canvas.blueGunUI);
            _canvas.TurnOff(_canvas.redGunUI);
            _canvas.TurnOn(_canvas.smokeBombUI);
            _canvas.UpdateGunNumber(_canvas.smokeBombUINum, smokeBombNumber);
        }
    }

    public void SetCurrentCan()
    {
        currentGunType = GunType.Can; // 오브젝트를 들고 있는 상태
        _canvas.ShowCanImage();
        _canvas.TurnOff(_canvas.redGunUI);
        _canvas.TurnOff(_canvas.blueGunUI);
        _canvas.TurnOff(_canvas.smokeBombUI);
    }

    public void PlayerGunFire()
    {
         // 총 발사 불가능 상태일 때는 아무것도 하지 않음
        if (!_canFire) return;

        Debug.Log("Fire|" + currentGunType + "|x=" + transform.position.x + "|y=" + transform.position.y);
        if (currentGunType == GunType.Can)
        {
            Vector3 mouseWorldPos = InputManager.Instance.PointerMoveInput;
            mouseWorldPos.z = 0f; // 2D에서는 z 고정

            Vector3 direction = (mouseWorldPos - transform.position).normalized;
            Vector3 spawnPos = transform.position + direction * 2f;

            _canObject = Instantiate(_can, spawnPos, Quaternion.identity);

            _canObject.GetComponent<Can>().Throw(direction);
            _canvas.HideCanImage();
            currentGunType = GunType.BlueGun;
            _canvas.TurnOff(_canvas.redGunUI);
            _canvas.TurnOn(_canvas.blueGunUI);
        }
        else if (currentGunType == GunType.SmokeBomb && smokeBombNumber > 0)
        {
            _canObject = Instantiate(_smokeBomb, transform.position + (-transform.up * 2f), transform.rotation);
            smokeBombNumber -= 1;
            _canvas.UpdateGunNumber(_canvas.smokeBombUINum, smokeBombNumber);
        }
        else if (currentGunType == GunType.BlueGun && blueGunNumber > 0)
        {
            _playerSound.PlayGunSound();
            _playerAnimatorController.PlayAnimation("PlayerGun1");
            gun.GetComponent<Gun>().BlueGunFire();
            Instantiate(soundwaveBlueGun, transform.position, transform.rotation);
            blueGunNumber -= 1;
            _canvas.UpdateGunNumber(_canvas.blueGunUINum, blueGunNumber);
            StartCoroutine(FireDelay(0.1f)); // 발사 딜레이
        }
        else if (currentGunType == GunType.RedGun && redGunNumber > 0)
        {
            _playerSound.PlayGunSound();
            _playerAnimatorController.PlayAnimation("PlayerGun2");
            gun.GetComponent<Gun>().RedGunFire();
            Instantiate(soundwaveRedGun, transform.position, transform.rotation);
            redGunNumber -= 1;
            _canvas.UpdateGunNumber(_canvas.redGunUINum, redGunNumber);
            StartCoroutine(FireDelay(0.1f)); // 발사 딜레이
        }
        
    }

    IEnumerator FireDelay(float delayTime)
    {
        _canFire = false; // 총 발사 불가능 상태로 설정
        yield return new WaitForSeconds(delayTime);
        _canFire = true; // 총 발사 가능 상태로 설정
    }

    private void OnDestroy()
    {
        InputManager.Instance.fireAction -= PlayerGunFire; // 총 발사
        InputManager.Instance.changeWeaponAction -= CheckMouseWheel; // 총 변경
    }
}
