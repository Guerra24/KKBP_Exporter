using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal class ClothesData
{
	public int CoordinateType = -1;

	public string VariableName = "";

	public List<string> RendNormal01 = new List<string>();

	public List<string> RendNormal02 = new List<string>();

	public List<string> RendNormal03 = new List<string>();

	public List<string> RendAlpha01 = new List<string>();

	public List<string> RendAlpha02 = new List<string>();

	public string RendAccessory = "";

	public string RendEmblem01 = "";

	public List<string> ExRendEmblem01 = new List<string>();

	public string RendEmblem02 = "";

	public List<string> ExRendEmblem02 = new List<string>();

	public List<string> ObjOpt01 = new List<string>();

	public List<string> ObjOpt02 = new List<string>();

	public List<string> ObjSleeves01 = new List<string>();

	public List<string> ObjSleeves02 = new List<string>();

	public List<string> ObjSleeves03 = new List<string>();

	public ClothesData(string varName, int coordType, ChaClothesComponent clothesComponent)
	{
		VariableName = varName;
		CoordinateType = coordType;
		if (clothesComponent == null)
		{
			return;
		}
		Renderer[] rendNormal = clothesComponent.rendNormal01;
		foreach (Renderer renderer in rendNormal)
		{
			if (renderer == null)
			{
				RendNormal01.Add("");
			}
			else
			{
				RendNormal01.Add(renderer.name + " " + PmxBuilder.GetAltInstanceID(renderer));
			}
		}
		rendNormal = clothesComponent.rendNormal02;
		foreach (Renderer renderer2 in rendNormal)
		{
			if (renderer2 == null)
			{
				RendNormal02.Add("");
			}
			else
			{
				RendNormal02.Add(renderer2.name + " " + PmxBuilder.GetAltInstanceID(renderer2));
			}
		}
		typeof(ChaClothesComponent).TryGetVariable<ChaClothesComponent, Renderer[]>("rendNormal03", clothesComponent, out var variable);
		if (variable != null)
		{
			rendNormal = variable;
			foreach (Renderer renderer3 in rendNormal)
			{
				if (renderer3 == null)
				{
					RendNormal03.Add("");
				}
				else
				{
					RendNormal03.Add(renderer3.name + " " + PmxBuilder.GetAltInstanceID(renderer3));
				}
			}
		}
		rendNormal = clothesComponent.rendAlpha01;
		foreach (Renderer renderer4 in rendNormal)
		{
			if (renderer4 == null)
			{
				RendAlpha01.Add("");
			}
			else
			{
				RendAlpha01.Add(renderer4.name + " " + PmxBuilder.GetAltInstanceID(renderer4));
			}
		}
		rendNormal = clothesComponent.rendAlpha02;
		foreach (Renderer renderer5 in rendNormal)
		{
			if (renderer5 == null)
			{
				RendAlpha02.Add("");
			}
			else
			{
				RendAlpha02.Add(renderer5.name + " " + PmxBuilder.GetAltInstanceID(renderer5));
			}
		}
		Renderer rendAccessory = clothesComponent.rendAccessory;
		if ((bool)rendAccessory)
		{
			RendAccessory = rendAccessory.name + " " + PmxBuilder.GetAltInstanceID(rendAccessory);
		}
		Renderer rendEmblem = clothesComponent.rendEmblem01;
		if ((bool)rendEmblem)
		{
			RendEmblem01 = rendEmblem.name + " " + PmxBuilder.GetAltInstanceID(rendEmblem);
		}
		typeof(ChaClothesComponent).TryGetVariable<ChaClothesComponent, Renderer[]>("exRendEmblem01", clothesComponent, out var variable2);
		if (variable2 != null)
		{
			rendNormal = variable2;
			foreach (Renderer renderer6 in rendNormal)
			{
				if (renderer6 == null)
				{
					ExRendEmblem01.Add("");
				}
				else
				{
					ExRendEmblem01.Add(renderer6.name + " " + PmxBuilder.GetAltInstanceID(renderer6));
				}
			}
		}
		Renderer rendEmblem2 = clothesComponent.rendEmblem02;
		if ((bool)rendEmblem2)
		{
			RendEmblem02 = rendEmblem2.name + " " + PmxBuilder.GetAltInstanceID(rendEmblem2);
		}
		typeof(ChaClothesComponent).TryGetVariable<ChaClothesComponent, Renderer[]>("exRendEmblem02", clothesComponent, out var variable3);
		if (variable3 != null)
		{
			rendNormal = variable3;
			foreach (Renderer renderer7 in rendNormal)
			{
				if (renderer7 == null)
				{
					ExRendEmblem02.Add("");
				}
				else
				{
					ExRendEmblem02.Add(renderer7.name + " " + PmxBuilder.GetAltInstanceID(renderer7));
				}
			}
		}
		GameObject[] objOpt = clothesComponent.objOpt01;
		foreach (GameObject gameObject in objOpt)
		{
			if (gameObject == null)
			{
				ObjOpt01.Add("");
			}
			else
			{
				ObjOpt01.Add(PmxBuilder.GetGameObjectPath(gameObject));
			}
		}
		objOpt = clothesComponent.objOpt02;
		foreach (GameObject gameObject2 in objOpt)
		{
			if (gameObject2 == null)
			{
				ObjOpt02.Add("");
			}
			else
			{
				ObjOpt02.Add(PmxBuilder.GetGameObjectPath(gameObject2));
			}
		}
		if (typeof(ChaClothesComponent).TryGetVariable<ChaClothesComponent, GameObject[]>("objSleeves01", clothesComponent, out var variable4))
		{
			objOpt = variable4;
			foreach (GameObject gameObject3 in objOpt)
			{
				if (gameObject3 == null)
				{
					ObjSleeves01.Add("");
				}
				else
				{
					ObjSleeves01.Add(PmxBuilder.GetGameObjectPath(gameObject3));
				}
			}
		}
		if (typeof(ChaClothesComponent).TryGetVariable<ChaClothesComponent, GameObject[]>("objSleeves02", clothesComponent, out var variable5))
		{
			objOpt = variable5;
			foreach (GameObject gameObject4 in objOpt)
			{
				if (gameObject4 == null)
				{
					ObjSleeves02.Add("");
				}
				else
				{
					ObjSleeves02.Add(PmxBuilder.GetGameObjectPath(gameObject4));
				}
			}
		}
		if (!typeof(ChaClothesComponent).TryGetVariable<ChaClothesComponent, GameObject[]>("objSleeves03", clothesComponent, out var variable6))
		{
			return;
		}
		objOpt = variable6;
		foreach (GameObject gameObject5 in objOpt)
		{
			if (gameObject5 == null)
			{
				ObjSleeves03.Add("");
			}
			else
			{
				ObjSleeves03.Add(PmxBuilder.GetGameObjectPath(gameObject5));
			}
		}
	}
}
