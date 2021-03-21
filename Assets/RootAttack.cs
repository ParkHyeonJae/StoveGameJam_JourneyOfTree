using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAttack : MonoBehaviour
{
    public float Ground;
    SkillAttack skillAttack;
    private void Awake()
    {
        skillAttack = GetComponent<SkillAttack>();
    }
    void Update()
    {
        if (skillAttack.followMouse)
        {
            var v3 = Input.mousePosition;
            v3.z = 10.0f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            transform.position = new Vector3(v3.x, Ground, 0);
        }
    }
}