using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillAttack : MonoBehaviour
{
    public float Damage;
    public float Rate;
    public float OriginalCool;
    public float DecreasedCool;
    public float CurrentCool;
    public Image image;

    public string trigger;
    Animator animator;
    public bool followMouse = true;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Fire()
    {
        CurrentCool = 0;
        followMouse = false;
        animator.SetTrigger(trigger);
    }
    private void Update()
    {
        DecreasedCool = OriginalCool * (100 - Player.player.CoolTD) / 100f;
        image.fillAmount = CurrentCool / DecreasedCool;
        CurrentCool += Time.deltaTime;
    }
}