using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : AISwordMan
{
    public Transform spellcastingLocation;
    public List<StageHazard> stageHazards;
    public bool castingSpell;
    public float timeBetweenSpells = 30; // seconds

    public Rigidbody fireballPrefab;
    public Vector3 fireballSpawnOffset = new Vector3(0, 1, 0);
    public float fireballSpeed = 15;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(CastSpell());
    }

    protected override void Update()
    {
        if (castingSpell)
        {
            target = spellcastingLocation.gameObject;
        }

        base.Update();
    }

    protected override void AttackInput()
    {
        base.AttackInput(); // instead attack when in range (large range) and a cooldown is up
    }

    protected override void Attack()
    {
        if (target == null || castingSpell)
        {
            return;
        }

        if (attacking)
        {
            Vector3 fireballSpawnPosition = this.transform.position + fireballSpawnOffset;

            Rigidbody fireball = Instantiate(fireballPrefab, fireballSpawnPosition, Quaternion.identity);
            DamagingArea damage = fireball.GetComponent<DamagingArea>();
            if (damage != null)
            {
                damage.immuneToDamage.Add(this.owner);
            }

            fireball.velocity = (target.transform.position - fireballSpawnPosition).normalized * fireballSpeed;
        }
        
        base.Attack();
    }

    protected override void BlockInput()
    {
        base.BlockInput(); // maybe this is an okay time to block
    }

    protected override void Block()
    {
        base.Block(); // maybe normal block is also okay.
    }

    public IEnumerator CastSpell()
    {
        yield return new WaitForSeconds(timeBetweenSpells);

        castingSpell = true;

        int spellIndex = Random.Range(0, stageHazards.Count);
        StageHazard spell = stageHazards[spellIndex];

        spell.TriggerHazard();

        yield return new WaitForSeconds(spell.duration);

        castingSpell = false;
        target = FindTarget();

        StartCoroutine(CastSpell());
    }
}
