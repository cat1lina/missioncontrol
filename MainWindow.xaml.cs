using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using static System.Windows.Forms.AxHost;

namespace WPf_try2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        
    }
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

        astar A = new astar();
        class astar
        {
            Node[][] map = new Node[15][];
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
            }

            public void SetSolid(int x, int y)
            {
                map[x][y].solid= true;
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
                        if (map[i][j].open == false)
                        {
                            openNode(map[i][j], node);
                        }
                    }
                }
            }
            public static  Stack<Node> back_track(Node node)
            {
                HashSet<Node> visitedNodes = new HashSet<Node>();
                Stack<Node> path = new Stack<Node>();

                while (node != null && !visitedNodes.Contains(node))
                {
                    path.Push(node);
                    visitedNodes.Add(node);
                    node = node.parent;
                }
                return path;
            }

            public Stack<Node> aStar(Node start, Node goal, Node curr)
            {
                calculateCost(start, goal);
                opened.Add(curr);
                Stack<Node> path = new Stack<Node>();

                while (opened.Count > 0)
                {
                    curr = getMincost();
                    if (curr.row == goal.row && curr.column == goal.column)
                    {
                    //    Console.WriteLine("End Reached");
                        path = back_track(curr);
                        return path; // Found the goal, terminate the algorithm
                    }
                    else
                    {
                        find_neighbours(curr);
                        closeNode(curr);
                    }
                }
                return null;
                //Console.WriteLine("No valid path to the goal.");
            }
        }

         
        public bool first = true;

        

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if(first) 
            {
                A.initializemap();
                first = false;
            }
            ToggleButton togglebutton = (ToggleButton)sender;
            var row = Grid.GetRow(togglebutton);
            var column = Grid.GetColumn(togglebutton);
            A.SetSolid(row, column);
            togglebutton.Background = Brushes.Red;
            //Git deneme
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            if(first)
            {
                A.initializemap();
            }
            Stack<Node> path = new Stack<Node>();
            var startbutton = (Button)sender;
            int startx = Int32.Parse(start_x.Text);
            int starty = Int32.Parse(start_y.Text);
            int endx = Int32.Parse(end_x.Text);
            int endy = Int32.Parse(end_y.Text);

            Node start = new Node(startx, starty);
            Node end = new Node(endx, endy);

            path = A.aStar(start, end, start);
            String string_path = " path: ";
            foreach( Node node in path ) 
            {
                string_path += "( " + node.row + ", " + node.column + " )"; 
            }
            result.Text = string_path;

        }


    }
}
