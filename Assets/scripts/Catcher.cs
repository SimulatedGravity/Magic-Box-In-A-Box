using UnityEngine;

public class Catcher : Power
{
    public Power target;
    public bool lazered;
    private void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
        if (lazered) PowerOn = true;
        else PowerOn = false;
        target.PowerOn = PowerOn;
        lazered = false;
    }
}
