using System;
using UnityEngine;

[Serializable]
public class SerializableVector2Int
{
    public int x;
	public int y;

    public SerializableVector2Int(Vector2Int original)
    {
        x = original.x;
        y = original.y;
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int(x, y);
    }
}

[Serializable]
public class SerializableVector3
{
	public float x;
	public float y;
    public float z;

	public SerializableVector3(Vector3 original)
	{
		x = original.x;
		y = original.y;
		z = original.z;
	}

	public Vector3 ToVector3()
	{
		return new Vector3(x, y, z);
	}
}
