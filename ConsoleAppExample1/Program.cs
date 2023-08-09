
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace MyApplication
//{
//    class Program
//    {
//        public static bool Search(int[] location, List<int[]> visited)
//        {
//            foreach (int[] array in visited)
//            {
//                if (array[0] == location[0] && array[1] == location[1])
//                    return true;
//            }
//            return false;
//        }

//        public static List<int[]> GetNeighbors(int[][] matrix, int x, int y)
//        {
//            int numRows = matrix.Length;
//            int numCols = matrix[0].Length;
//            List<int[]> result = new List<int[]>();
//            for (int i = (x - 1 < 0 ? 0 : x - 1); i < (x + 2 > numRows ? numRows : x + 2); i++)
//            {
//                for (int j = (y - 1 < 0 ? 0 : y - 1); j < (y + 2 > numCols ? numCols : y + 2); j++)
//                {
//                    if (matrix[i][j] == 0 && (i != x || j != y))
//                    {
//                        result.Add(new int[] { i, j });
//                        //Console.WriteLine("( " + i + "," + j + ")");
//                    }
//                }
//            }
//            return result;
//        }

//        public static int[] DFS(int[][] matrix, int[] loc, List<int[]> visited, int[] end, List<int[]> path)
//        {
//            Console.WriteLine($"current: ({loc[0]}, {loc[1]})");
//            List<int[]> neighbours = GetNeighbors(matrix, loc[0], loc[1]);

//            foreach (int[] neigh in neighbours)
//            {
//                int x = neigh[0];
//                int y = neigh[1];

//                if (!Search(neigh, visited) && matrix[x][y] == 0)
//                {
//                    visited.Add(neigh);
//                    path.Add(neigh);
//                    if (neigh[0] == end[0] && neigh[1] == end[1])
//                    {
//                        Console.WriteLine("End goal reached!");
//                        Console.WriteLine(" ---" + neigh[0] + neigh[1]);
//                        return neigh;
//                    }
//                    path.Remove(neigh);
//                    return DFS(matrix, neigh, visited, end, path);
//                }

//            }

//            return loc;
//        }

//        static void Main(string[] args)
//        {
//            // Example usage:
//            int[][] matrix = new int[][] {
//                new int[] { 0, 0, 0, 0, 0},
//                new int[] { 0, 0, 0, 0, 0 },
//                new int[] {0, 0, 0, 0, 0 }   
//            };
//            List<int[]> visited = new List<int[]>();
//            List<int[]> path = new List<int[]>();
//            int[] startLocation = new int[] { 0, 0 };
//            int[] end = new int[] { 2, 2 };

//            DFS(matrix, startLocation, visited, end, path);

//            for (int i = 0; i < path.Count; i++)
//            {
//                Console.WriteLine(path[i][0] + "- " + path[i][1]);
//            }

//        }
//    }
//}




////A Star

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Program
{
    class Program1
    {
        class Node
        {
            public Node parent = null;
            public int row;
            public int column;
            public int cost;
            public bool start = false;
            public bool goal = false;
            public bool solid = false;
            public bool open = false;
            public bool closed = false;

            public Node(int r, int c)
            {
                row = r;
                column = c;
            }

        }

        static Node[][] map = new Node[15][];
        class astar
        {
            List<Node> opened = new List<Node>();
            List<Node> closed = new List<Node>();

            public void initializemap()
            {
                for (int i = 0; i < map.Length; i++)
                {
                    map[i] = new Node[15]; // Initialize each row

                    for (int j = 0; j < 15; j++)
                    {
                        map[i][j] = new Node(i, j); // Initialize each node in the row
                    }
                }
                //map[3][3].solid = true;
                //map[0][1].solid = true;
            }
            public int getCost(Node node, Node start, Node goal)
            {
                return Math.Abs(node.row - start.row) + Math.Abs(node.column - start.column) + Math.Abs(node.row - goal.row) + Math.Abs(node.column - goal.column);
            }

            public void calculateCost(Node start, Node goal)
            {
                for (int i = 0; i < map.Length; i++)
                {
                    for (int j = 0; j < map[0].Length; j++)
                    {
                        map[i][j].cost = getCost(map[i][j], start, goal);
                        map[i][j].row = i;
                        map[i][j].column = j;
                    }
                }
            }
            public void openNode(Node node, Node curr)
            {

                if (!node.open || !node.solid || !node.closed)
                {
                    node.open = true;
                    node.closed = false;
                    node.parent = curr;
                    opened.Add(node);
                    //Console.WriteLine("Opened node: " + node.row + "," + node.column);
                }

            }
            public void closeNode(Node node)
            {
                if (node.open || !node.solid || !node.closed)
                {
                    node.closed = true;
                    opened.Remove(node);
                    closed.Add(node);
                    //Console.WriteLine("___Closed node: " + node.row + "," + node.column);

                }
            }
            public Node getMincost()
            {
                if (opened.Count == 0)
                    return null;

                Node min_value = opened[0];

                for (int i = 1; i < opened.Count; i++)
                {
                    if (opened[i].cost < min_value.cost)
                    {
                        min_value = opened[i];
                    }
                }

                return min_value;
            }


            public void find_neighbours(Node node)
            {
                int x = node.row;
                int y = node.column;
                for (int i = (x - 1 < 0 ? 0 : x - 1); i < (x + 2 > 15 ? 15 : x + 2); i++)
                {
                    for (int j = (y - 1 < 0 ? 0 : y - 1); j < (y + 2 > 15 ? 15 : y + 2); j++)
                    {
                        if (map[i][j].open == false && map[i][j].solid==false)
                        {
                            openNode(map[i][j], node);
                        }
                    }
                }
            }
            public void back_track(Node node)
            {
                HashSet<Node> visitedNodes = new HashSet<Node>();
                Stack<Node> path = new Stack<Node>();

                while (node != null && !visitedNodes.Contains(node))
                {
                    path.Push(node);
                    visitedNodes.Add(node);
                    node = node.parent;
                }
                int path_length = path.Count;
                Console.WriteLine("length of the shortest path: " + path_length);
                foreach (var item in path)
                {
                    Thread.Sleep(300);
                    Console.WriteLine(item.row + "," + item.column);
                }
            }

            public void aStar(Node start, Node goal, Node curr)
            {

                start.solid = true;
                calculateCost(start, goal);
                opened.Add(curr);

                for (int i=0; i< opened.Count; i++)
                {
                    opened[i].solid = true;
                    curr = getMincost();

                    if (curr.row == goal.row && curr.column == goal.column)
                    {
                        
                        Console.WriteLine("End Reached");
                        back_track(curr);
                        return; // Found the goal, terminate the algorithm
                    }
                    else
                    {
                        start.solid = false;
                        opened[i].solid = false;
                        find_neighbours(curr);
                        closeNode(curr);
                        
                    }
                    
                }

                Console.WriteLine("No valid path to the goal.");
            }
        }
        class Program
        {
            

            static void Main(string[] args)
            {


                //Actor 1
                astar x = new astar();
                x.initializemap();
                Node start_node = new Node(0, 0);
                Node end_node = new Node(4, 4);
                Node curr_node = start_node;

                void DoWork1()
                {
                    x.aStar(start_node, end_node, curr_node);
                }
                //Actor 2
                astar y = new astar();
                y.initializemap();
                Node start_node2 = new Node(2, 2);
                Node end_node2 = new Node(5, 5);
                Node curr_node2 = start_node2;

                void DoWork2()
                {
                    y.aStar(start_node2, end_node2, curr_node2);
            }

            Thread thread1 = new Thread(DoWork1);
            Thread thread2 = new Thread(DoWork2);

            thread1.Start();
                thread2.Start();

                thread1.Join();
                thread2.Join();



            }
        }

    }
}

/// DFS

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Security.Cryptography.X509Certificates;
//namespace program
//{
//    class Node
//    {
//        public int row;
//        public int column;
//        public bool block = false;
//        public Node(int r, int c)
//        {
//            row = r;
//            column = c;
//        }

//    }
//    class DfsSimple
//    {
//        public static Node[][] map = new Node[15][];
//        public static int goalx = 2;
//        public static int goaly = 4;
//        public static void initializemap()
//        {
//            for (int i = 0; i < map.Length; i++)
//            {
//                map[i] = new Node[15]; // Initialize each row

//                for (int j = 0; j < 15; j++)
//                {
//                    map[i][j] = new Node(i, j); // Initialize each node in the row
//                }
//            }
//            map[1][1].block = true;
//            map[2][2].block = true;
//            map[2][3].block = true;
//        }
//        public static void find_neighbours(Node node, List<Node> neighbours)
//        {
//            int x = node.row;
//            int y = node.column;
//            for (int i = (x - 1 < 0 ? 0 : x - 1); i < (x + 2 > 15 ? 15 : x + 2); i++)
//            {
//                for (int j = (y - 1 < 0 ? 0 : y - 1); j < (y + 2 > 15 ? 15 : y + 2); j++)
//                {
//                    if (map[i][j].block == false)
//                    {
//                        neighbours.Add(map[i][j]);
//                    }
//                }
//            }
//        }
//        public static void DFS(List<Node> visited, Node node)
//        {
//            if (!visited.Contains(node))
//            {
//                visited.Add(node);

//                if (node.row == goalx && node.column == goaly)
//                {
//                    foreach (Node node2 in visited)
//                    {
//                        Console.WriteLine("-" + node2.row + " ," + node2.column);
//                    }
//                }

//                List<Node> neighbours = new List<Node>();
//                find_neighbours(node, neighbours);
//                foreach (Node neigh in neighbours)
//                {
//                    DFS(visited, neigh);
//                }
//            }
//        }
//        static void Main(String[] args)
//        {
//            initializemap();
//            List<Node> list = new List<Node>();
//            Node node = new Node(0, 0);
//            DFS(list, node);
//        }
//    }

//}

