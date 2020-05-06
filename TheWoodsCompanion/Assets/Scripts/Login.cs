using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_username;
    [SerializeField] private TMP_InputField m_password;
    [SerializeField] private Button m_loginButton;
    [SerializeField] private TextMeshProUGUI m_errorText;

    private void Awake()
    {
        VerifyInputs();
    }

    public void Submit()
    {
        StartCoroutine(TryLogin());
    }

    private IEnumerator TryLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", m_username.text);
        form.AddField("password", m_password.text);
        WWW www = new WWW("https://thewoodsardatabase.000webhostapp.com/php/login.php", form);
        yield return www;

        if (www.text.Length == 0)
        {
            Debug.Log("No response from server - is it running?");
            m_errorText.text = "Error - No response from server. Please check internet connection.";
            yield break;
        }

        if (www.text[0] == '0')
        {
            Debug.Log("Logged In!");
            m_errorText.text = "Logged In!";
            UserManager.m_username = m_username.text;
            UserManager.m_userID = int.Parse(www.text.Split('\t')[2]);    
        }
        else
        {
            Debug.Log("Login Failed - " + www.text);
            m_errorText.text = "Login Failed";
            yield break;
        }
        StartCoroutine(GetWarbands());
    }

    private IEnumerator GetWarbands()
    {
        Debug.Log("Getting Warbands.");
        m_errorText.text = "Getting Warbands.";

        //get warbands
        WWWForm form = new WWWForm();
        form.AddField("userid", UserManager.m_userID);
        WWW www = new WWW("https://thewoodsardatabase.000webhostapp.com/php/getwarbands.php", form);
        yield return www;

        if (www.text[0] == '0')
        {
            string[] returned = www.text.Split('\t');
            for (int i = 1; i < returned.Length - 2; i += 2)
            {
                Warband band = new Warband();
                band.m_userID = UserManager.m_userID;
                band.m_warbandID = int.Parse(returned[i]);
                band.m_name = returned[i + 1];
                UserManager.m_warbands.Add(band);
            }
        }
        else
        {
            Debug.Log("Warbands Failed - " + www.text);
            m_errorText.text = "Warbands Failed - " + www.text;

        }
        StartCoroutine(GetUnits());
    }

    private IEnumerator GetUnits()
    {
        Debug.Log("Getting Units.");
        m_errorText.text = "Getting Units.";

        //get units
        foreach (Warband band in UserManager.m_warbands)
        {
            WWWForm form = new WWWForm();
            form.AddField("warbandid", band.m_warbandID);
            WWW www = new WWW("https://thewoodsardatabase.000webhostapp.com/php/getunits.php", form);
            yield return www;

            if (www.text[0] == '0')
            {
                string[] returned = www.text.Split('\t');
                for (int i = 1; i < returned.Length - 7; i += 7)
                {
                    Unit unit = new Unit();
                    unit.m_warbandID = band.m_warbandID;
                    unit.m_unitID = int.Parse(returned[i]);
                    unit.m_name = returned[i + 1];
                    unit.m_strength = int.Parse(returned[i + 2]);
                    unit.m_wisdom = int.Parse(returned[i + 3]);
                    unit.m_agility = int.Parse(returned[i + 4]);
                    unit.m_nerve = int.Parse(returned[i + 5]);
                    unit.m_perception = int.Parse(returned[i + 6]);


                    band.m_units.Add(unit);
                }
                Debug.Log(band.m_units.Count);
            }
            else
            {
                Debug.Log("Getting Units Failed - " + www.text);
                m_errorText.text = "Getting Units Failed - " + www.text;
            }
        }        
        SceneManager.LoadScene(1);
    }

    public void VerifyInputs()
    {
        m_loginButton.interactable = (m_username.text.Length >= 8 && m_password.text.Length >= 8);
    }
}
