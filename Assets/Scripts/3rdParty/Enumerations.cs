public class Enumerations
{
	public enum DropType : byte
	{
		Fuel,
		Water,
		Poison,
		Empty
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
