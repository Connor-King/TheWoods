using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject m_unitButton;
    [SerializeField] private GameObject m_unitButtonList;
    private List<GameObject> m_unitButtonsPool = new List<GameObject>();
    public Warband m_selectedBand;
    public Unit m_selectedUnit;
    public GameObject m_statBlock;
    public TMP_InputField[] m_statInputs;

    [ContextMenu("populate")]
    public void PopulateList(Warband band)
    {
        Debug.Log(band.m_name);

        int i = 0;
        foreach (Unit unit in band.m_units)
        {
            if (m_unitButtonsPool.Count > i)
            {
                m_unitButtonsPool[i].SetActive(true);
                m_unitButtonsPool[i].GetComponent<UnitButton>().SetUnit(unit);
            }
            else
            {
                m_unitButtonsPool.Add(Instantiate(m_unitButton, m_unitButtonList.transform));
                m_unitButtonsPool[i].SetActive(true);
                m_unitButtonsPool[i].GetComponent<UnitButton>().SetUnit(unit);
                m_unitButtonsPool[i].transform.SetSiblingIndex(m_unitButtonList.transform.childCount - 5);
            }
            i++;
        }

        for (; i < m_unitButtonsPool.Count; i++)
        {
            m_unitButtonsPool[i].SetActive(false);
        }
        m_selectedBand = band;
    }

    public void ApplyStats()
    {
        m_selectedUnit.m_name = m_statInputs[0].text;
        m_selectedUnit.m_strength = int.Parse(m_statInputs[1].text);
        m_selectedUnit.m_wisdom = int.Parse(m_statInputs[2].text);
        m_selectedUnit.m_agility = int.Parse(m_statInputs[3].text);
        m_selectedUnit.m_nerve = int.Parse(m_statInputs[4].text);
        m_selectedUnit.m_perception = int.Parse(m_statInputs[5].text);
        foreach (TMP_InputField input in m_statInputs)
        {
            input.text = "";
        }
    }

    public void AddNewUnit()
    {
        StartCoroutine(CreateUnit());
    }

    public void UpdateUnit()
    {
        ApplyStats();
        UpdateStatBlock();
        PopulateList(m_selectedBand);
        StartCoroutine(SaveUnit());
    }

    public void DeleteUnit()
    {
        m_selectedBand.m_units.Remove(m_selectedUnit);
        PopulateList(m_selectedBand);
        StartCoroutine(RemoveUnit());
    }

    IEnumerator RemoveUnit()
    {
        if (!UserManager.m_loggedIn)
        {
            Debug.Log("Not logged in!");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("unitid", m_selectedUnit.m_unitID);

        WWW www = new WWW("https://thewoodsardatabase.000webhostapp.com/php/removeunit.php", form);
        yield return www;

        if (www.text[0] == '0')
        {
            Debug.Log("Unit Removed!");
        }
        else
        {
            Debug.Log("Unit Removal Failed Error - " + www.text);
            yield break;
        }
    }

    IEnumerator CreateUnit()
    {
        if (!UserManager.m_loggedIn)
        {
            Debug.Log("Not logged in!");
            yield break;
        }

        Unit unit = new Unit();
        unit.m_warbandID = m_selectedBand.m_warbandID;
        WWWForm form = new WWWForm();
        form.AddField("warbandid", unit.m_warbandID);
        form.AddField("unitname", unit.m_name);

        WWW www = new WWW("https://thewoodsardatabase.000webhostapp.com/php/newunit.php", form);
        yield return www;

        if (www.text[0] == '0')
        {
            Debug.Log("New Unit Created!");
            unit.m_unitID = int.Parse(www.text.Split('\t')[1]);
        }
        else
        {
            Debug.Log("Unit Creation Failed Error - " + www.text);
            yield break;
        }

        m_selectedBand.m_units.Add(unit);
        foreach (GameObject button in m_unitButtonsPool)
        {
            if (!button.activeSelf)
            {
                button.transform.SetSiblingIndex(m_unitButtonList.transform.childCount - 5);
                button.GetComponent<UnitButton>().SetUnit(unit);
                button.SetActive(true);
                yield break;
            }
        }
        GameObject newButton = Instantiate(m_unitButton, m_unitButtonList.transform);
        newButton.GetComponent<UnitButton>().SetUnit(unit);
        newButton.transform.SetSiblingIndex(m_unitButtonList.transform.childCount - 5);
        newButton.SetActive(true);
        m_unitButtonsPool.Add(newButton);
    }

    IEnumerator SaveUnit()
    {        
        if (!UserManager.m_loggedIn)
        {
            Debug.Log("Not logged in!");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("unitid", m_selectedUnit.m_unitID);
        form.AddField("unitname", m_selectedUnit.m_name);
        form.AddField("strength", m_selectedUnit.m_strength);
        form.AddField("agility", m_selectedUnit.m_agility);
        form.AddField("perception", m_selectedUnit.m_perception);
        form.AddField("wisdom", m_selectedUnit.m_wisdom);
        form.AddField("nerve", m_selectedUnit.m_nerve);


        WWW www = new WWW("https://thewoodsardatabase.000webhostapp.com/php/updateunit.php", form);
        yield return www;

        if (www.text[0] == '0')
        {
            Debug.Log("Unit Updated!");
        }
        else
        {
            Debug.Log("Unit Update Failed Error - " + www.text);
            yield break;
        }
    }

    public void SelectUnit(Unit unit)
    {
        m_selectedUnit = unit;
        UpdateStatBlock();
    }

    public void ShootWithUnit()
    {
        FindObjectOfType<ShootCalc>().SetUnitStats(m_selectedUnit);
    }

    public void GrappleWithUnit()
    {
        FindObjectOfType<GrappleCalc>().SetUnitStats(m_selectedUnit);
    }

    void UpdateStatBlock()
    {
        m_statBlock.transform.GetChild(0).GetComponent<Text>().text = m_selectedUnit.m_name;
        m_statBlock.transform.GetChild(1).GetComponent<Text>().text = $"Strength: {m_selectedUnit.m_strength.ToString()}";
        m_statBlock.transform.GetChild(2).GetComponent<Text>().text = $"Wisdom: {m_selectedUnit.m_wisdom.ToString()}";
        m_statBlock.transform.GetChild(3).GetComponent<Text>().text = $"Agility: {m_selectedUnit.m_agility.ToString()}";
        m_statBlock.transform.GetChild(4).GetComponent<Text>().text = $"Nerve: {m_selectedUnit.m_nerve.ToString()}";
        m_statBlock.transform.GetChild(5).GetComponent<Text>().text = $"Perception: {m_selectedUnit.m_perception.ToString()}";
        m_statBlock.transform.GetChild(6).GetComponent<Text>().text = $"Dexterity: {m_selectedUnit.m_dexterity.ToString()}";
        m_statBlock.transform.GetChild(7).GetComponent<Text>().text = $"Accuracy: {m_selectedUnit.m_accuracy.ToString()}";
        m_statBlock.transform.GetChild(8).GetComponent<Text>().text = $"Charisma: {m_selectedUnit.m_charisma.ToString()}";
        m_statBlock.transform.GetChild(9).GetComponent<Text>().text = $"Presence: {m_selectedUnit.m_presence.ToString()}";
        m_statBlock.transform.GetChild(10).GetComponent<Text>().text = $"Finesse: {m_selectedUnit.m_finesse.ToString()}";
    }
}
