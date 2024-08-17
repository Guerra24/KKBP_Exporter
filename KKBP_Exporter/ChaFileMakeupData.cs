using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
internal class ChaFileMakeupData
{
	public int EyeshadowId;

	public Color EyeshadowColor = Color.white;

	public int CheekId;

	public Color CheekColor = Color.white;

	public int LipId;

	public Color LipColor = Color.white;

	public List<int> PaintId = new List<int>(2);

	public List<Color> PaintColor = new List<Color>(2);

	public List<Vector4> PaintLayout = new List<Vector4>(2);

	public ChaFileMakeupData()
	{
	}

	public ChaFileMakeupData(ChaFileMakeup chaFileMakeup)
	{
		if (chaFileMakeup != null)
		{
			EyeshadowId = chaFileMakeup.eyeshadowId;
			EyeshadowColor = chaFileMakeup.eyeshadowColor;
			CheekId = chaFileMakeup.cheekId;
			CheekColor = chaFileMakeup.cheekColor;
			LipId = chaFileMakeup.lipId;
			LipColor = chaFileMakeup.lipColor;
			PaintId = chaFileMakeup.paintId.ToList();
			PaintColor = chaFileMakeup.paintColor.ToList();
			PaintLayout = chaFileMakeup.paintLayout.ToList();
		}
	}
}
