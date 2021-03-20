using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string[] skillKeys;
    public GameObject[] smartKeys;
    public GameObject[] Skills;
    bool holding = false;
    void Update()
    {
        if (!holding)
        {
            for (int i = 0; i < skillKeys.Length; i++)
            {
                if (Input.GetKeyDown(skillKeys[i]))
                {
                    SkillAttack skillAttack = Skills[i].GetComponent<SkillAttack>();
                    if (skillAttack.CurrentCool < skillAttack.DecreasedCool)
                    {
                        return;
                    }
                    skillAttack.followMouse = true;
                    smartKeys[i].SetActive(true);
                    holding = true;
                    break;
                }
            }
        }
        if (holding)
        {
            for (int i = 0; i < skillKeys.Length; i++)
            {
                if (Input.GetKeyUp(skillKeys[i]))
                {
                    smartKeys[i].SetActive(false);
                    Skills[i].GetComponent<SkillAttack>().Fire();
                    holding = false;
                    break;
                }
            }
        }
    }
}