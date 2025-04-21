using Unity.Cinemachine;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    CinemachineImpulseSource _hit;
    PlayerController _playerController;
    Poro _poro;

    void Start()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
        _poro = FindAnyObjectByType<Poro>();
        _hit = transform.GetChild(0).GetComponent<CinemachineImpulseSource>();
        _playerController.OnHpDownEvent += Hit;
        _poro.OnPoroHitEvent += Hit;
    }

    public void Hit(int hpId)
    {
        _hit.GenerateImpulse();
    }

    private void OnDestroy()
    {
        _playerController.OnHpDownEvent -= Hit;
        _poro.OnPoroHitEvent -= Hit;
    }
}
