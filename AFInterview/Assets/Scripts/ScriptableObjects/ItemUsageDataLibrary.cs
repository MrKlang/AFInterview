using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AFSInterview
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
    public class ItemUsageDataLibrary : ScriptableObject
    {
        [SerializeField] private List<ItemUsageData> itemUsageDataList;

        public ItemUsageData GetItemUsageData(string name) // i'd prefer Guids however i'm low on time
        {
            return itemUsageDataList.First(e => e.itemName.Equals(name));
        }

        public bool CheckIfItemIsInDatabase(string name)
        {
            return itemUsageDataList.Where(e => e.itemName.Equals(name)).ToList().Count > 0;
        }
    }
}