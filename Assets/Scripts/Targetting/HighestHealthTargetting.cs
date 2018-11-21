using UnityEngine;

/// <summary>
/// Gets the closest target
/// </summary>
[CreateAssetMenu(menuName = "TargettingStyles/Highest Health")]
public class HighestHealthTargetting : UnitTargetting
{

    public override unit GetTarget(Vector3 startPos, float range)
    {
        unit target = null;

        var highestHealth = 0f;

        foreach (var current in targetList.Items)
        {
            //don't select dead targets
            if (current.myStatus == unitStatus.dead)
                continue;

            var currDistance = Vector2.Distance(current.gameObject.transform.position, startPos);
            if (currDistance <= range)
            {
                if (current.percLife > highestHealth)
                {
                    target = current;
                    highestHealth = current.percLife;
                }
            }
        }

        return target;
    }
}
