using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public static class UnitObjectManager
    {
        public static UnitObject[] UnitObjects => Resources.LoadAll<UnitObject>("UnitObjects");

        private static Dictionary<string, UnitObject> unitsByName;

        private static bool IsInitialized => unitsByName != null;

        private static void InitializeManager()
        {
            if (IsInitialized) return;

            unitsByName = new Dictionary<string, UnitObject>();

            foreach (var unitObject in UnitObjects)
            {
                unitsByName.Add(unitObject.unitName, unitObject);
            }
        }
        public static UnitObject GetUnit(string unitType)
        {
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
