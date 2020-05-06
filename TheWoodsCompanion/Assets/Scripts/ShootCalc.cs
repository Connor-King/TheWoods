using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class ShootCalc : MonoBehaviour
{
    public Text m_hitText;
    public ScrollRect m_scroll;
    public TMP_InputField m_accuracyInput;
    public TMP_InputField m_distanceInput;
    public TMP_Dropdown m_timesAimedDropdown;
    public TMP_Dropdown m_timesFiredDropdown;
    private int m_didntMove = 0;
    private int m_sprinted = 0;
    private int m_timesAimed = 0;
    private int m_timesFired = 0;
    private int m_tgtDidntMove = 0;
    private int m_tgtSprinted = 0;
    private int m_tgtWithinFOV = 0;
    private int m_tgtProneOrDucked = 0;

    public void CheckForHit()
    {
        m_scroll.verticalNormalizedPosition = 1;

        int accuracy = m_accuracyInput.text.Length > 0 ? int.Parse(m_accuracyInput.text) : 0;
        float distance = m_distanceInput.text.Length > 0 ? float.Parse(m_distanceInput.text) : 0;
        accuracy += m_didntMove
            + m_sprinted
            + m_timesAimed
            + m_timesFired
            + m_tgtDidntMove
            + m_tgtSprinted
            + m_tgtWithinFOV
            + m_tgtProneOrDucked;
        Debug.Log("Hit: " + (accuracy >= distance));
        m_hitText.text = accuracy >= distance ? "Hit!" : "Miss";
        Debug.Log
            (
            int.Parse(m_accuracyInput.text)
            + m_didntMove
            + m_sprinted
            + m_timesAimed
            + m_timesFired
            + m_tgtDidntMove
            + m_tgtSprinted
            + m_tgtWithinFOV
            + m_tgtProneOrDucked
            );
        //reset
    }

    public void SetDidntMove(bool value)
    {
        m_didntMove = value ? 1 : 0;
    }

    public void SetSprinted(bool value)
    {
        m_sprinted = value ? -2 : 0;
    }

    public void SetTimesAimed()
    {
        m_timesAimed = m_timesAimedDropdown.value * 2;
    }

    public void SetTimesFired()
    {
        m_timesFired = m_timesFiredDropdown.value * -1;
    }

    public void SetTgtMoved(bool value)
    {
        m_tgtDidntMove = value ? 2 : 0;
    }

    public void SetTgtSprinted(bool value)
    {
        m_tgtSprinted = value ? -2 : 0;
    }

    public void SetTgtWithinFOV(bool value)
    {
        m_tgtWithinFOV = value ? 3 : 0;
    }

    public void SetTgtProneORDucked(bool value)
    {
        m_tgtProneOrDucked = value ? -2 : 0;
    }

    public void SetUnitStats(Unit unit)
    {
        m_accuracyInput.text = unit.m_accuracy.ToString();
    }
}
