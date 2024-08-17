using System;
using System.Collections.Generic;

[Serializable]
internal class ChaFileAccessoryData
{
	public List<ChaFileAccessory_PartsInfo> Parts = new List<ChaFileAccessory_PartsInfo>();

	public ChaFileAccessoryData()
	{
	}

	public ChaFileAccessoryData(ChaFileAccessory chaFileAccessory)
	{
		ChaFileAccessory.PartsInfo[] parts = chaFileAccessory.parts;
		foreach (ChaFileAccessory.PartsInfo partsInfo in parts)
		{
			if (partsInfo != null)
			{
				Parts.Add(new ChaFileAccessory_PartsInfo(partsInfo));
			}
			else
			{
				Parts.Add(new ChaFileAccessory_PartsInfo());
			}
		}
	}
}
