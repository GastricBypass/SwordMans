using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Protectable : IEntity
{
    public Man protectionTarget;
    
    public Slider healthMeter;
    public Text healthValue;

    // Start is called before the first frame update
    void Start()
    {
        if (protectionTarget == null)
        {
            protectionTarget = this.GetComponent<Man>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            ChangeHealth(protectionTarget.health / protectionTarget.maxHealth);
        }
    }

    private void ChangeHealth(float percent)
    {
        healthMeter.value = percent * protectionTarget.maxHealth;
        healthValue.text = (Mathf.Ceil(healthMeter.value) + " / " + protectionTarget.maxHealth);
    }
}
