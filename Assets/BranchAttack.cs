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
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 To = new Vector3(mouse.x, mouse.y, 0);
            transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(To.y - transform.position.y, To.x - transform.position.x) * Mathf.Rad2Deg);
            Branch.transform.position = (To + transform.position) / 2;
            Branch.transform.localScale = originalsize * Vector3.Distance(transform.position, To);
        }
    }
}