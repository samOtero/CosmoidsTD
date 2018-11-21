using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitGameEventListener {

   void OnEventRaised(unit whichUnit);
}
