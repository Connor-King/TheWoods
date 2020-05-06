using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserManager
{
    public static string m_username;
    public static int m_userID;
    public static List<Warband> m_warbands = new List<Warband>();

    public static bool m_loggedIn { get { return m_username != null; } }

    public static void Logout()
    {
        m_username = null;
    }
}
