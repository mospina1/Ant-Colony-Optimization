using Godot;
using Godot.Collections;
public class Edge : Line2D
{
	// Declare member variables here. Examples:
	public float pheromoneStrength = 0f;
	// public float cost = 0f;
	public Color strength = new Color(102f,128f,255f,0f);
	//public Vector2 start;
	//public Vector2 end;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		//strength = new Color(102f,128f,255f,1f);
		strength = new Color(.4f,.5f,1f,pheromoneStrength); // use this one after evaporation is set up
		DefaultColor = strength;
	}
}
