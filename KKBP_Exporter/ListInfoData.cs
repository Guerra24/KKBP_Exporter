using System;
using System.Collections.Generic;

[Serializable]
internal class ListInfoData
{
	public int CoordinateType = -1;

	public string InfoType = "";

	public List<int> EnumID = new List<int>();

	public List<string> Data = new List<string>();

	public ListInfoData(ListInfoBase listInfo, string infoType)
	{
		if (PmxBuilder.nowCoordinate < PmxBuilder.maxCoord)
		{
			CoordinateType = PmxBuilder.nowCoordinate;
		}
		InfoType = infoType;
		if (listInfo == null)
		{
			return;
		}
		foreach (KeyValuePair<int, string> item in listInfo.dictInfo)
		{
			EnumID.Add(item.Key);
			Data.Add(item.Value);
		}
	}
}
