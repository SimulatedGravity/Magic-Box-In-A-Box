using UnityEngine;

public class Multipower : Power
{
    public Power[] targets;
    public Power[] antiTargets;

    private void Update()
    {
        foreach (Power t in targets)
        {
            t.PowerOn = PowerOn;
        }
        foreach (Power t in antiTargets)
        {
            t.PowerOn = !PowerOn;
        }
    }
}
