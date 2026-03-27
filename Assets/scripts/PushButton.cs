using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PushButton : Power
{
    public Power target;
    [SerializeField] Color onColour;
    [SerializeField] Color offColour;
    List<Power> collisions = new List<Power>();

    private Light2D powerLight => transform.GetComponentInChildren<Light2D>();

    private void Update()
    {
        PowerOn = false;
        foreach (Power power in collisions) {
            if (power.PowerOn) PowerOn = true;
        }

        target.PowerOn = PowerOn;
        powerLight.color = PowerOn ? onColour : offColour;
        sprite.color = PowerOn ? onColour : offColour;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Power collisionPower = collision.collider.GetComponent<Box>();
        if (collisionPower != null)
        {
            collisions.Add(collisionPower);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Power collisionPower = collision.collider.GetComponent<Box>();
        if (collisionPower != null)
        {
            if (collisions.Contains(collisionPower))
            {
                collisions.Remove(collisionPower);
            }
        }
    }
}
