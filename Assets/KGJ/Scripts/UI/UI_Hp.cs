using UnityEngine;

public class UI_Hp : MonoBehaviour
{
    Animator _hpAni1;
    Animator _hpAni2;
    Animator _hpAni3;

    PlayerController _playerController;

    void Start()
    {
        _hpAni1 = transform.GetChild(0).GetComponent<Animator>();
        _hpAni2 = transform.GetChild(1).GetComponent<Animator>();
        _hpAni3 = transform.GetChild(2).GetComponent<Animator>();

        _playerController = FindAnyObjectByType<PlayerController>();
        _playerController.OnHpDownEvent += PlayHpDown;
    }

    void PlayHpDown(int hpId)
    {
        if (hpId == 0)
            _hpAni1.Play("hpDownAnimation");
        else if (hpId == 1)
            _hpAni2.Play("hpDownAnimation");
        else if (hpId == 2)
            _hpAni3.Play("hpDownAnimation");
    }
}
