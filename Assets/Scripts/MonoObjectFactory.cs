using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ObjectFactory
{
    public class MonoObjectFactory<T> : MonoBehaviour
    {
        #region Instance creation

        public static T CreateInstance(ObjectConstructMaterials materials)
        {
            return InstantiateNInitialize(materials);
        }

        #endregion

        #region Singleton creation

        static readonly Dictionary<string, T> singletonMap = new Dictionary<string, T>();

        public static void RegisterSingleton(string id, ObjectConstructMaterials materials)
        {
            if (!singletonMap.ContainsKey(id))
            {
                singletonMap.Add(id, InstantiateNInitialize(materials));
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
                return singletonMap[id];
            }
            else
            {
                throw new Exception("Class not registered!!");
            }
        }

        #endregion

        #region Private utility methods

        private static T InstantiateNInitialize(ObjectConstructMaterials material)
        {
            GameObject temp = Instantiate(material.prefab, Vector3.zero, Quaternion.identity) as GameObject;
            temp.GetComponent<T>().GetType().GetMethod("Initialize").Invoke(temp.GetComponent<T>(), material.parameters);

            return temp.GetComponent<T>();
        }

        #endregion

    }

    public struct ObjectConstructMaterials
    {
        public GameObject prefab;
        public object[] parameters;
    }
}