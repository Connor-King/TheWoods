using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WarbandButton : MonoBehaviour
{
    public Warband m_warband;

    public void SetWarband(Warband band)
    {
        m_warband = band;
        GetComponentInChildren<TextMeshProUGUI>().text = m_warband.m_name;
    }

    public void Click()
    {
        FindObjectOfType<UnitManager>().PopulateList(m_warband);
        FindObjectOfType<WarbandManager>().m_selectedBand = m_warband;
    }
}
