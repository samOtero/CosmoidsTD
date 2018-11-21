using UnityEngine;

/// <summary>
/// Gets the closest target
/// </summary>
[CreateAssetMenu(menuName = "TargettingStyles/First")]
public class FirstTargetting :UnitTargetting
{

    public override unit GetTarget(Vector3 startPos, float range)
    {
        unit target = null;

        foreach (var current in targetList.Items)
        {
            //don't select dead targets
            if (current.myStatus == unitStatus.dead)
                continue;

            var currDistance = Vector2.Distance(current.gameObject.transform.position, startPos);
            if (currDistance <= range)
            {
                target = current;
                break;
            }
        }

        return target;
    }
}
