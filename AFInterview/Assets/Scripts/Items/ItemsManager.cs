namespace AFSInterview.Items
{
    using System.Collections;
    using TMPro;
	using UnityEngine;

	public class ItemsManager : Singleton<ItemsManager>, IObserver
	{
		[SerializeField] private InventoryController inventoryController;
		[SerializeField] private int itemSellMaxValue;
		[SerializeField] private Transform itemSpawnParent;
		[SerializeField] private GameObject itemPrefab;
		[SerializeField] private BoxCollider itemSpawnArea;
		[SerializeField] private float itemSpawnInterval;
		[SerializeField] private ItemUsageDataLibrary itemData;

		private TextMeshProUGUI moneyText;
		private WaitForSeconds spawningInterval;

		public ItemUsageDataLibrary ItemDataLibrary => itemData;

		private void Awake()
		{
			OnAwake();
		}

		private void Start()
        {
			inventoryController.AttachObserver(this);
			spawningInterval = new WaitForSeconds(itemSpawnInterval);
			moneyText = FindObjectOfType<TextMeshProUGUI>();
			UpdateMoneyText();
			SpawnNewItem();
		}

        private void OnDestroy()
        {
			inventoryController.DetachObserver(this);
        }

        private void Update()
		{
			if (Input.GetMouseButtonDown(0))
				TryPickUpItem();
			
			if (Input.GetKeyDown(KeyCode.Space))
				inventoryController.SellAllItemsUpToValue(itemSellMaxValue);

			if (Input.GetKeyDown(KeyCode.C))
				inventoryController.UseFirstConsumableIfPossible();
		}

		private void SpawnNewItem()
		{
			var spawnAreaBounds = itemSpawnArea.bounds;
			var position = new Vector3(
				Random.Range(spawnAreaBounds.min.x, spawnAreaBounds.max.x),
				0f,
				Random.Range(spawnAreaBounds.min.z, spawnAreaBounds.max.z)
			);
			
			Instantiate(itemPrefab, position, Quaternion.identity, itemSpawnParent);
			StartCoroutine(SpawnIntervalCoroutine());
		}

		private void TryPickUpItem()
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var layerMask = LayerMask.GetMask("Item");
			if (!Physics.Raycast(ray, out var hit, 100f, layerMask) || !hit.collider.TryGetComponent<IItemHolder>(out var itemHolder))
				return;
			
			var item = itemHolder.GetItem(true);
            inventoryController.AddItem(item);
            Debug.Log("Picked up " + item.Name + " with value of " + item.Value + " and now have " + inventoryController.ItemsCount + " items");
		}

		private IEnumerator SpawnIntervalCoroutine()
        {
			yield return spawningInterval;
			SpawnNewItem();
        }

		private void UpdateMoneyText()
        {
			moneyText.text = "Money: " + inventoryController.Money;
		}

        public void UpdateObserver()
        {
			UpdateMoneyText();
        }

		public static void OnItemUsed(Item item)
        {
			var temporarilyCachedInstance = GetInstance();
            if (!temporarilyCachedInstance.ItemDataLibrary.CheckIfItemIsInDatabase(item.Name))
            {
#if (UNITY_EDITOR)
				Debug.LogError($"There is no such thing as: {item.Name} in our usage data library!");
#endif
				return;
            }

			ItemUsageData data = temporarilyCachedInstance.ItemDataLibrary.GetItemUsageData(item.Name);

			if (data.addItemsOnUse)
            {
				temporarilyCachedInstance.inventoryController.AddItems(data.itemsToAddOnUse);
			}

            if (data.addMoneyOnUse)
            {
				temporarilyCachedInstance.inventoryController.Money += data.moneyToAddOnUse != item.Value ? item.Value : data.moneyToAddOnUse; //assume custom items on map/in inventory have value priority over default values in library
			}

			temporarilyCachedInstance.inventoryController.RemoveItem(item);
        }
    }
}