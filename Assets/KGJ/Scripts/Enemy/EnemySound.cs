using UnityEngine;

public class EnemySound : MonoBehaviour
{
    AudioSource _gunAudioSource;
    AudioClip _gunSound;
    [SerializeField] float _gunSoundVolume = 1f;
    Enemy _enemy;

    void Start()
    {
        _gunAudioSource = GetComponent<AudioSource>();
        _gunSound = Resources.Load<AudioClip>("Audios/gun");
        _enemy = GetComponent<Enemy>();
        _enemy.OnEnemyGunEvent += PlayGunSound;
    }

    void PlayGunSound()
    {
        Debug.Log("PlayGunSound");
        _gunAudioSource.PlayOneShot(_gunSound, _gunSoundVolume);
    }

    void OnDestroy()
    {
        _enemy.OnEnemyGunEvent -= PlayGunSound;
    }
}
