using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMultiplyingObject : MonoBehaviour
{
    public float damageMultiplier = 1; // only useful on a rigidbody that will in some other way deal damage
    public List<Man> immuneToDamage;
}
