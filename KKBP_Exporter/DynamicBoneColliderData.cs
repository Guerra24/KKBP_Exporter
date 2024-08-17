using System;
using UnityEngine;

[Serializable]
internal class DynamicBoneColliderData
{
	public int CoordinateType = -1;

	public string Name = "";

	public Vector3 OffsetRotation;

	public Vector3 Center;

	public float Radius;

	public Vector3 LossyScale;

	public float Height;

	public int Direction;

	public int Bound;

	public DynamicBoneColliderData(DynamicBoneCollider dynamicBoneCollider)
	{
		if (PmxBuilder.nowCoordinate < PmxBuilder.maxCoord)
		{
			CoordinateType = PmxBuilder.nowCoordinate;
		}
		Name = dynamicBoneCollider.name;
		Vector3 eulerAngles = dynamicBoneCollider.transform.rotation.eulerAngles;
		OffsetRotation = new Vector3(0f - eulerAngles.x, eulerAngles.y, 0f - eulerAngles.z);
		Center = dynamicBoneCollider.m_Center;
		Radius = dynamicBoneCollider.m_Radius;
		LossyScale = dynamicBoneCollider.transform.lossyScale;
		Height = dynamicBoneCollider.m_Height;
		Direction = (int)dynamicBoneCollider.m_Direction;
		Bound = (int)dynamicBoneCollider.m_Bound;
	}
}
