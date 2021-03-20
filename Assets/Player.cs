using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public Text[] texts;
    public int[] stats;
    public GameObject CoolButton;
    public GameObject StatUI;
    public Slider HP;

    public int fullHP;
    public int currentHP;
    public int HPincrease;
    
    public int CoolTD;
    public int CoolTDV;

    public int Defense;
    public int DefenseI;

    public int Att;
    public int AttI;

    public static Player player;
    private void Awake()
    {
        player = this;
    }
    public void Statup(string k)
    {
        if(k == "HP")
        {
            stats[0]++;
            texts[0].text = stats[0].ToString();
            fullHP += HPincrease;
            currentHP += HPincrease;
            HP.maxValue = fullHP;
            HP.value = currentHP;
        }
        else if(k == "Defense")
        {
            stats[1]++;
            texts[1].text = stats[1].ToString();
            Defense += DefenseI;
        }
        else if (k == "Cool")
        {
            stats[3]++;
            texts[3].text = stats[3].ToString();
            CoolTD += CoolTDV;
            if (CoolTD >= 50)
            {
                CoolButton.SetActive(false);
                return;
            }
        }
        else
        {
            stats[2]++;
            texts[2].text = stats[2].ToString();
            Att += AttI;
        }
    }
    public void StatOnOFF()
    {
        if (StatUI.activeSelf)
        {
            StatUI.SetActive(false);
        }
        else
        {
            StatUI.SetActive(true);
        }
    }
}