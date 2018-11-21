using UnityEngine;

public class levelManager : MonoBehaviour {

    public static levelManager current;

    public unit selectedHero;
	// Use this for initialization
	void Start () {

        current = this;

        //Init our heroes, WIP
        var heroes = GameObject.FindGameObjectsWithTag("Hero");
        foreach(var heroObj in heroes)
        {
            var hero = heroObj.GetComponent<unit>();

            //Select our first hero
            if (selectedHero == null)
                selectedHero = hero;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
