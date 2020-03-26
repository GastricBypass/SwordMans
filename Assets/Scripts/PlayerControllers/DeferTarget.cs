using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeferTarget : IEntity
{
    [Tooltip("This AIEnemy will not pick it's own target, it will instead defer to this AIEnemy's target")]
    public AIEnemy deferTo;

    private AIEnemy attachedAI;

    // Start is called before the first frame update
    void Start()
    {
        attachedAI = gameObject.GetComponent<AIEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            attachedAI.target = deferTo.target;
        }
    }
}
