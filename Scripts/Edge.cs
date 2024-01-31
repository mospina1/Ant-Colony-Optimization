using Godot;
using Godot.Collections;
public class Edge : Line2D
{
	public float pheromoneStrength = 0f;
	// public float cost = 0f;
	public Color strength;
	public float r = .4f;
	public float b = .5f;
	public float g = 1f;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		strength = new Color(r,g,b,pheromoneStrength); // displays the strength of a pheromone trail
		DefaultColor = strength;
	}
}
