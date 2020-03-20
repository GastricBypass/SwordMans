using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBodyPart : BodyPart
{
    public bool destroyFixedJoints = false;
    public bool destroyCharacterJoints = false;
    public bool destroyGameObject = false;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (owner.health <= 0)
        {
            if (destroyFixedJoints)
            {
                FixedJoint[] joints = this.gameObject.GetComponents<FixedJoint>();
                foreach (FixedJoint joint in joints)
                {
                    Destroy(joint);
                }
            }

            if (destroyCharacterJoints)
            {
                CharacterJoint[] joints = this.gameObject.GetComponents<CharacterJoint>();
                foreach (CharacterJoint joint in joints)
                {
                    Destroy(joint);
                }
            }

            if (destroyGameObject)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
