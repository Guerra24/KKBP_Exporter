using System;

[Serializable]
internal class ChaFileCoordinateData
{
	public int CoordinateType = -1;

	public bool EnableMakeup;

	public ChaFileClothesData Clothes = new ChaFileClothesData();

	public ChaFileAccessoryData Accessory = new ChaFileAccessoryData();

	public ChaFileMakeupData Makeup = new ChaFileMakeupData();

	public ChaFileCoordinateData(ChaControl chaControl)
	{
		if (PmxBuilder.nowCoordinate < PmxBuilder.maxCoord)
		{
			CoordinateType = PmxBuilder.nowCoordinate;
		}
		Clothes = new ChaFileClothesData(chaControl.nowCoordinate.clothes);
		Accessory = new ChaFileAccessoryData(chaControl.nowCoordinate.accessory);
		Makeup = new ChaFileMakeupData(chaControl.nowCoordinate.enableMakeup ? chaControl.nowCoordinate.makeup : chaControl.fileFace.baseMakeup);
		EnableMakeup = chaControl.nowCoordinate.enableMakeup;
	}
}
