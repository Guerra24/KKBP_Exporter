using System;
using System.Collections.Generic;
using System.Linq;
using PmxLib;
using UnityEngine;

[Serializable]
internal class MaterialData
{
	public string MaterialName;

	public string ShaderName;

	public List<string> ShaderPropNames;

	public List<string> ShaderPropTextures;

	public List<UnityEngine.Vector4> ShaderPropTextureValues;

	public List<Color> ShaderPropColorValues;

	public List<float> ShaderPropFloatValues;

	public MaterialData(Material material, string _materialName)
	{
		MaterialName = _materialName;
		ShaderName = material.shader.name;
		ShaderPropNames = new List<string>();
		ShaderPropTextures = new List<string>();
		ShaderPropTextureValues = new List<UnityEngine.Vector4>();
		ShaderPropColorValues = new List<Color>();
		ShaderPropFloatValues = new List<float>();
		MaterialShader materialShader2 = MaterialShaders.materialShaders.Find((MaterialShader materialShader) => string.CompareOrdinal(materialShader.shaderName, ShaderName) == 0);
		if (materialShader2 == null)
		{
			return;
		}
		foreach (MaterialShaderItem property in materialShader2.properties)
		{
			string text = "_" + property.name;
			if (property.type == "Texture")
			{
				UnityEngine.Vector2 textureOffset = material.GetTextureOffset(text);
				UnityEngine.Vector2 textureScale = material.GetTextureScale(text);
				UnityEngine.Vector4 item = new UnityEngine.Vector4(textureOffset.x, textureOffset.y, textureScale.x, textureScale.y);
				Dictionary<string, string> dictionary = PmxBuilder.typeMap.ToDictionary((KeyValuePair<string, string> x) => x.Value, (KeyValuePair<string, string> x) => x.Key);
				if (dictionary.ContainsKey(text))
				{
					string text2 = dictionary[text];
					ShaderPropNames.Add(text + " " + property.type + " " + ShaderPropTextures.Count());
					ShaderPropTextures.Add(MaterialName + text2 + ((text2 == "_MT") ? "_CT" : (TextureWriter.normalMapProperties.Contains(text, StringComparer.Ordinal) ? "_CNV" : "")));
				}
				ShaderPropNames.Add(text + "_ST " + property.type + "_ST " + ShaderPropTextureValues.Count());
				ShaderPropTextureValues.Add(item);
			}
			if (property.type == "Color")
			{
				ShaderPropNames.Add(text + " " + property.type + " " + ShaderPropColorValues.Count());
				ShaderPropColorValues.Add(material.GetColor(text));
			}
			if (property.type == "Float")
			{
				ShaderPropNames.Add(text + " " + property.type + " " + ShaderPropFloatValues.Count());
				ShaderPropFloatValues.Add(material.GetFloat(text));
			}
		}
	}
}
