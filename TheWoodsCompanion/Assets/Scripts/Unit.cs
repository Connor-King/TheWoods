using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public int m_unitID = -1;
    public int m_warbandID = -1;
    public string m_name = "NoNameUnit";

    public int m_strength = 0;
    public int m_wisdom = 0;
    public int m_agility = 0;
    public int m_nerve = 0;
    public int m_perception = 0;

    public int m_dexterity { get { return m_agility + m_nerve; } private set { } }
    public int m_accuracy { get { return m_perception + m_nerve; } private set { } }
    public int m_charisma { get { return m_perception + m_wisdom; } private set { } }
    public int m_presence { get { return m_strength + m_wisdom; } private set { } }
    public int m_finesse { get { return m_strength + m_agility; } private set { } }

    public int m_stamina { get { return m_strength + m_agility + m_nerve; } private set { } }
    public int m_decisiveness { get { return m_wisdom + m_perception + m_nerve; } private set { } }
}
