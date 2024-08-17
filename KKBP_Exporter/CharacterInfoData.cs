using System;
using System.Collections.Generic;

[Serializable]
internal class CharacterInfoData
{
	public int Personality;

	public float VoiceRate;

	public float PupilWidth;

	public float PupilHeight;

	public float PupilX;

	public float PupilY;

	public float HlUpY;

	public float HlDownY;

	public List<float> ShapeInfoFace = new List<float>();

	public List<float> ShapeInfoBody = new List<float>();
}
