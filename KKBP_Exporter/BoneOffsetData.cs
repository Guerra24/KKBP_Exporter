using System;
using PmxLib;
using UnityEngine;

[Serializable]
internal class BoneOffsetData
{
	public int CoordinateType = -1;

	public string BoneName = "";

	public UnityEngine.Vector3 Offset;

	public BoneOffsetData(string boneName, PmxLib.Vector3 offset)
	{
		if (PmxBuilder.nowCoordinate < PmxBuilder.maxCoord)
		{
			CoordinateType = PmxBuilder.nowCoordinate;
		}
		BoneName = boneName;
		Offset = new UnityEngine.Vector3(offset.X, offset.Y, offset.Z);
	}
}
