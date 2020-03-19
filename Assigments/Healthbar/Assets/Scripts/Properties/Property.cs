using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Property : MonoBehaviour
{
    public int propertyValue;
    private int baseValue;

    private void Start()
    {
        baseValue = propertyValue;
    }

    public void Decrease(int amount)
    {
        propertyValue -= amount;
    }

    public void Increase(int amount)
    {
        propertyValue += amount;
    }

    public void RestoreBackToInit()
    {
        propertyValue = baseValue;
    }

    public int GetValue()
    {
        return propertyValue;
    }
}
