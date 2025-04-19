using Unity.Cinemachine;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    CinemachineImpulseSource _hit;
    PlayerController _playerController;

    void Start()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
        _hit = transform.GetChild(0).GetComponent<CinemachineImpulseSource>();
        _playerController.OnHpDownEvent += Hit;
    }

    public void Hit(int hpId)
    {
        _hit.GenerateImpulse();
    }

    private void OnDestroy()
    {
        _playerController.OnHpDownEvent -= Hit;
    }
}
