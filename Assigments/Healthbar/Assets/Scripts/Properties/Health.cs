using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Property
{
    public bool Dead()
    {
        if(propertyValue <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
