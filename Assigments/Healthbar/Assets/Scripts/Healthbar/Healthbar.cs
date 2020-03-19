using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public Health health;

    private float maxhealth = 0;

    // Update is called once per frame
    void Update()
    {
        if(maxhealth == 0)
        {
            maxhealth = health.GetValue();
        }

        if(!health.Dead())
        {
            float healthLeft = 2f / 100.0f * ((float)(100f / float.Parse(maxhealth.ToString())) * health.GetValue());

            this.transform.localScale = new Vector3(healthLeft, .25f);
        }
        else
        {
            this.transform.localScale = new Vector3(0f, .25f);
        }
    }
}
