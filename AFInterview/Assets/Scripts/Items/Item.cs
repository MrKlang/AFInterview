namespace AFSInterview.Items
{
	using System;
	using UnityEngine;

	[Serializable]
	public class Item
	{
		[SerializeField] private string name;
		[SerializeField] private int value;
		[SerializeField] private bool isConsumable;

		public string Name => name;
		public int Value => value;

		public bool IsConsumable => isConsumable;

		public Item(string name, int value, bool isConsumable)
		{
			this.name = name;
			this.value = value;
			this.isConsumable = false;
		}

		public void Use()
		{
			Debug.Log("Using" + Name);
			ItemsManager.OnItemUsed(this);
		}
	}
}