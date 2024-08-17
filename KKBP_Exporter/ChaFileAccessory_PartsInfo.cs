using System;

[Serializable]
internal class ChaFileAccessory_PartsInfo
{
	public int Type = -1;

	public int Id = -1;

	public int HideCategory = -1;

	public bool NoShake;

	public bool PartsOfHead;

	public ChaFileAccessory_PartsInfo()
	{
	}

	public ChaFileAccessory_PartsInfo(ChaFileAccessory.PartsInfo partsInfo)
	{
		Type = partsInfo.type;
		Id = partsInfo.id;
		HideCategory = partsInfo.hideCategory;
		typeof(ChaFileAccessory.PartsInfo).TryGetVariable<ChaFileAccessory.PartsInfo, bool>("noShake", partsInfo, out NoShake);
		PartsOfHead = partsInfo.partsOfHead;
	}
}
