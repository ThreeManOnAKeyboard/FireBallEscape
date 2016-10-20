public class Enumerations
{
	public enum DropType : byte
	{
		Empty,
		Fuel,
		Water,
		Poison
	}

	public enum ControlType : byte
	{
		Free,
		Sideways,
		ZigZag
	}

	public enum DestroyerType : byte
	{
		DropsDestroyer,
		BackgroundDestroyer
	}

	public enum Direction : byte
	{
		Right,
		Left
	}
}
