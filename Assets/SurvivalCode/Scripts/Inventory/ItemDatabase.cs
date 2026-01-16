using System.Collections.Generic;
using UnityEngine;

namespace Platformers
{
	[CreateAssetMenu(menuName = "Scriptable Object/Item Database")]
	public class ItemDatabase : ScriptableObject
	{
		public Item[] items;
		private Dictionary<string, Item> map;

		private void OnEnable()
		{
			map = new Dictionary<string, Item>();
			foreach (var item in items)
			{
				//prevents null reference errors
				if (item == null) continue;

				//every item must have a stable unique ID
				if (string.IsNullOrEmpty(item.Id))
				{
					Debug.LogWarning($"Item '{item.name}' has an empty Id and cannot be loaded from saves.");
					continue;
				}

				//prevent duplicates
				if (map.ContainsKey(item.Id))
				{
					Debug.LogWarning($"Duplicate Item Id '{item.Id}' found. Item '{item.name}' will be ignored.");
					continue;
				}

				map.Add(item.Id, item);
			}
		}

		public bool TryGet(string id, out Item item)
		{
			if (map == null) OnEnable();
			return map.TryGetValue(id, out item);
		}
	}
}
