using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractFactory<T> where T : IAbstractFactoryProduct, new()
{
    public static T CreateInstance()
    {
        return new T();
    }
}
