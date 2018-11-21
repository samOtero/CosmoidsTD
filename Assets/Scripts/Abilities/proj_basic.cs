using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proj_basic : MonoBehaviour {

    public unit parent;
    public unit myTarget;
    public Vector3 targetLocation;
    public float speed;
    public int damage;

    public void init(unit parent, int damage, Vector3 targetLocation, float speed = 2f, unit myTarget = null)
    {
        this.parent = parent;
        this.myTarget = myTarget;
        this.targetLocation = targetLocation;
        this.speed = speed;
        this.damage = damage;
    }

    void Update()
    {
        if (myTarget != null && myTarget.myStatus != unitStatus.dead)
            updateTargetLocation();

        var velocity = GetVelocity();
        transform.Translate(velocity * Time.deltaTime);
        var distance = Vector2.Distance(transform.position, targetLocation);
        if (distance < speed * Time.deltaTime)
            hitTarget();        
    }

    private void updateTargetLocation()
    {
        targetLocation = myTarget.transform.position + new Vector3(0f, 1f, 0f); //hardcode a delta for now
    }

    private Vector2 GetVelocity()
    {
        var heading = targetLocation - transform.position;
        var direction = heading / heading.magnitude;
        return direction * speed;
    }

    protected virtual void hitTarget()
    {
        if (myTarget != null && myTarget.myStatus != unitStatus.dead)
            myTarget.myFunctionality.damage(damage, parent, myTarget);

        //Get rid of projectile, probably want to do some kind of pool instead
        Destroy(gameObject);
    }
}
