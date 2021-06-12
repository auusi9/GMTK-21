using Common;
using UnityEngine;

public class PhonePlugs : MonoBehaviour
{
    [SerializeField] private PanelElement[] _plugs;
    [SerializeField] private Canvas _canvas;

    private void Start()
    {
        foreach (var plug in _plugs)
        {
            plug.Configure(_canvas, (RectTransform) transform);
        }
    }
}
