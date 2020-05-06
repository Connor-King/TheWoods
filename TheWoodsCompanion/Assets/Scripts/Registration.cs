using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Registration : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_username;
    [SerializeField] private TMP_InputField m_password;
    [SerializeField] private Button m_submit;
    [SerializeField] private TextMeshProUGUI m_errorText;

    public void Submit()
    {
        StartCoroutine(Register());
    }

    private IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", m_username.text);
        form.AddField("password", m_password.text);

        WWW www = new WWW("https://thewoodsardatabase.000webhostapp.com/php/register.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("New User Created!");
            m_errorText.text = "New User Created!";
        }
        else
        {
            Debug.Log("User Creation Failed Error - " + www.text);
            m_errorText.text = $"User Creation Failed Error - {www.text}";
        }
    }

    public void VerifyInputs()
    {
        m_submit.interactable = (m_username.text.Length >= 8 && m_password.text.Length >= 8);
        if (m_submit.interactable)
        {
            m_errorText.text = "";
        }
        else
        {
            m_errorText.text = "Please enter username and password of at least 8 characters.";
        }
    }
}
