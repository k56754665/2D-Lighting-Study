using UnityEngine;

public class GunAmmo_Script : MonoBehaviour
{
    public enum GunAmmoColor
    {
        Blue,
        Red
    }

    [SerializeField] GunAmmoColor currentGunAmmoColor;
    PlayerFire _playerFire;
    Canvas_Script _canvas;
    ParticleSystem _redParticle;
    ParticleSystem _blueParticle;

    public int ammoPlusNum;
    int _ammoMaxNum = 20;

    private void Start()
    {
        _playerFire = GameObject.FindFirstObjectByType<PlayerFire>();
        _canvas = GameObject.FindFirstObjectByType<Canvas_Script>();
        _redParticle = Resources.Load<ParticleSystem>("Particles/Damage_Particle");
        _blueParticle = Resources.Load<ParticleSystem>("Particles/Stun_Particle");
    }


    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("Player"))
        {
            if (currentGunAmmoColor == GunAmmoColor.Blue)
            {
                if(_playerFire.blueGunNumber >= _ammoMaxNum) return; // 최대 총알 수 제한
                else if (_playerFire.blueGunNumber + ammoPlusNum > _ammoMaxNum) ammoPlusNum = _ammoMaxNum - _playerFire.blueGunNumber; // 최대 총알 수 제한
                _playerFire.blueGunNumber += ammoPlusNum;
                Instantiate(_blueParticle, transform.position, transform.rotation);
                _canvas.UpdateGunNumber(_canvas.blueGunUINum, _playerFire.blueGunNumber);
                Destroy(this.gameObject);
            }
            else if (currentGunAmmoColor == GunAmmoColor.Red)
            {
                if (_playerFire.redGunNumber >= _ammoMaxNum) return; // 최대 총알 수 제한
                else if (_playerFire.redGunNumber + ammoPlusNum > _ammoMaxNum) ammoPlusNum = _ammoMaxNum - _playerFire.redGunNumber; // 최대 총알 수 제한
                _playerFire.redGunNumber += ammoPlusNum;
                Instantiate(_redParticle, transform.position, transform.rotation);
                _canvas.UpdateGunNumber(_canvas.redGunUINum, _playerFire.redGunNumber);
                Destroy(this.gameObject);
            }
        }
    }
}
