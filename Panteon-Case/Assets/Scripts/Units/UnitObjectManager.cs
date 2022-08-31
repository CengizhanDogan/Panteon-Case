using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public static class UnitObjectManager
    {
        // Gets all scriptable objects that are created at Resources/UnitObjects file
        public static UnitObject[] UnitObjects => Resources.LoadAll<UnitObject>("UnitObjects");

        private static Dictionary<string, UnitObject> unitsByName;

        private static bool IsInitialized => unitsByName != null;

        private static void InitializeManager()
        {
            if (IsInitialized) return;

            // Creates dictionary for all units 
            unitsByName = new Dictionary<string, UnitObject>();

            foreach (var unitObject in UnitObjects)
            {
                unitsByName.Add(unitObject.unitName, unitObject);
            }
            //
        }
        public static UnitObject GetUnit(string unitType)
        {
            // Gets Unit scriptable object by given string
            InitializeManager();

            if (unitsByName.ContainsKey(unitType))
            {
                UnitObject unitObject = unitsByName[unitType];
                return unitObject;
            }

            return null;
        }
    }
}
