using System;
using UnityEngine;

public class GraphicManagerControl : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
        base.get_transform().set_position(new Vector3(0f, 0f, base.get_transform().get_position().z));
    }
}

