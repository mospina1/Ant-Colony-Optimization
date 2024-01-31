using Godot;
using Godot.Collections;

public class GraphNode : Node2D
{
	[Export] public float x = 0;
	[Export] public float y = 0;
	public bool visited = false;
	//public Vector2 location;
	public Array<Edge> edges;
	public GraphNode nextNode;
	public int nodeNumber = 0;
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		CallDeferred("set","position", new Vector2(x,y));
	}
}
