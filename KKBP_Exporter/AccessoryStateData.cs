using System;
using System.Linq;
using Accessory_States;

[Serializable]
internal class AccessoryStateData
{
	public int Coordinate = -1;

	public int SlotNo = -1;

	public int Binding = -1;

	public int[] States;

	public int Shoetype = 2;

	public bool Parented;

	public AccessoryStateData(int slotNo, Slotdata slotData)
	{
		Coordinate = PmxBuilder.nowCoordinate;
		SlotNo = slotNo;
		Binding = slotData.Binding;
		int num = slotData.States.Count();
		int num2 = 2;
		States = new int[num * num2];
		int num3 = 0;
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				States[num3++] = slotData.States[j][i];
			}
		}
		Shoetype = slotData.Shoetype;
		Parented = slotData.Parented;
	}
}
