using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WarbandManager : MonoBehaviour
{
    [SerializeField] private GameObject m_warbandButton;
    [SerializeField] private GameObject m_warbandButtonList;
    private List<GameObject> m_warbandButtonsPool = new List<GameObject>();
    public Warband m_selectedBand;
    public TMP_InputField m_newName; 

    private void Start()
    {
        //for (int i = 0; i < 5; i++)
        //{
        //    GameObject newButton = Instantiate(m_warbandButton, m_warbandButtonList.transform);
        //    newButton.transform.SetSiblingIndex(m_warbandButtonList.transform.childCount - 2);
        //    newButton.SetActive(false);
        //    m_warbandButtonsPool.Add(newButton);
        //}
    }

    [ContextMenu("populate")]
    public void PopulateList()
    {
        int i = 0;
        foreach (Warband band in UserManager.m_warbands)
        {
            if (m_warbandButtonsPool.Count > i)
            {
                m_warbandButtonsPool[i].GetComponent<WarbandButton>().SetWarband(band);
                m_warbandButtonsPool[i].SetActive(true);
            }
            else
            {
                m_warbandButtonsPool.Add(Instantiate(m_warbandButton, m_warbandButtonList.transform));
                m_warbandButtonsPool[i].GetComponent<WarbandButton>().SetWarband(band);
                m_warbandButtonsPool[i].transform.SetSiblingIndex(m_warbandButtonList.transform.childCount-3);
                m_warbandButtonsPool[i].SetActive(true);
            }
            i++;
        }
        for (; i < m_warbandButtonsPool.Count; i++)
        {
            m_warbandButtonsPool[i].SetActive(false);
        }
    }

    public void AddNewWarband()
    {
        StartCoroutine(CreateWarband());        
    }

    public void UpdateName()
    {
        m_selectedBand.m_name = m_newName.text;
        PopulateList();
        StartCoroutine(EditBandName());
    }

    public void DeleteWarband()
    {
        UserManager.m_warbands.Remove(m_selectedBand);
        PopulateList();
        StartCoroutine(DeleteBand());
    }

    IEnumerator EditBandName()
    {
        if (!UserManager.m_loggedIn)
        {
            Debug.Log("Not logged in!");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("warbandid", m_selectedBand.m_warbandID);
        form.AddField("warbandname", m_newName.text);
        m_newName.text = "";

        WWW www = new WWW("https://thewoodsardatabase.000webhostapp.com/php/updatewarband.php", form);
        yield return www;

        if (www.text[0] == '0')
        {
            Debug.Log("Warband Updated!");
        }
        else
        {
            Debug.Log("Warband Update Failed Error - " + www.text);
            yield break;
        }
    }

    IEnumerator DeleteBand()
    {
        if (!UserManager.m_loggedIn)
        {
            Debug.Log("Not logged in!");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("warbandid", m_selectedBand.m_warbandID);

        WWW www = new WWW("https://thewoodsardatabase.000webhostapp.com/php/removewarband.php", form);
        yield return www;

        if (www.text[0] == '0')
        {
            Debug.Log("Warband Deleted!");
        }
        else
        {
            Debug.Log("Deleted Warband Failed Error - " + www.text);
            yield break;
        }
    }

    IEnumerator CreateWarband()
    {
        if (!UserManager.m_loggedIn)
        {
            Debug.Log("Not logged in!");
            yield break;
        }

        Warband band = new Warband();
        WWWForm form = new WWWForm();
        form.AddField("userid", UserManager.m_userID);
        form.AddField("warbandname", band.m_name);

        WWW www = new WWW("https://thewoodsardatabase.000webhostapp.com/php/newwarband.php", form);
        yield return www;

        if (www.text[0] == '0')
        {
            Debug.Log("New Warband Created!");
            band.m_warbandID = int.Parse(www.text.Split('\t')[1]);
        }
        else
        {
            Debug.Log("Warband Creation Failed Error - " + www.text);
            yield break;
        }

        UserManager.m_warbands.Add(band);
        foreach (GameObject button in m_warbandButtonsPool)
        {
            if (!button.activeSelf)
            {
                button.transform.SetSiblingIndex(m_warbandButtonList.transform.childCount - 3);
                button.GetComponent<WarbandButton>().SetWarband(band);
                button.SetActive(true);
                yield break;
            }
        }
        GameObject newButton = Instantiate(m_warbandButton, m_warbandButtonList.transform);
        newButton.GetComponent<WarbandButton>().SetWarband(band);
        newButton.transform.SetSiblingIndex(m_warbandButtonList.transform.childCount - 3);
        newButton.SetActive(true);
        m_warbandButtonsPool.Add(newButton);
    }
}
