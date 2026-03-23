using Unity.Mathematics;
using UnityEngine;

public class Lazer : Power
{
    [SerializeField] LineRenderer line;
    [SerializeField] LayerMask mask;
    [SerializeField] AudioClip onClip;
    [SerializeField] AudioClip offClip;
    [SerializeField] GameObject light;
    bool wasOn = false;
    private void Update()
    {
        if (PowerOn != wasOn)
        {
            wasOn = PowerOn;
            if (PowerOn)
            {
                SoundFxManager.Instance.PlaySoundFxClip(onClip, transform, 1f);
            }
            else
            {
                SoundFxManager.Instance.PlaySoundFxClip(offClip, transform, 1f);
            }
        }

        if (PowerOn)
        {
            light.SetActive(true);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up/5, transform.up, float.PositiveInfinity, mask);
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
    }
}
