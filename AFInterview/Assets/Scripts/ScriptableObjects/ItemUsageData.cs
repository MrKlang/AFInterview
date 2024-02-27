using AFSInterview.Items;
using System;
using System.Collections.Generic;

namespace AFSInterview
{
    [Serializable]
    public class ItemUsageData
    {
        public string itemName;
        public bool addMoneyOnUse;
        public bool addItemsOnUse;
        public int moneyToAddOnUse;
        public List<Item> itemsToAddOnUse;
    }
}
