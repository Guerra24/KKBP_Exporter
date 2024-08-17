using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
internal class ChaFileClothes_PartsInfo
{
	public int Id = -1;

	public int EmblemeId = -1;

	public int EmblemeId2 = -1;

	public List<bool> HideOpt = new List<bool> { false, false };

	public int SleevesType = -1;

	public ChaFileClothes_PartsInfo()
	{
	}

	public ChaFileClothes_PartsInfo(ChaFileClothes.PartsInfo partsInfo)
	{
		Id = partsInfo.id;
		EmblemeId = partsInfo.emblemeId;
		typeof(ChaFileClothes.PartsInfo).TryGetVariable<ChaFileClothes.PartsInfo, int>("emblemeId2", partsInfo, out EmblemeId2);
		typeof(ChaFileClothes.PartsInfo).TryGetVariable<ChaFileClothes.PartsInfo, bool[]>("hideOpt", partsInfo, out var variable);
		if (variable != null)
		{
			HideOpt = variable.ToList();
		}
		typeof(ChaFileClothes.PartsInfo).TryGetVariable<ChaFileClothes.PartsInfo, int>("sleevesType", partsInfo, out SleevesType);
	}
}
