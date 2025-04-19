using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyFOV : MonoBehaviour
{
    Light2D _light2D;

    void Start()
    {
        _light2D = GetComponent<Light2D>();
    }

    public void TurnOn()
    {
        _light2D.enabled = true;
    }

    public void TurnOff()
    {
        _light2D.enabled = false;
    }

    public void SetColorRed()
    {
        _light2D.color = Color.red;
    }

    public void SetColorYellow()
    {
        _light2D.color = Color.yellow;
    }

    public void SetColorWhite()
    {
        _light2D.color = Color.white;
    }
}
