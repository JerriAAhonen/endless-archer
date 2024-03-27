public static class Is
{
	public static bool Null(object o)
	{
		if (ReferenceEquals(o, null))
			return true;
		if (o is UnityEngine.Object uo && uo == null)
			return true;
		return false;
	}
	
	public static bool NotNull(object o) => !Null(o);
}
