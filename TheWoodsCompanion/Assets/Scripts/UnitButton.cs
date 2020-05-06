using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitButton : MonoBehaviour
{
    public Unit m_unit;
    public GameObject m_statBlock;

    public void SetUnit(Unit unit)
    {
        m_unit = unit;
        GetComponentInChildren<TextMeshProUGUI>().text = unit.m_name;
    }

    public void SelectUnit()
    {
        FindObjectOfType<UnitManager>().SelectUnit(m_unit);
    }
}
