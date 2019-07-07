using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int m_damage = 1;
    public int GetDamage()
    {
        return m_damage;
    }

    public void SetDamage(int damage)
    {
        m_damage = damage;
    }


}
