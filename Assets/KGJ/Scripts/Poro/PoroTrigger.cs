using UnityEngine;

public class PoroTrigger : MonoBehaviour
{
    public PoroDialog _poroDialog;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _poroDialog.StartEnding();
            Destroy(gameObject);
        }
    }
}
