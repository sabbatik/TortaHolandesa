using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misc : MonoBehaviour
{
    public Camera _mainCamera;
    public Color _newColor;

    void Update()
    {
        _mainCamera.backgroundColor = _newColor;
    }
}