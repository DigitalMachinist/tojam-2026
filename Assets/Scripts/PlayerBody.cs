using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerBody : MonoBehaviour
{
    public event Action<Hazard> HazardContactEntered;
    public event Action<Hazard> HazardContactTicked;
    public event Action<Hazard> HazardContactExited;

    private struct Contact
    {
        public Hazard Hazard;
        public float NextTickTime;
    }

    private readonly List<Contact> contacts = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        var hazard = other.GetComponentInParent<Hazard>();
        if (hazard == null || IndexOf(hazard) >= 0) return;

        contacts.Add(new Contact { Hazard = hazard, NextTickTime = Time.time + hazard.DamageInterval });
        GameLog.Contact($"Hazard entered: {hazard.name}", hazard);
        HazardContactEntered?.Invoke(hazard);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var hazard = other.GetComponentInParent<Hazard>();
        if (hazard == null) return;

        int i = IndexOf(hazard);
        if (i < 0) return;

        contacts.RemoveAt(i);
        GameLog.Contact($"Hazard exited: {hazard.name}", hazard);
        HazardContactExited?.Invoke(hazard);
    }

    private void Update()
    {
        float now = Time.time;
        for (int i = contacts.Count - 1; i >= 0; i--)
        {
            Contact c = contacts[i];
            if (c.Hazard == null)
            {
                contacts.RemoveAt(i);
                continue;
            }
            if (now >= c.NextTickTime)
            {
                GameLog.Contact($"Hazard tick: {c.Hazard.name}", c.Hazard);
                HazardContactTicked?.Invoke(c.Hazard);
                c.NextTickTime = now + c.Hazard.DamageInterval;
                contacts[i] = c;
            }
        }
    }

    private int IndexOf(Hazard hazard)
    {
        for (int i = 0; i < contacts.Count; i++)
        {
            if (contacts[i].Hazard == hazard) return i;
        }
        return -1;
    }
}
