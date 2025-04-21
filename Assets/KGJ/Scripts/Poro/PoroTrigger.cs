using Unity.Cinemachine;
using UnityEngine;

public class PoroTrigger : MonoBehaviour
{
    public PoroDialog _poroDialog;
    public CinemachineTargetGroup _targetGroup;
    public BoxCollider2D _boxCollider2D;
    public Animator _cinematicUIAniamtor;
    private CameraZoomController _cameraZoomController;

    private void Start()
    {
        _boxCollider2D.enabled = false;
        _cameraZoomController = FindAnyObjectByType<CameraZoomController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _boxCollider2D.enabled = true;
            // ½Ã³×¸¶Æ½ ºä·Î ÀüÈ¯
            // ÁÜÀ» ¶¯±è
            _cameraZoomController.IsEnding = true;
            _cinematicUIAniamtor.Play("CinematicAnimation");
            _targetGroup.AddMember(_poroDialog.transform.parent, 1f, 5f);
            _poroDialog.StartEnding();
            Destroy(gameObject);
        }
    }
}
