using Godot;
using Godot.Collections;

public class Graph : Node2D
{
	[Export] public ulong seed = 5112001;
	[Export] public int antCount = 5;
	[Export] public int nodeCount = 15;
	public Array<Ant> ants;
	public Array<GraphNode> nodes;
	public float[,] distances;
	private PackedScene AntScene = (PackedScene)GD.Load("res://Scenes/Ant.tscn");
	private PackedScene NodeScene = (PackedScene)GD.Load("res://Scenes/GraphNode.tscn");
	public PackedScene EdgeScene = (PackedScene)GD.Load("res://Scenes/Edge.tscn");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		GD.Seed(seed);
		nodes = new Array<GraphNode>();
		ants = new Array<Ant>();
		distances = new float[nodeCount,nodeCount];
		// initializes graph nodes
		for(int i = 0; i < nodeCount; i++)
		{
			GraphNode node = (GraphNode)NodeScene.Instance();
			node.x = (float)GD.RandRange(-475, 475);
			node.y = (float)GD.RandRange(-225, 225);
			node.nodeNumber = i;
			node.edges = new Array<Edge>();
			CallDeferred("add_child", node);
			nodes.Add(node);
		}
		// initializes ants
		for(int i = 0; i < antCount; i++)
		{
			Ant ant = (Ant)AntScene.Instance();
			ant.currentNode = (GraphNode)nodes[i%nodeCount];
			Vector2 location = new Vector2(ant.currentNode.x,ant.currentNode.y);
			ant.Position = location;
			ant.visited = new Array<bool>();
			ant.path = new Array<GraphNode>();
			// Sets all nodes to unvisited
			for(int k = 0; k < nodeCount; k++)
				ant.visited.Add(false);
			CallDeferred("add_child", ant);
			ants.Add(ant);
		}
		//sets edges and distances
		for (int i = 0; i < nodeCount; i++)
		{
			for (int k = 0; k < nodeCount; k++)
			{

				Edge edge = (Edge)EdgeScene.Instance();
				Vector2 start = new Vector2(nodes[i].x,nodes[i].y);
				Vector2 end = new Vector2(nodes[k].x,nodes[k].y);
				edge.AddPoint(start);
				edge.AddPoint(end);
				edge.Width = 2f;
				// edge.cost = (float)GD.RandRange(1,15);
				// GD.Print(edge.cost);
				//GD.Print(edge.GetPointPosition(0),edge.GetPointPosition(1));
				distances[i,k] = hueristic(nodes[i],nodes[k]);
				CallDeferred("add_child", edge);
				nodes[i].edges.Add(edge);
				//GD.Print(edge.GetPointCount());
			}
		}
		//if there is time make sure the nodes don't intersect with like areas or something
	}
	//Euclidean hueristic
	public float hueristic(GraphNode from, GraphNode to)
	{
		float distance = Mathf.Sqrt(Mathf.Pow(from.x-to.x, 2f) + Mathf.Pow(from.y-to.y, 2f));
		//GD.Print(distance);
		return distance;
	}
}
