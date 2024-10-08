using System;
using System.Collections.Generic;
using System.IO;

namespace PmxLib;

public class PmxSoftBody : IPmxObjectKey, IPmxStreamIO, ICloneable, INXName
{
	public enum ShapeKind
	{
		TriMesh,
		Rope
	}

	[Flags]
	public enum SoftBodyFlags
	{
		GenerateBendingLinks = 1,
		GenerateClusters = 2,
		RandomizeConstraints = 4
	}

	public struct SoftBodyConfig
	{
		public int AeroModel;

		public float VCF;

		public float DP;

		public float DG;

		public float LF;

		public float PR;

		public float VC;

		public float DF;

		public float MT;

		public float CHR;

		public float KHR;

		public float SHR;

		public float AHR;

		public float SRHR_CL;

		public float SKHR_CL;

		public float SSHR_CL;

		public float SR_SPLT_CL;

		public float SK_SPLT_CL;

		public float SS_SPLT_CL;

		public int V_IT;

		public int P_IT;

		public int D_IT;

		public int C_IT;

		public void Clear()
		{
			AeroModel = 0;
			VCF = 1f;
			DP = 0f;
			DG = 0f;
			LF = 0f;
			PR = 0f;
			VC = 0f;
			DF = 0.2f;
			MT = 0f;
			CHR = 1f;
			KHR = 0.1f;
			SHR = 1f;
			AHR = 0.7f;
			SRHR_CL = 0.1f;
			SKHR_CL = 1f;
			SSHR_CL = 0.5f;
			SR_SPLT_CL = 0.5f;
			SK_SPLT_CL = 0.5f;
			SS_SPLT_CL = 0.5f;
			V_IT = 0;
			P_IT = 1;
			D_IT = 0;
			C_IT = 4;
		}
	}

	public struct SoftBodyMaterialConfig
	{
		public float LST;

		public float AST;

		public float VST;

		public void Clear()
		{
			LST = 1f;
			AST = 1f;
			VST = 1f;
		}
	}

	public class BodyAnchor : IPmxObjectKey, ICloneable
	{
		public int Body;

		public int Vertex;

		public int NodeIndex;

		public bool IsNear;

		public PmxBody RefBody { get; set; }

		public PmxVertex RefVertex { get; set; }

		public PmxObject ObjectKey => PmxObject.SoftBodyAnchor;

		public BodyAnchor()
		{
			NodeIndex = -1;
			IsNear = false;
		}

		public BodyAnchor(BodyAnchor ac)
		{
			Body = ac.Body;
			Vertex = ac.Vertex;
			NodeIndex = ac.NodeIndex;
			IsNear = ac.IsNear;
		}

		public object Clone()
		{
			return new BodyAnchor(this);
		}
	}

	public class VertexPin : IPmxObjectKey, ICloneable
	{
		public int Vertex;

		public int NodeIndex;

		public PmxVertex RefVertex { get; set; }

		public PmxObject ObjectKey => PmxObject.SoftBodyPinVertex;

		public VertexPin()
		{
			NodeIndex = -1;
		}

		public VertexPin(VertexPin pin)
		{
			Vertex = pin.Vertex;
			NodeIndex = pin.NodeIndex;
		}

		public object Clone()
		{
			return new VertexPin(this);
		}
	}

	public ShapeKind Shape;

	public int Material;

	public int Group;

	public PmxBodyPassGroup PassGroup;

	public SoftBodyFlags Flags;

	public int BendingLinkDistance;

	public int ClusterCount;

	public float TotalMass;

	public float Margin;

	public SoftBodyConfig Config;

	public SoftBodyMaterialConfig MaterialConfig;

	PmxObject IPmxObjectKey.ObjectKey => PmxObject.SoftBody;

	public string Name { get; set; }

	public string NameE { get; set; }

	public PmxMaterial RefMaterial { get; set; }

	public bool IsGenerateBendingLinks
	{
		get
		{
			return (Flags & SoftBodyFlags.GenerateBendingLinks) > (SoftBodyFlags)0;
		}
		set
		{
			if (value)
			{
				Flags |= SoftBodyFlags.GenerateBendingLinks;
			}
			else
			{
				Flags &= ~SoftBodyFlags.GenerateBendingLinks;
			}
		}
	}

	public bool IsGenerateClusters
	{
		get
		{
			return (Flags & SoftBodyFlags.GenerateClusters) > (SoftBodyFlags)0;
		}
		set
		{
			if (value)
			{
				Flags |= SoftBodyFlags.GenerateClusters;
			}
			else
			{
				Flags &= ~SoftBodyFlags.GenerateClusters;
			}
		}
	}

	public bool IsRandomizeConstraints
	{
		get
		{
			return (Flags & SoftBodyFlags.RandomizeConstraints) > (SoftBodyFlags)0;
		}
		set
		{
			if (value)
			{
				Flags |= SoftBodyFlags.RandomizeConstraints;
			}
			else
			{
				Flags &= ~SoftBodyFlags.RandomizeConstraints;
			}
		}
	}

	public List<BodyAnchor> BodyAnchorList { get; private set; }

	public List<VertexPin> VertexPinList { get; private set; }

	public int[] VertexIndices { get; set; }

	public string NXName
	{
		get
		{
			return Name;
		}
		set
		{
			Name = value;
		}
	}

	public void NormalizeBodyAnchorList()
	{
		if (BodyAnchorList.Count <= 0)
		{
			return;
		}
		List<int> list = new List<int>(BodyAnchorList.Count);
		Dictionary<string, int> dictionary = new Dictionary<string, int>(BodyAnchorList.Count);
		for (int i = 0; i < BodyAnchorList.Count; i++)
		{
			BodyAnchor bodyAnchor = BodyAnchorList[i];
			string key = bodyAnchor.Body + "_" + bodyAnchor.Vertex;
			if (!dictionary.ContainsKey(key))
			{
				dictionary.Add(key, i);
			}
			else
			{
				list.Add(i);
			}
		}
		if (list.Count > 0)
		{
			int[] array = CP.SortIndexForRemove(list.ToArray());
			foreach (int index in array)
			{
				BodyAnchorList.RemoveAt(index);
			}
		}
	}

	public void SetVertexPinFromText(string text)
	{
		VertexPinList.Clear();
		string[] array = text.Split(',');
		if (array == null)
		{
			return;
		}
		VertexPinList.Capacity = array.Length;
		for (int i = 0; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]) && int.TryParse(array[i].Trim(), out var result))
			{
				VertexPinList.Add(new VertexPin
				{
					Vertex = result
				});
			}
		}
	}

	public void SortVertexPinList()
	{
		if (VertexPinList.Count > 0)
		{
			List<int> list = new List<int>(VertexPinList.Count);
			for (int i = 0; i < VertexPinList.Count; i++)
			{
				list.Add(VertexPinList[i].Vertex);
			}
			list.Sort();
			for (int j = 0; j < VertexPinList.Count; j++)
			{
				VertexPin vertexPin = VertexPinList[j];
				vertexPin.Vertex = list[j];
				vertexPin.NodeIndex = -1;
				vertexPin.RefVertex = null;
			}
		}
	}

	public void NormalizeVertexPinList()
	{
		if (VertexPinList.Count <= 0)
		{
			return;
		}
		SortVertexPinList();
		bool[] array = new bool[VertexPinList.Count];
		array[0] = false;
		for (int i = 1; i < VertexPinList.Count; i++)
		{
			if (VertexPinList[i - 1].Vertex == VertexPinList[i].Vertex)
			{
				array[i] = true;
			}
		}
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (BodyAnchor bodyAnchor in BodyAnchorList)
		{
			dictionary.Add(bodyAnchor.Vertex, 0);
		}
		for (int j = 0; j < VertexPinList.Count; j++)
		{
			int vertex = VertexPinList[j].Vertex;
			if (dictionary.ContainsKey(vertex))
			{
				array[j] = true;
			}
		}
		for (int num = array.Length - 1; num > 0; num--)
		{
			if (array[num])
			{
				VertexPinList.RemoveAt(num);
			}
		}
	}

	public PmxSoftBody()
	{
		Name = "";
		NameE = "";
		Shape = ShapeKind.TriMesh;
		Material = -1;
		Group = 0;
		PassGroup = new PmxBodyPassGroup();
		InitializeParameter();
		BodyAnchorList = new List<BodyAnchor>();
		VertexPinList = new List<VertexPin>();
		VertexIndices = new int[0];
	}

	public PmxSoftBody(PmxSoftBody sbody, bool nonStr)
	{
		FromPmxSoftBody(sbody, nonStr);
	}

	public void InitializeParameter()
	{
		ClearGenerate();
		TotalMass = 1f;
		Margin = 0.05f;
		Config.Clear();
		MaterialConfig.Clear();
	}

	public void ClearGenerate()
	{
		IsGenerateBendingLinks = true;
		IsGenerateClusters = false;
		IsRandomizeConstraints = true;
		BendingLinkDistance = 2;
		ClusterCount = 0;
	}

	public void FromPmxSoftBody(PmxSoftBody sbody, bool nonStr)
	{
		if (!nonStr)
		{
			Name = sbody.Name;
			NameE = sbody.NameE;
		}
		Shape = sbody.Shape;
		Material = sbody.Material;
		Group = sbody.Group;
		PassGroup = sbody.PassGroup.Clone();
		IsGenerateBendingLinks = sbody.IsGenerateBendingLinks;
		IsGenerateClusters = sbody.IsGenerateClusters;
		IsRandomizeConstraints = sbody.IsRandomizeConstraints;
		BendingLinkDistance = sbody.BendingLinkDistance;
		ClusterCount = sbody.ClusterCount;
		TotalMass = sbody.TotalMass;
		Margin = sbody.Margin;
		Config = sbody.Config;
		MaterialConfig = sbody.MaterialConfig;
		BodyAnchorList = CP.CloneList(sbody.BodyAnchorList);
		VertexPinList = CP.CloneList(sbody.VertexPinList);
		VertexIndices = CP.CloneArray_ValueType(sbody.VertexIndices);
	}

	public void FromStreamEx(Stream s, PmxElementFormat f)
	{
		Name = PmxStreamHelper.ReadString(s, f);
		NameE = PmxStreamHelper.ReadString(s, f);
		Shape = (ShapeKind)PmxStreamHelper.ReadElement_Int32(s, 1, signed: true);
		Material = PmxStreamHelper.ReadElement_Int32(s, f.MaterialSize, signed: true);
		Group = PmxStreamHelper.ReadElement_Int32(s, 1, signed: true);
		PassGroup.FromFlagBits((ushort)PmxStreamHelper.ReadElement_Int32(s, 2, signed: false));
		Flags = (SoftBodyFlags)PmxStreamHelper.ReadElement_Int32(s, 1, signed: true);
		BendingLinkDistance = PmxStreamHelper.ReadElement_Int32(s, 4, signed: true);
		ClusterCount = PmxStreamHelper.ReadElement_Int32(s, 4, signed: true);
		TotalMass = PmxStreamHelper.ReadElement_Float(s);
		Margin = PmxStreamHelper.ReadElement_Float(s);
		Config.AeroModel = PmxStreamHelper.ReadElement_Int32(s, 4, signed: true);
		Config.VCF = PmxStreamHelper.ReadElement_Float(s);
		Config.DP = PmxStreamHelper.ReadElement_Float(s);
		Config.DG = PmxStreamHelper.ReadElement_Float(s);
		Config.LF = PmxStreamHelper.ReadElement_Float(s);
		Config.PR = PmxStreamHelper.ReadElement_Float(s);
		Config.VC = PmxStreamHelper.ReadElement_Float(s);
		Config.DF = PmxStreamHelper.ReadElement_Float(s);
		Config.MT = PmxStreamHelper.ReadElement_Float(s);
		Config.CHR = PmxStreamHelper.ReadElement_Float(s);
		Config.KHR = PmxStreamHelper.ReadElement_Float(s);
		Config.SHR = PmxStreamHelper.ReadElement_Float(s);
		Config.AHR = PmxStreamHelper.ReadElement_Float(s);
		Config.SRHR_CL = PmxStreamHelper.ReadElement_Float(s);
		Config.SKHR_CL = PmxStreamHelper.ReadElement_Float(s);
		Config.SSHR_CL = PmxStreamHelper.ReadElement_Float(s);
		Config.SR_SPLT_CL = PmxStreamHelper.ReadElement_Float(s);
		Config.SK_SPLT_CL = PmxStreamHelper.ReadElement_Float(s);
		Config.SS_SPLT_CL = PmxStreamHelper.ReadElement_Float(s);
		Config.V_IT = PmxStreamHelper.ReadElement_Int32(s, 4, signed: true);
		Config.P_IT = PmxStreamHelper.ReadElement_Int32(s, 4, signed: true);
		Config.D_IT = PmxStreamHelper.ReadElement_Int32(s, 4, signed: true);
		Config.C_IT = PmxStreamHelper.ReadElement_Int32(s, 4, signed: true);
		MaterialConfig.LST = PmxStreamHelper.ReadElement_Float(s);
		MaterialConfig.AST = PmxStreamHelper.ReadElement_Float(s);
		MaterialConfig.VST = PmxStreamHelper.ReadElement_Float(s);
		int num = PmxStreamHelper.ReadElement_Int32(s, 4, signed: true);
		BodyAnchorList.Clear();
		BodyAnchorList.Capacity = num;
		for (int i = 0; i < num; i++)
		{
			int body = PmxStreamHelper.ReadElement_Int32(s, f.BodySize, signed: true);
			int vertex = PmxStreamHelper.ReadElement_Int32(s, f.VertexSize, signed: true);
			int num2 = PmxStreamHelper.ReadElement_Int32(s, 1, signed: true);
			BodyAnchorList.Add(new BodyAnchor
			{
				Body = body,
				Vertex = vertex,
				IsNear = (num2 != 0)
			});
		}
		int num3 = PmxStreamHelper.ReadElement_Int32(s, 4, signed: true);
		VertexPinList.Clear();
		VertexPinList.Capacity = num3;
		for (int j = 0; j < num3; j++)
		{
			int vertex2 = PmxStreamHelper.ReadElement_Int32(s, f.VertexSize, signed: true);
			VertexPinList.Add(new VertexPin
			{
				Vertex = vertex2
			});
		}
		NormalizeBodyAnchorList();
		NormalizeVertexPinList();
	}

	public void ToStreamEx(Stream s, PmxElementFormat f)
	{
		PmxStreamHelper.WriteString(s, Name, f);
		PmxStreamHelper.WriteString(s, NameE, f);
		PmxStreamHelper.WriteElement_Int32(s, (int)Shape, 1, signed: true);
		PmxStreamHelper.WriteElement_Int32(s, Material, f.MaterialSize, signed: true);
		PmxStreamHelper.WriteElement_Int32(s, Group, 1, signed: true);
		PmxStreamHelper.WriteElement_Int32(s, PassGroup.ToFlagBits(), 2, signed: false);
		PmxStreamHelper.WriteElement_Int32(s, (int)Flags, 1, signed: false);
		PmxStreamHelper.WriteElement_Int32(s, BendingLinkDistance, 4, signed: true);
		PmxStreamHelper.WriteElement_Int32(s, ClusterCount, 4, signed: true);
		PmxStreamHelper.WriteElement_Float(s, TotalMass);
		PmxStreamHelper.WriteElement_Float(s, Margin);
		PmxStreamHelper.WriteElement_Int32(s, Config.AeroModel, 4, signed: true);
		PmxStreamHelper.WriteElement_Float(s, Config.VCF);
		PmxStreamHelper.WriteElement_Float(s, Config.DP);
		PmxStreamHelper.WriteElement_Float(s, Config.DG);
		PmxStreamHelper.WriteElement_Float(s, Config.LF);
		PmxStreamHelper.WriteElement_Float(s, Config.PR);
		PmxStreamHelper.WriteElement_Float(s, Config.VC);
		PmxStreamHelper.WriteElement_Float(s, Config.DF);
		PmxStreamHelper.WriteElement_Float(s, Config.MT);
		PmxStreamHelper.WriteElement_Float(s, Config.CHR);
		PmxStreamHelper.WriteElement_Float(s, Config.KHR);
		PmxStreamHelper.WriteElement_Float(s, Config.SHR);
		PmxStreamHelper.WriteElement_Float(s, Config.AHR);
		PmxStreamHelper.WriteElement_Float(s, Config.SRHR_CL);
		PmxStreamHelper.WriteElement_Float(s, Config.SKHR_CL);
		PmxStreamHelper.WriteElement_Float(s, Config.SSHR_CL);
		PmxStreamHelper.WriteElement_Float(s, Config.SR_SPLT_CL);
		PmxStreamHelper.WriteElement_Float(s, Config.SK_SPLT_CL);
		PmxStreamHelper.WriteElement_Float(s, Config.SS_SPLT_CL);
		PmxStreamHelper.WriteElement_Int32(s, Config.V_IT, 4, signed: true);
		PmxStreamHelper.WriteElement_Int32(s, Config.P_IT, 4, signed: true);
		PmxStreamHelper.WriteElement_Int32(s, Config.D_IT, 4, signed: true);
		PmxStreamHelper.WriteElement_Int32(s, Config.C_IT, 4, signed: true);
		PmxStreamHelper.WriteElement_Float(s, MaterialConfig.LST);
		PmxStreamHelper.WriteElement_Float(s, MaterialConfig.AST);
		PmxStreamHelper.WriteElement_Float(s, MaterialConfig.VST);
		PmxStreamHelper.WriteElement_Int32(s, BodyAnchorList.Count, 4, signed: true);
		for (int i = 0; i < BodyAnchorList.Count; i++)
		{
			PmxStreamHelper.WriteElement_Int32(s, BodyAnchorList[i].Body, f.BodySize, signed: true);
			PmxStreamHelper.WriteElement_Int32(s, BodyAnchorList[i].Vertex, f.VertexSize, signed: false);
			PmxStreamHelper.WriteElement_Int32(s, BodyAnchorList[i].IsNear ? 1 : 0, 1, signed: true);
		}
		PmxStreamHelper.WriteElement_Int32(s, VertexPinList.Count, 4, signed: true);
		for (int j = 0; j < VertexPinList.Count; j++)
		{
			PmxStreamHelper.WriteElement_Int32(s, VertexPinList[j].Vertex, f.VertexSize, signed: false);
		}
	}

	object ICloneable.Clone()
	{
		return new PmxSoftBody(this, nonStr: false);
	}

	public PmxSoftBody Clone()
	{
		return new PmxSoftBody(this, nonStr: false);
	}
}
