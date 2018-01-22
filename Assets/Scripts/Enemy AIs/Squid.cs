using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : EnemyMan
{
    public float deathTime;
    public Vector3 sinkSpeed;

    public GameObject invisibleWall;

    private bool onStage2 = false;

    public void Update()
    {
        if (!onStage2 && health < maxHealth / 2)
        {
            onStage2 = true;
            StartStage2();
        }
        if (dead)
        {
            this.transform.position = this.transform.position - sinkSpeed;
        }
    }

    protected override void ExtraDeathEffects()
    {
        Destroy(invisibleWall.gameObject);
        if (disappearsOnDeath)
        {
            StartCoroutine(DeathAnimation());
        }
    }

    public void StartStage2()
    {
        Oscillator oscillator = this.GetComponent<Oscillator>();
        if (oscillator != null)
        {
            oscillator.Activate();
        }

        RotationalOscillator rotOscillator = this.GetComponent<RotationalOscillator>();
        if (rotOscillator != null)
        {
            rotOscillator.active = true; //.Activate();
            rotOscillator.xRange = rotOscillator.xRange * 2;
            rotOscillator.yRange = rotOscillator.yRange * 2;
            rotOscillator.yRange = rotOscillator.yRange * 2;
        }
    }

    public IEnumerator DeathAnimation()
    {
        Collider[] colliders = this.transform.GetComponentsInChildren<Collider>();
        foreach(var collider in colliders)
        {
            collider.isTrigger = true;
        }

        Oscillator oscillator = this.GetComponent<Oscillator>();
        if (oscillator != null)
        {
            oscillator.active = false;
        }

        RotationalOscillator rotOscillator = this.GetComponent<RotationalOscillator>();
        if (rotOscillator != null)
        {
            rotOscillator.active = false;
        }

        yield return new WaitForSeconds(deathTime);
        Destroy(this.gameObject);
    }
}
