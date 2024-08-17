using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal class AccessoryData
{
	public int CoordinateType = -1;

	public List<string> RendNormal = new List<string>();

	public List<string> RendAlpha = new List<string>();

	public List<string> RendHair = new List<string>();

	public AccessoryData(int coordType, ChaAccessoryComponent accessoryComponent)
	{
		CoordinateType = coordType;
		Renderer[] rendNormal = accessoryComponent.rendNormal;
		foreach (Renderer renderer in rendNormal)
		{
			if (renderer == null)
			{
				RendNormal.Add("");
			}
			else
			{
				RendNormal.Add(renderer.name + " " + PmxBuilder.GetAltInstanceID(renderer));
			}
		}
		rendNormal = accessoryComponent.rendAlpha;
		foreach (Renderer renderer2 in rendNormal)
		{
			if (renderer2 == null)
			{
				RendAlpha.Add("");
			}
			else
			{
				RendAlpha.Add(renderer2.name + " " + PmxBuilder.GetAltInstanceID(renderer2));
			}
		}
		rendNormal = accessoryComponent.rendHair;
		foreach (Renderer renderer3 in rendNormal)
		{
			if (renderer3 == null)
			{
				RendHair.Add("");
			}
			else
			{
				RendHair.Add(renderer3.name + " " + PmxBuilder.GetAltInstanceID(renderer3));
			}
		}
	}
}
