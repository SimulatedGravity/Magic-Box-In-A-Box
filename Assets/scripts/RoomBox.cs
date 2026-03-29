using Unity.VisualScripting;
using UnityEngine;

public class RoomBox : Box
{
    public Transform EndPos;
    public bool usesPower;
    public Power roomPower;

    [SerializeField] LazerHole topHole;

    private void Start()
    {
        Transform endLazer = EndPos.Find("lazer emmiter_T");
        if (endLazer != null)
        {
            endLazer.GetComponent<LazerHole>().target = topHole;
            topHole.target = endLazer.GetComponent<LazerHole>();
        }
    }

    private void Update()
    {
        if (usesPower)
        {
            PowerOn = roomPower.PowerOn;
        }
    }
}
