using System;
using System.Collections;
using BepInEx;
using KK_Plugins;
using KKAPI.Maker;
using UnityEngine;

[BepInPlugin("com.bepis.bepinex.kkbpexporter", "KKBP_Exporter", "4.30")]
public class PmxExpoter : BaseUnityPlugin
{
	private PmxBuilder builder;

	private KKBPExporterConfig exportConfig = new KKBPExporterConfig();

	private bool openOptions = true;

	private void OnGUI()
	{
		if (MakerAPI.InsideAndLoaded)
		{
			float num = 80f;
			float num2 = 90f;
			float num3 = 50f;
			float num4 = 80f;
			float num5 = 70f;
			float num6 = 170f;
			GUI.Box(new Rect((float)Screen.width / 2f - num2, 0f, num2 * 2f, 60f), "");
			openOptions = GUI.Toggle(new Rect((float)Screen.width / 2f - num3, 35f, num3 * 2f, 30f), openOptions, "Show Options");
			if (openOptions)
			{
				GUI.Box(new Rect((float)Screen.width / 2f - num6, num5, num6 * 2f, 120f), "");
				exportConfig.exportAllVariations = GUI.Toggle(new Rect((float)Screen.width / 2f - num4 * 2f, num5 + 5f, num4 * 2f, 30f), exportConfig.exportAllVariations, "Export Variations");
				exportConfig.exportHitBoxes = GUI.Toggle(new Rect((float)Screen.width / 2f, num5 + 5f, num4 * 2f, 30f), exportConfig.exportHitBoxes, "Export Hit Meshes");
				exportConfig.exportSingleOutfit = GUI.Toggle(new Rect((float)Screen.width / 2f - num4 * 2f, num5 + 35f, num4 * 2f, 30f), exportConfig.exportSingleOutfit, "Export Single Outfit");
				exportConfig.exportWithoutPhysics = GUI.Toggle(new Rect((float)Screen.width / 2f, num5 + 35f, num4 * 2f, 30f), exportConfig.exportWithoutPhysics, "Export Without Physics");
				exportConfig.exportWithPushups = GUI.Toggle(new Rect((float)Screen.width / 2f - num4 * 2f, num5 + 65f, num4 * 2f, 30f), exportConfig.exportWithPushups, "Enable Pushups");
				exportConfig.exportWithEnabledShapekeys = GUI.Toggle(new Rect((float)Screen.width / 2f, num5 + 65f, num4 * 2f, 30f), exportConfig.exportWithEnabledShapekeys, "Enable Shapekeys");
				exportConfig.exportCurrentPose = GUI.Toggle(new Rect((float)Screen.width / 2f - num4, num5 + 95f, num4 * 2f, 30f), exportConfig.exportCurrentPose, "Keep Current Pose");
			}
			if (GUI.Button(new Rect((float)Screen.width / 2f - num, 0f, num * 2f, 30f), "Export Model for KKBP") && builder == null)
			{
				builder = new PmxBuilder
				{
					exportAll = exportConfig.exportAllVariations,
					exportHitBoxes = exportConfig.exportHitBoxes,
					exportWithEnabledShapekeys = exportConfig.exportWithEnabledShapekeys,
					exportCurrentPose = exportConfig.exportCurrentPose
				};
				StartCoroutine(StartBuild());
			}
		}
	}

	private IEnumerator StartBuild()
	{
		DateTime startDateTime = DateTime.Now;
		ChaControl chaControl = MakerAPI.GetCharacterControl();
		Pushup.PushupController pushupController = MakerAPI.GetCharacterControl().gameObject.GetComponent<Pushup.PushupController>();
		PmxBuilder.pmxFile = null;
		builder.CreateBaseSavePath();
		builder.ChangeAnimations();
		builder.CollectIgnoreList();
		builder.CreateCharacterInfoData();
		builder.ExportDataToJson(exportConfig, "KK_KKBPExporterConfig.json");
		yield return null;
		int num = (exportConfig.exportSingleOutfit ? chaControl.fileStatus.coordinateType : 0);
		int maxCoord = ((!exportConfig.exportSingleOutfit) ? chaControl.chaFile.coordinate.Length : (num + 1));
		PmxBuilder.minCoord = num;
		PmxBuilder.maxCoord = maxCoord;
		bool braPushupBackup = false;
		bool topPushupBackup = false;
		for (int i = num; i < maxCoord + 1; i++)
		{
			yield return null;
			if (i < maxCoord)
			{
				chaControl.ChangeCoordinateTypeAndReload((ChaFileDefine.CoordinateType)i);
			}
			chaControl.SetClothesState(7, 1);
			yield return new WaitForSeconds(0.1f);
			chaControl.SetClothesState(7, 0);
			if (!exportConfig.exportWithPushups && pushupController != null)
			{
				braPushupBackup = pushupController.CurrentBraData.EnablePushup;
				topPushupBackup = pushupController.CurrentTopData.EnablePushup;
				pushupController.CurrentBraData.EnablePushup = false;
				pushupController.CurrentTopData.EnablePushup = false;
				pushupController.RecalculateBody();
				yield return new WaitForSeconds(2f);
			}
			if (!exportConfig.exportWithoutPhysics)
			{
				yield return new WaitForSeconds(2f);
				yield return new WaitForEndOfFrame();
			}
			else
			{
				yield return null;
				chaControl.resetDynamicBoneAll = true;
				yield return null;
			}
			PmxBuilder.nowCoordinate = i;
			yield return StartCoroutine(builder.BuildStart_OG());
			if (!exportConfig.exportWithPushups && pushupController != null)
			{
				pushupController.CurrentBraData.EnablePushup = braPushupBackup;
				pushupController.CurrentTopData.EnablePushup = topPushupBackup;
				pushupController.RecalculateBody();
			}
		}
		if (exportConfig.exportCurrentPose)
		{
			chaControl.animBody.speed = 1f;
		}
		builder.ExportAllDataLists();
		builder.OpenFolderInExplorer(builder.baseSavePath);
		builder = null;
		Console.WriteLine("KKBP Exporter finished in: " + (DateTime.Now - startDateTime).TotalSeconds + " seconds");
	}
}
