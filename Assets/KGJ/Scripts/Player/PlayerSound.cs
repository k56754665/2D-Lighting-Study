using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    AudioSource _audioSource;
    AudioClip _walkSound;
    AudioSource _gunAudioSource;
    AudioClip _gunSound;
    AudioClip _assassinationSound;
    AudioClip _hitSound;
    [SerializeField] float _gunVolume;
    [SerializeField] float _assassinationVolume;
    [SerializeField] float _hitVolume;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _gunAudioSource = transform.GetChild(0).GetComponent<AudioSource>();
        _walkSound = Resources.Load<AudioClip>("Audios/walk");
        _gunSound = Resources.Load<AudioClip>("Audios/gun");
        _assassinationSound = Resources.Load<AudioClip>("Audios/assassinate");
        _hitSound = Resources.Load<AudioClip>("Audios/bang");
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

    public void PlayAssassinationSound()
    {
        _gunAudioSource.PlayOneShot(_assassinationSound, _assassinationVolume);
    }

    public void PlayHitSound()
    {
        _gunAudioSource.PlayOneShot(_hitSound, _hitVolume);
    }
}
