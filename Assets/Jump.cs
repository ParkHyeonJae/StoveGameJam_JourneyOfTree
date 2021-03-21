using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Jump : MonoBehaviour
{
    public float Damage;
    public float Rate;
    public float OriginalCool;
    public float DecreasedCool;
    public float CurrentCool;
    public Image image;

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown("r")&& CurrentCool >= DecreasedCool)
        {
            animator.SetTrigger("Jump");
            CurrentCool = 0;
        }
        DecreasedCool = OriginalCool * (100 - Player.player.CoolTD) / 100f;
        image.fillAmount = CurrentCool / DecreasedCool;
        CurrentCool += Time.deltaTime;
    }
}
