using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="GameEvent")]
public class UnitGameEvent : ScriptableObject {

    private List<IUnitGameEventListener> listeners = new List<IUnitGameEventListener>();

    public void Raise(unit whichUnit)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(whichUnit);
    }

    public void RegisterListener(IUnitGameEventListener listener)
    {
        if (listeners.Contains(listener) == false)
            listeners.Add(listener);
    }

    public void UnregisterListener(IUnitGameEventListener listener)
    {
        if (listeners.Contains(listener) == true)
            listeners.Remove(listener);
    }
}
