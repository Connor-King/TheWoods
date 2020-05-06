using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrappleCalc : MonoBehaviour
{
    public TMP_InputField m_attackersFinesseInput;
    public TMP_InputField m_attackersRemainingStaminaInput;
    public TMP_InputField m_attackersStaminaUsedInput;
    public TMP_InputField m_defendersFinesseInput;
    public TMP_InputField m_defendersRemainingStaminaInput;
    public GameObject[] m_actions;

    private int m_attackersFinesse = 0;
    private int m_attackersRemainingStamina = 0;
    private int m_staminaUsed = 0;
    private int m_attackerTouchingBase = 0;

    private int m_defendersFinesse = 0;
    private int m_defendersRemainingStamina = 0;
    private int m_defenderTouchingBase = 0;

    //Attacker
    public void SetAttackerTouchingBase(bool value)
    {
        m_attackerTouchingBase = value ? -3 : 0;
    }    

    public void SetAttackersFinesse()
    {
        m_attackersFinesse = int.Parse(m_attackersFinesseInput.text);
    }

    public void SetAttackersRemainingStamina()
    {
        m_attackersRemainingStamina = int.Parse(m_attackersRemainingStaminaInput.text);
    }

    public void SetStaminaUsed()
    {
        m_staminaUsed = int.Parse(m_attackersStaminaUsedInput.text);
    }

    //Defender

    public void SetDefenderTouchingBase(bool value)
    {
        m_defenderTouchingBase = value ? -3 : 0;
    }

    public void SetDefendersFinesse()
    {
        m_defendersFinesse = int.Parse(m_defendersFinesseInput.text);
    }

    public void SetDefendersRemainingStamina()
    {
        m_defendersRemainingStamina = int.Parse(m_defendersRemainingStaminaInput.text);
    }


    //Other
    public void SetStaminaOption(bool value)
    {
        if (!value) m_staminaUsed = 0;
        m_attackersStaminaUsedInput.gameObject.SetActive(!value);
    }

    public void CalculateGrapple()
    {
        m_attackersFinesse = m_attackersFinesseInput.text.Length > 0 ? int.Parse(m_attackersFinesseInput.text) : 0;

        int attacker =
            m_attackersFinesse
            + m_attackersRemainingStamina
            + m_staminaUsed
            + m_attackerTouchingBase;

        int defender = m_defendersFinesse
            + m_defendersRemainingStamina
            + m_defenderTouchingBase;

        int diff = attacker - defender;
        Debug.Log(diff);
        for (int i = 0; i < m_actions.Length; i++)
        {
            m_actions[i].SetActive(i < diff);
        }
    }

    public void SetUnitStats(Unit unit)
    {
        m_attackersFinesseInput.text = unit.m_finesse.ToString();
    }
}
