using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
internal class ChaFileClothesData
{
	public List<int> SubPartsID = new List<int>();

	public List<bool> HideBraOpt = new List<bool>();

	public List<bool> HideShortsOpt = new List<bool>();

	public List<ChaFileClothes_PartsInfo> Parts = new List<ChaFileClothes_PartsInfo>();

	public ChaFileClothesData()
	{
	}

	public ChaFileClothesData(ChaFileClothes chaFileClothes)
	{
		ChaFileClothes.PartsInfo[] parts = chaFileClothes.parts;
		foreach (ChaFileClothes.PartsInfo partsInfo in parts)
		{
			if (partsInfo != null)
			{
				Parts.Add(new ChaFileClothes_PartsInfo(partsInfo));
			}
			else
			{
				Parts.Add(new ChaFileClothes_PartsInfo());
			}
		}
		SubPartsID = chaFileClothes.subPartsId.ToList();
		HideBraOpt = chaFileClothes.hideBraOpt.ToList();
		HideShortsOpt = chaFileClothes.hideShortsOpt.ToList();
	}
}
