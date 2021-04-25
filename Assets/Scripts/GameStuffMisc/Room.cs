using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Enemy[] m_enemies;
    private Breakable[] m_breakables;

    private void Awake()        // I'm using awake because Start() wasn't getting called
    {
        m_enemies = GetComponentsInChildren<Enemy>(true);
        m_breakables = GetComponentsInChildren<Breakable>(true);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            foreach (Enemy enemy in m_enemies)
                ChangeActivation(enemy, true);

            foreach (Breakable breakable in m_breakables)
                ChangeActivation(breakable, true);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            foreach (Enemy enemy in m_enemies)
                ChangeActivation(enemy, false);

            foreach (Breakable breakable in m_breakables)
                ChangeActivation(breakable, false);
        }
    }

    private void ChangeActivation(Component component, bool activate)
    {
        component.gameObject.SetActive(activate);
    }
}
