using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles creating/updating a unit's life bar
/// </summary>
[CreateAssetMenu(menuName = "Functionality/Basic/Life Bar")]
public class unitLifeBarFunc : ScriptableObject {

    //Do when unit starts
    public void Init(unit whichUnit)
    {
        addLifeBar(whichUnit);
    }
    /// <summary>
    /// Add a Life Bar to a unit, possibly we won't need this step if we handle it in prefab
    /// </summary>
    /// <param name="whichUnit"></param>
	public void addLifeBar(unit whichUnit)
    {
        var lifeBar = Instantiate(Resources.Load("Lifebar"), whichUnit.gameObject.transform) as GameObject;
        whichUnit.lifeBarGfx = lifeBar.transform.Find("Life").gameObject;
        var newHeight = 1.7f; //TEMP, need to figure this out better
        lifeBar.transform.localPosition = new Vector3(0, newHeight, 0);
        whichUnit.lifeBarContainer = lifeBar.GetComponent<Canvas>();
        whichUnit.lifeBarContainer.enabled = false; //Don't show life bar by default
    }

    /// <summary>
    /// Update Lifebar, usually called when unit's life changes
    /// </summary>
    /// <param name="whichUnit"></param>
    public void updateLifeBar(unit whichUnit)
    {
        whichUnit.percLife = (whichUnit.life + 0.0f) / whichUnit.myProfile.totalLife;
        whichUnit.lifeBarGfx.transform.localScale = new Vector3(whichUnit.percLife, 1f, 1f);
        changeColor(whichUnit);

        //Hide lifebar gfx if unit is fully healthy
        if (whichUnit.percLife == 1)
            whichUnit.lifeBarContainer.enabled = false;
        else
            whichUnit.lifeBarContainer.enabled = true;
    }

    /// <summary>
    /// Change the color of the life bar, used for showing that a unit is weak
    /// </summary>
    /// <param name="whichUnit"></param>
    private void changeColor(unit whichUnit)
    {
        if (whichUnit.percLife <= 0.5)
            whichUnit.lifeBarGfx.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        else
            whichUnit.lifeBarGfx.GetComponent<Image>().color = new Color32(0, 255, 58, 255);
    }
}
