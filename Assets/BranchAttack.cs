using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchAttack : MonoBehaviour
{
    public SkillAttack skillAttack;
    public GameObject Branch;
    public float size;
    Vector3 originalsize;
    private void Awake()
    {
        Branch.transform.localScale = Branch.transform.localScale / size;
        originalsize = Branch.transform.localScale;
    }
    private void Update()
    {
        if (skillAttack.followMouse)
        {
            var v3 = Input.mousePosition;
            v3.z = 10.0f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            Vector3 mouse = v3;
            Vector3 To = new Vector3(mouse.x, mouse.y, 0);
            transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(To.y - transform.position.y, To.x - transform.position.x) * Mathf.Rad2Deg);
            Branch.transform.position = (To + transform.position) / 2;
            Branch.transform.localScale = originalsize * Vector3.Distance(transform.position, To);
        }
    }
}