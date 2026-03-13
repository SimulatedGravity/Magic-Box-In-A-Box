using Unity.VisualScripting;
using UnityEngine;

public class RoomBox : Box
{
    public Transform EndPos;
    public bool usesPower;
    public Power roomPower;

    private void Update()
    {
        if (usesPower)
        {
            PowerOn = roomPower.PowerOn;
        }
    }
}
