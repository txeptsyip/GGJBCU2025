using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PathFinder
{
    public bool ThereIsAWay(int NCells, MapGenerator.MapElement[][] map, Vector2 start, Vector2 end)
    {
        //Generate bool map
        bool[][] boolmap = new bool[NCells][];
        for (int i = 0; i < NCells; i++)
        {
            boolmap[i] = new bool[NCells];
            for (int j = 0; j < NCells; j++)
            {
                if (map[i][j] == MapGenerator.MapElement.Ground || map[i][j] == MapGenerator.MapElement.PlayerStart)
                {
                    boolmap[i][j] = true;
                }
                else
                {
                    boolmap[i][j] = false;
                }
            }
        }

        GridPoint startpoint = new GridPoint(start.x, start.y);
        GridPoint endpoint = new GridPoint(end.x, end.y);

        return FindPath(boolmap, startpoint, endpoint).Count > 0;
    }


    List<GridPoint> closedSet = new List<GridPoint>();
    List<GridPoint> openSet = new List<GridPoint>();

    //cost of start to this key node
    Dictionary<GridPoint, int> gScore = new Dictionary<GridPoint, int>();
    //cost of start to goal, passing through key node
    Dictionary<GridPoint, int> fScore = new Dictionary<GridPoint, int>();

    Dictionary<GridPoint, GridPoint> nodeLinks = new Dictionary<GridPoint, GridPoint>();

    public class GridPoint : IEquatable<GridPoint>
    {
        public int X, Y;
        public GridPoint(int x, int y) { X = x; Y = y; }
        public GridPoint(float x, float y) { X = (int)x; Y = (int)y; }
        public GridPoint(Vector3 v3) { X = (int)v3.x; Y = (int)v3.y; }
        public string toString()
        {
            return "(" + X + ", " + Y + ")";
        }

        public bool Equals(GridPoint other)
        {
            return X == other.X && Y == other.Y;
        }
    }

    public List<GridPoint> FindPath(bool[][] graph, GridPoint start, GridPoint goal)
    {

        openSet.Add(start);
        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        int limits = graph.Length * graph[0].Length;
        while (openSet.Count > 0 && limits > 0)
        {
            //SortOpenSet();
            GridPoint current = openSet[0];
            if (current.Equals(goal))
            {
                return Reconstruct(current);
            }

            //Debug.Log(current.toString());

            openSet.Remove(current);
            closedSet.Add(current);

            //Debug.Log("closed: " + closedSet.Count);

            List<GridPoint> neighbors = Neighbors(graph, current);

            //Debug.Log("Neighbors: " + neighbors.Count);

            foreach (var neighbor in neighbors)
            {
                if (closedSet.Contains(neighbor))
                    continue;

                var projectedG = getGScore(current) + 1;

                if (!openSet.Contains(neighbor))
                {
                    //record it
                    nodeLinks[neighbor] = current;
                    gScore[neighbor] = projectedG;
                    fScore[neighbor] = projectedG + Heuristic(neighbor, goal);

                    AddToOpenSet(neighbor);
                    //openSet.Add(neighbor);
                }
                else if (projectedG >= getGScore(neighbor))
                    continue;
            }
            limits--;
            if (limits <= 0)
            {
                Debug.Log("ERROR " + openSet.Count + " " + closedSet.Count);
            }
        }



        return new List<GridPoint>();
    }

    public void SortOpenSet()
    {
        GridPoint p;
        for (int i = 1; i < openSet.Count; i++)
        {
            for (int j = i; j > 0 && getFScore(openSet[j]) < getFScore(openSet[j - 1]); j--)
            {
                p = openSet[j];
                openSet[j] = openSet[j - 1];
                openSet[j - 1] = p;
            }
        }
    }

    public void AddToOpenSet(GridPoint pt)
    {
        openSet.Add(pt);
        bool found = false;
        for (int i = 0; i < openSet.Count && !found; i++)
        {
            if (getFScore(pt) < getFScore(openSet[i]))
            {
                openSet.Insert(i, pt);
                found = true;

                for (int j = openSet.Count - 1; j > i; j--)
                {
                    openSet[j] = openSet[j - 1];
                }
            }
        }
    }


    private int Heuristic(GridPoint start, GridPoint goal)
    {
        var dx = goal.X - start.X;
        var dy = goal.Y - start.Y;
        return Math.Abs(dx) + Math.Abs(dy);
    }

    private int getGScore(GridPoint pt)
    {
        int score = int.MaxValue;
        gScore.TryGetValue(pt, out score);
        return score;
    }


    private int getFScore(GridPoint pt)
    {
        int score = int.MaxValue;
        fScore.TryGetValue(pt, out score);
        return score;
    }

    public List<GridPoint> Neighbors(bool[][] graph, GridPoint center)
    {
        List<GridPoint> neighbors = new List<GridPoint>();
        GridPoint pt;
        bool left, right, up, down;
        left = right = up = down = false;

        pt = new GridPoint(center.X, center.Y - 1);
        if (IsValidNeighbor(graph, pt))
        {
            neighbors.Add(pt);
            up = true;
        }


        //middle row
        pt = new GridPoint(center.X - 1, center.Y);
        if (IsValidNeighbor(graph, pt))
        {
            neighbors.Add(pt);
            left = true;
        }

        pt = new GridPoint(center.X + 1, center.Y);
        if (IsValidNeighbor(graph, pt))
        {
            neighbors.Add(pt);
            right = true;
        }


        //bottom row
        pt = new GridPoint(center.X, center.Y + 1);
        if (IsValidNeighbor(graph, pt))
        {
            neighbors.Add(pt);
            down = true;
        }

        if (up || left)
        {
            pt = new GridPoint(center.X - 1, center.Y - 1);
            if (IsValidNeighbor(graph, pt))
                neighbors.Add(pt);
        }

        if (up || right)
        {
            pt = new GridPoint(center.X + 1, center.Y - 1);
            if (IsValidNeighbor(graph, pt))
                neighbors.Add(pt);
        }

        if (down || right)
        {
            pt = new GridPoint(center.X + 1, center.Y + 1);
            if (IsValidNeighbor(graph, pt))
                neighbors.Add(pt);
        }

        if (down || left)
        {
            pt = new GridPoint(center.X - 1, center.Y + 1);
            if (IsValidNeighbor(graph, pt))
                neighbors.Add(pt);
        }

        return neighbors;
    }

    public bool IsValidNeighbor(bool[][] matrix, GridPoint pt)
    {
        int x = pt.X;
        int y = pt.Y;
        if (x < 0 || x >= matrix.Length)
            return false;

        if (y < 0 || y >= matrix[x].Length)
            return false;

        if (closedSet.Contains(pt))
        {
            return false;
        }

        if (openSet.Contains(pt))
            return false;

        return matrix[x][y];
    }

    private List<GridPoint> Reconstruct(GridPoint current)
    {
        List<GridPoint> path = new List<GridPoint>();
        while (nodeLinks.ContainsKey(current))
        {
            path.Add(current);
            current = nodeLinks[current];
        }

        path.Reverse();
        return path;
    }

    private GridPoint nextBest()
    {
        int best = int.MaxValue;
        GridPoint bestPt = null;
        foreach (var node in openSet)
        {
            var score = getFScore(node);
            if (score < best)
            {
                bestPt = node;
                best = score;
            }
        }


        return bestPt;
    }

}