using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;
    public Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
            return mainCamera;
        }
    }
    [SerializeField] bool IsRun = true;
    [SerializeField] bool IsStatic = true;

    void LateUpdate()
    {
        if (!IsRun)
            return;

        if (!IsStatic)
            transform.LookAt(MainCamera.transform);
        else transform.rotation = MainCamera.transform.rotation;

        transform.rotation = Quaternion.Euler(-transform.eulerAngles.x, 0f, 0f);
    }
}
