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
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Ground, 0);
        }
    }
}