using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : AISwordMan
{
    public List<StageHazard> stageHazards;

    protected override void Update()
    {
        base.Update();
    }

    protected override void AttackInput()
    {
        base.AttackInput(); // instead attack when in range (large range) and a cooldown is up
    }

    protected override void Attack()
    {
        base.Attack(); // instead shoot fireball
    }

    protected override void BlockInput()
    {
        base.BlockInput(); // maybe this is an okay time to block
    }

    protected override void Block()
    {
        base.Block(); // maybe normal block is also okay.
    }
}
