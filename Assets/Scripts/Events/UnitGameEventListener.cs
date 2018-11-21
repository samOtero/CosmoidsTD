using UnityEngine;
using UnityEngine.Events;

public class UnitGameEventListener : MonoBehaviour, IUnitGameEventListener  {

    public UnitGameEvent Event;
    public UnitEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(unit whichUnit)
    {
        Response.Invoke(whichUnit);
    }
}
