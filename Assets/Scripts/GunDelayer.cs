using UnityEngine;

public class GunDelayer 
{
    public GunDelayer(float delay = 1)
    {
        time = 0;
        this.delay = delay;
    }

    public float delay;
    public float time;

    public bool CanShoot()
    {
        if (time <= Time.time)
        {
            //Cooldown
            time = Time.time + delay;
            return true;
        }

        return false;
    }
}
