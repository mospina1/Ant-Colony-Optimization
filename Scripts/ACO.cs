using Godot;
using Godot.Collections;

public class ACO : Node
{
	public Graph graph;
	public Label distanceLabel;
	public Label antCountLabel;
	public Button startButton;
	public float bestPathLength = Mathf.Inf;
	public Array<GraphNode> bestPath;
	[Export] public float distancePower = .5f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		graph = GetNode<Graph>("Graph");
		distanceLabel = GetNode<Label>("DistanceLabel");
		antCountLabel = GetNode<Label>("AntCountLabel");
		startButton = GetNode<Button>("StartButton");	
	}
	public void _on_StartButton_pressed()
	{
		startButton.QueueFree();
		//while all nodes not yet visited by all ants
		for (int i = 0; i < graph.antCount; i++)
		{
			while (graph.ants[i].visited.Contains(false))
			{
				ConstructSolution();
				UpdatePheromones();
			}
			//GD.Print(graph.ants[i].pathLength,", Ant ", i);
			if (graph.ants[i].pathLength < bestPathLength)
			{
				bestPathLength = graph.ants[i].pathLength;
				bestPath = graph.ants[i].path;
			}
			distanceLabel.Text = "Best Path: " + bestPathLength;
		}
		GD.Print("Best Path: " + bestPathLength);
		DisplayBestPath(bestPath);
		antCountLabel.Text = graph.antCount + " Ants";

		Button resetButton = new Button();
		resetButton.Text = "Reset Graph";
		resetButton.Connect("pressed", this, "_on_ResetButton_pressed");
		CallDeferred("add_child", resetButton);
	}
	public void _on_ResetButton_pressed()
	{
		GetTree().ReloadCurrentScene();
	}
	public void DisplayBestPath(Array<GraphNode> path)
	{
		for(int i = 0; i < path.Count-1; i++)
		{
			Edge edge = (Edge)graph.EdgeScene.Instance();
			Vector2 start = new Vector2(path[i].x, path[i].y);
			Vector2 end = new Vector2(path[i+1].x, path[i+1].y);
			edge.AddPoint(start);
			edge.AddPoint(end);
			edge.Width = 2f;
			edge.r = 1f;
			edge.b = .4f;
			edge.g = .4f;
			edge.pheromoneStrength = 1f;
			CallDeferred("add_child", edge);
		}
	}

	public void ConstructSolution()
	{
		int n = graph.nodeCount;
		int m = graph.antCount;
		int step = 0; // depth of solution
		for(int k = 0; k < m; k++)
		{
			graph.ants[k].visited[k%n] = true; //Starting nodes are equal to the ant's index
			graph.ants[k].path.Add(graph.ants[k].currentNode);
		}
		while(step < n-1)
		{
			step += 1;
			for(int k = 0; k < m; k++)
			{
				ACODecisionRule(k,step);
			}
		}
		for(int k = 0; k < m; k++) 
		{
			//Make the a circlular path
			graph.ants[k].path[step].nextNode = graph.ants[k].path[0];
			graph.ants[k].pathLength = ComputePathLength(k);
		}
	}
	public void ACODecisionRule(int antID, int step)
	{
		int n = graph.nodeCount;
		int m = graph.antCount;
		//GD.Print(step);
		int c = graph.nodes[step].nodeNumber;
		float[] probabilitySelect = new float[n];
		float probabilitySum = 0f;
		for (int j = 0 ; j < n; j++)
		{
			if(graph.ants[antID].visited[j])
				probabilitySelect[j] = 0f;
			else
			{
				if (graph.nodes[step].edges[j].pheromoneStrength > 0)
					probabilitySelect[j] = graph.distances[c,j] * (graph.nodes[step].edges[j].pheromoneStrength);
				else
					probabilitySelect[j] = Mathf.Pow(graph.distances[c,j], distancePower);
				//GD.Print(probabilitySelect[j]);
				probabilitySum += probabilitySelect[j];
			}
		}
		GD.Randomize();
		float r = (float)GD.RandRange(0,probabilitySum);
		int k = 0;
		float p = probabilitySelect[k];
		while (p < r)
		{
			//GD.Print(p);
			k++;
			p += probabilitySelect[k];
		}
		graph.nodes[c].nextNode = graph.nodes[k];
		graph.ants[antID].path.Add(graph.nodes[k]);
		graph.ants[antID].visited[k] = true;
	}
	public float ComputePathLength(int antID)
	{
		Ant currAnt = graph.ants[antID];
		//GD.Print(currAnt.path.Count);
		float length = 0;
		for(int i = 1; i < currAnt.path.Count; i++)
		{
			//formula for dist between two points
			length += graph.hueristic(currAnt.path[i],currAnt.path[i-1]);
		}
		//GD.Print(length);
		return length;
	}
	public void UpdatePheromones()
	{
		int m = graph.antCount;
		Evaporate();
		for (int k = 0; k < m; k++)
			DepositPheromones(k);
	}
	public void Evaporate()
	{
		float p = .03f;
		int n = graph.nodeCount;
		for (int i = 0; i < n; i++)
		{
			for (int j = 0; j < n; j++)
			{
				graph.nodes[i].edges[j].pheromoneStrength *= (1f - p);
				graph.nodes[j].edges[i].pheromoneStrength = graph.nodes[i].edges[j].pheromoneStrength; //makes sure pheromones are mirrored
			}
		}
	}
	public void DepositPheromones(int antID)
	{
		int n = graph.nodeCount;
		float tau = 1f/(float)graph.ants[antID].pathLength;
		for (int i = 0; i < n-1; i++)
		{
			int j = graph.ants[antID].path[i].nodeNumber;
			int l = graph.ants[antID].path[i+1].nodeNumber;
			graph.nodes[j].edges[l].pheromoneStrength += tau;
			graph.nodes[l].edges[j].pheromoneStrength = graph.nodes[j].edges[l].pheromoneStrength;  // makes sure pheromones are mirrored

		}
	}
}
