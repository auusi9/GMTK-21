using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class PhoneMachine : MonoBehaviour
{
    [SerializeField] private PanelElement[] _jacks;
    [SerializeField] private Canvas _canvas;

    private void Start()
    {
        foreach (var jack in _jacks)
        {
            jack.Configure(_canvas, (RectTransform) transform);
        }
    }
}
