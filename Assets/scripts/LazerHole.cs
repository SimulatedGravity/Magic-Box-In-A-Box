using UnityEngine;

public class LazerHole : Catcher
{
    [SerializeField] LineRenderer line;
    [SerializeField] LayerMask mask;
    [SerializeField] GameObject light;
    public bool wasLazered;
    public override void OnUpdate()
    {
        wasLazered = lazered;
        if (lazered) PowerOn = true;
        else
        {
            PowerOn = target.GetComponent<LazerHole>().wasLazered;
        }

        if (PowerOn && !lazered)
        {
            light.SetActive(true);

                RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up / 5, transform.up, float.PositiveInfinity, mask);
                Debug.DrawRay(transform.position, transform.up, Color.red);

                if (hit)
                {
                    line.SetPosition(0, transform.position);
                    line.SetPosition(1, hit.point);

                    if (hit.collider.CompareTag("Catcher"))
                    {
                        hit.collider.GetComponent<Catcher>().lazered = true;
                    }
                }
        }
        else
        {
            light.SetActive(false);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position);
        }
        lazered = false;
    }
}
