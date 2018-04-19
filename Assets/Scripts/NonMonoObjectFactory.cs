using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectFactory
{
    public class NonMonoObjectFactory<T>
    {
        #region Instance construction

        public static T CreateInstance(Func<T> constructor)
        {
            return constructor();
        }

        #endregion

        #region Singleton construction

        private readonly static Dictionary<string, Func<T>> singletonMap = new Dictionary<string, Func<T>>();

        public static void RegisterSingletonInstance(string id, Func<T> constructor)
        {
            if (!singletonMap.ContainsKey(id))
            {
                singletonMap.Add(id, constructor);
            }
            else
            {
                throw new Exception("Class already registered!!");
            }
        }

        public static T CreateOrGetSingleton(string id)
        {
            if (singletonMap.ContainsKey(id))
            {
                return singletonMap[id]();
            }
            else
            {
                throw new Exception("Class not registered!!");
            }
        }

        #endregion
    }
}