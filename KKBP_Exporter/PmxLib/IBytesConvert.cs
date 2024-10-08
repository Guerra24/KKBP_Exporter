namespace PmxLib;

public interface IBytesConvert
{
	int ByteCount { get; }

	byte[] ToBytes();

	void FromBytes(byte[] bytes, int startIndex);
}
