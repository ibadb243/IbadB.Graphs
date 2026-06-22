# IbadB.Graphs

A lightweight, generic graph data structure library for .NET with built-in pathfinding algorithms.

[![NuGet](https://img.shields.io/nuget/v/IbadB.Graphs.svg)](https://www.nuget.org/packages/IbadB.Graphs)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com)

## Features

- Generic graph with customizable node model (`TModel`) and edge weight (`TValue`)
- Directed graph with nodes and weighted edges
- Five built-in pathfinding algorithms:
  - **BFS** — Breadth-First Search (shortest path by hops)
  - **DFS** — Depth-First Search
  - **Dijkstra** — Shortest path by total weight
  - **A\*** — Heuristic-guided shortest path
  - **GBFS** — Greedy Best-First Search

## Installation

```bash
dotnet add package IbadB.Graphs
```

## Quick Start

```csharp
using IbadB.Graphs;
using IbadB.Graphs.Algorithms;

// Create a graph with string model and int edge weight
var graph = new Graph<string, int>();

// Create nodes
var city1 = new GraphNode<string, int>("London", "London");
var city2 = new GraphNode<string, int>("Paris", "Paris");
var city3 = new GraphNode<string, int>("Berlin", "Berlin");

// Add nodes to graph
graph.AddNode(city1);
graph.AddNode(city2);
graph.AddNode(city3);

// Add edges (directed)
graph.AddEdge(city1, city2, 340);  // London -> Paris, distance 340 km
graph.AddEdge(city2, city3, 1050); // Paris  -> Berlin, distance 1050 km
graph.AddEdge(city1, city3, 930);  // London -> Berlin, distance 930 km

// Find shortest path by weight
var path = graph.Dijkstra(city1, city3);

foreach (var node in path.Vertexes)
    Console.WriteLine(node.Name);

// Output:
// London
// Berlin
```

## Algorithms

### BFS — Breadth-First Search

Finds the path with the **fewest hops** (ignores edge weights).
Best for: unweighted graphs, level-by-level exploration.

```csharp
var path = graph.BFS(start, end);
```

### DFS — Depth-First Search

Explores as far as possible along each branch before backtracking.
Best for: checking connectivity, exploring all paths.

```csharp
var path = graph.DFS(start, end);
```

### Dijkstra's Algorithm

Finds the path with the **lowest total weight**.
Best for: weighted graphs where you need the globally optimal path.

```csharp
var path = graph.Dijkstra(start, end);
```

### A* (A-Star)

Finds the shortest path using a **heuristic function** to guide the search.
Faster than Dijkstra when a good heuristic is available.
Best for: spatial graphs (maps, grids) where distance estimation is possible.

```csharp
// Heuristic: estimated cost from node to goal
int Heuristic(GraphNode<string, int> node)
{
    // your estimation logic here
    return estimatedDistance[node.Name];
}

var path = graph.AStar(start, end, Heuristic);
```

> **Note:** With `h = _ => 0`, A* behaves identically to Dijkstra.

### GBFS — Greedy Best-First Search

Always moves toward the node with the **lowest heuristic value**.
Faster than A\* but does **not** guarantee the shortest path.
Best for: finding *a* path quickly when optimality is not required.

```csharp
var path = graph.GBFS(start, end, Heuristic);
```

## Algorithm Comparison

| Algorithm | Considers weights | Requires heuristic | Guarantees shortest path |
|-----------|:-----------------:|:------------------:|:------------------------:|
| BFS       | No                | No                 | Yes (by hops)            |
| DFS       | No                | No                 | No                       |
| Dijkstra  | Yes               | No                 | Yes                      |
| A\*       | Yes               | Yes                | Yes (with admissible h)  |
| GBFS      | No                | Yes                | No                       |

## Working with Results

All algorithms return a `Pathway<TModel, TValue>` containing the ordered list of nodes from start to end:

```csharp
var path = graph.Dijkstra(start, end);

// Check if path was found
if (path.Vertexes.Count == 0)
{
    Console.WriteLine("No path found.");
    return;
}

// Iterate over nodes in path
foreach (var node in path.Vertexes)
    Console.WriteLine($"{node.Name}: {node.Model}");

// Total number of stops
Console.WriteLine($"Stops: {path.Vertexes.Count}");
```

## Graph API

```csharp
var graph = new Graph<TModel, TValue>();

// Nodes
graph.AddNode(node);
graph.GetNode(id);         // returns null if not found

// Edges
graph.AddEdge(from, to, weight);
graph.EditEdge(from, to, newWeight);
graph.RemoveEdge(from, to);
```

## Custom Node Model

`TModel` can be any class — a coordinate, a game state, a city, a task:

```csharp
public class Location
{
    public double Latitude  { get; set; }
    public double Longitude { get; set; }
}

var graph = new Graph<Location, double>();

var a = new GraphNode<Location, double>("A", new Location { Latitude = 51.5, Longitude = -0.1 });
var b = new GraphNode<Location, double>("B", new Location { Latitude = 48.8, Longitude = 2.3 });

graph.AddNode(a);
graph.AddNode(b);
graph.AddEdge(a, b, 340.5);

// Haversine or Euclidean heuristic for A*
double Heuristic(GraphNode<Location, double> node)
    => Math.Abs(node.Model.Latitude  - b.Model.Latitude)
     + Math.Abs(node.Model.Longitude - b.Model.Longitude);

var path = graph.AStar(a, b, Heuristic);
```

## Samples

The repository includes two sample console applications demonstrating real-world usage:

- **[8Puzzle](samples/IbadB.Graphs.Samples.8Puzzle)** — classic sliding puzzle solved with A* using Manhattan distance heuristic

## Requirements

- .NET 10.0 or later

## License

MIT — see [LICENSE](LICENSE)