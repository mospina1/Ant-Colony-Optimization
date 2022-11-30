using Godot;
using Godot.Collections;
public class Ant : Node2D
{
	[Export] public GraphNode currentNode;
	public Array<GraphNode> path;
	public Array<bool> visited;
	public float pathLength;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// path = new Array<GraphNode>();
		//visited = new Array<bool>();
		pathLength = Mathf.Inf;
	}

  // Called every frame. 'delta' is the elapsed time since the previous frame.
	 public override void _Process(float delta)
 	{
		CallDeferred("set", "position", new Vector2(currentNode.x,currentNode.y));
	}
}
