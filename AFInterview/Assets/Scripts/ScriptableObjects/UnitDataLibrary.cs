using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AFSInterview
{
    [CreateAssetMenu(fileName = "UnitDataLibrary", menuName = "ScriptableObjects/UnitDataLibrary", order = 1)]
    public class UnitDataLibrary : ScriptableObject
    {
        [SerializeField] private List<UnitData> unitDatas;

        public UnitData GetItemUsageData(int id) // i'd prefer Guids however i'm low on time
        {
            return unitDatas.First(e => e.id.Equals(id));
        }
    }
}
