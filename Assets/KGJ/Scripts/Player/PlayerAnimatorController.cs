using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    Animator _animator;
    PlayerInteraction _playerInteraction;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerInteraction = GetComponent<PlayerInteraction>();
        _playerInteraction.OnClosetEnterEvent += () => SetAnimatorBool("isMoving", false);
    }

    void Update()
    {
        
    }

    public void SetAnimatorBool(string name, bool value)
    {
        if (_animator != null)
        {
            _animator.SetBool(name, value);
        }
    }

    public void PlayAnimation(string name)
    {
        if (_animator != null)
        {
            _animator.Play(name);
        }
    }
}
