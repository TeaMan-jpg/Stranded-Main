using System;
using System.Collections.Generic;

namespace Platformers
{
	[Serializable]
	public class InventorySave
	{
		//which slot was selected when the game saved
		public int selectedSlot;

		//list of saved slot entries
		public List<SlotSave> slots = new();
	}

	[Serializable]
	public class SlotSave
	{
		//which slot index this data belongs to
		public int slotIndex;

		//item id
		public string itemId;

		//item count
		public int count;
	}
}