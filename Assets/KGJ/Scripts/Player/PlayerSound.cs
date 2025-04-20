using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    AudioSource _audioSource;
    AudioClip _walkSound;
    AudioSource _gunAudioSource;
    AudioClip _gunSound;
    [SerializeField] float _gunVolume;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _gunAudioSource = transform.GetChild(0).GetComponent<AudioSource>();
        _walkSound = Resources.Load<AudioClip>("Audios/walk");
        _gunSound = Resources.Load<AudioClip>("Audios/gun");
        _audioSource.clip = _walkSound;
    }

    public void PlayWalkSound()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public void StopWalkSound()
    {
        _audioSource.Stop();
    }

    public void PlayGunSound()
    {
        _gunAudioSource.PlayOneShot(_gunSound, _gunVolume);
    }
}
