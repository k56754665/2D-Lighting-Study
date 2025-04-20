using UnityEngine;

public class EnemySound : MonoBehaviour
{
    AudioSource _gunAudioSource;
    AudioClip _gunSound;
    AudioClip _hitSound;
    [SerializeField] float _gunSoundVolume = 1f;
    [SerializeField] float _hitSoundVolume = 1f;
    Enemy _enemy;

    void Start()
    {
        _gunAudioSource = GetComponent<AudioSource>();
        _gunSound = Resources.Load<AudioClip>("Audios/gun");
        _hitSound = Resources.Load<AudioClip>("Audios/bang");
        _enemy = GetComponent<Enemy>();
        _enemy.OnEnemyGunEvent += PlayGunSound;
        _enemy.OnEnemyHitEvent += PlayHitSound;
    }

    void PlayGunSound()
    {
        _gunAudioSource.PlayOneShot(_gunSound, _gunSoundVolume);
    }

    void PlayHitSound()
    {
        _gunAudioSource.PlayOneShot(_hitSound, _hitSoundVolume);
    }

    void OnDestroy()
    {
        _enemy.OnEnemyGunEvent -= PlayGunSound;
        _enemy.OnEnemyHitEvent -= PlayHitSound;
    }
}
