namespace AFSInterview.Items
{
	using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

	public class InventoryController : MonoBehaviour, IMoneyListener
	{
		[SerializeField] private List<Item> items;
		[SerializeField] private int money;

		private List<IObserver> observers = new List<IObserver>();

		public int Money 
		{
			get 
			{
				return money;
			}
            set
            {
				money = value;
				NotifyObservers();
            } 
		}

		public int ItemsCount => items.Count;

		public void SellAllItemsUpToValue(int maxValue)
		{
			int currentMoney = 0;
			for (var i = 0; i < items.Count; i++)
			{
				if (items[i].Value > maxValue)
					continue;
#if (UNITY_EDITOR)
				Debug.Log($"Selling {items[i].Name} for: {items[i].Value} gold");
#endif
				currentMoney += items[i].Value;
				items.RemoveAt(i);
			}

#if (UNITY_EDITOR)
			Debug.Log($"Sold items for: {currentMoney} with wallet balance: {Money} totaling at: {currentMoney + Money}");
#endif

			Money += currentMoney;
		}

		public void AddItem(Item item)
		{
			items.Add(item);
		}

        public void AttachObserver(IObserver observer)
        {
			observers.Add(observer);
		}

        public void DetachObserver(IObserver observer)
        {
			observers.Remove(observer);
		}

        public void NotifyObservers()
        {
			for(int i = 0; i < observers.Count; i++)
            {
				observers[i].UpdateObserver();
            }
		}

		public void RemoveItem(Item item)
        {
			items.Remove(item);
        }

		public void AddItems(List<Item> itemsToAdd)
        {
#if (UNITY_EDITOR)
			foreach (Item item in itemsToAdd)
			{
				Debug.Log($"Adding {item.Name} to inventory");
			}
#endif

			items.AddRange(itemsToAdd);
        }

        public void UseFirstConsumableIfPossible()
        {
			var consumables = items.Where(e => e.IsConsumable).ToList();
			if(consumables.Count > 0)
            {
				consumables[0].Use();
            }
        }
    }
}