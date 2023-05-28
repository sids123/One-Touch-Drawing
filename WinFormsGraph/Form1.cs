namespace WinFormsGraph
{
    public partial class Form1 : Form
    {
        Point point;
        Point startingPoint;
        Point finishingPoint;

        bool isMouseDown = false;
        bool isDoneWithVertices = false;
        bool isAEdge = false;
        bool hasSolutions = false;
        bool isFinishedDrawing = false;

        Rectangle vertice;
        List<Rectangle> vertices;
        Edge edgeInGraph;
        List<Edge> edges;
        int[,] graph;

        List<int[]> solutions;

        int startingVertice;
        int finishingVertice;

        Graphics g;
        Brush drawingBrush;
        Pen drawingPen;

        int heightAndWidth = 20;

        int stateOfButton = 0;

        int index = 0;

        int currentSolution = 0;

        Point vertice1;
        Point middlePoint;
        Point vertice2;
        int firstVertice;
        int secondVertice;

        public Form1()
        {
            InitializeComponent();
            vertices = new List<Rectangle>();
            edges = new List<Edge>(); 
            drawingBrush = new SolidBrush(Color.Red);
            drawingPen = new Pen(Color.Black, 10);
            g = CreateGraphics();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDoneWithVertices && !isFinishedDrawing)
            {
                point = e.Location;
                isMouseDown = true;
                if (point.Y < 450)
                {
                    for (int i = 0; i < vertices.Count; i++)
                    {
                        if (IsClickOnVertice(vertices[i], point))
                        {
                            startingVertice = i;
                            startingPoint = point;
                            isAEdge = true;
                        }
                    }
                }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMouseDown && isDoneWithVertices && !isFinishedDrawing)
            {
                point = e.Location;
                isMouseDown = false;
                if (point.Y < 450 && isAEdge)
                {
                    isAEdge = false;
                    for (int i = 0; i < vertices.Count; i++)
                    {
                        if (IsClickOnVertice(vertices[i], point))
                        {
                            finishingVertice = i;
                            finishingPoint = point;
                            isAEdge = true;
                        }
                    }
                    if (isAEdge)
                    {
                        edgeInGraph = new Edge(startingPoint, finishingPoint, startingVertice, finishingVertice);
                        edges.Add(edgeInGraph);
                        graph[startingVertice, finishingVertice]++;
                        graph[finishingVertice, startingVertice]++;
                        g.DrawLine(drawingPen, startingPoint, finishingPoint);
                    }
                    isAEdge = false;
                }
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isDoneWithVertices)
            {
                point = e.Location;
                if (point.Y < 450)
                {
                    vertice = new Rectangle(point.X, point.Y, heightAndWidth, heightAndWidth);
                    g.FillRectangle(drawingBrush, vertice);
                    vertices.Add(vertice);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (stateOfButton == 0)
            {
                isDoneWithVertices = true;
                heightAndWidth = 10;
                button1.Text = "Done";
                graph = new int[vertices.Count, vertices.Count];
                stateOfButton++;
            }
            else if (stateOfButton == 1)
            {
                stateOfButton++;
                isFinishedDrawing = true;
                Refresh();
                GetAllEulerianRoutes();
                if (solutions == null)
                {
                    hasSolutions = false;
                    g.DrawString("No Solution", new Font("Arial", 30), drawingBrush, new PointF(250f, 175f));
                    button1.Text = "";
                }
                else
                {
                    hasSolutions = true;
                    for (int i = 0; i < vertices.Count; i++)
                        g.FillRectangle(drawingBrush, vertices[i]);
                    button1.Text = "Next";
                }
            }
            else if (stateOfButton == 2 && hasSolutions)
            {
                if (index + 1 == solutions[currentSolution].Length)
                {
                    stateOfButton++;
                    button1.Text = "Click To See Another Solution Or Exit Form";
                }
                else
                {
                    firstVertice = solutions[currentSolution][index];
                    secondVertice = solutions[currentSolution][index + 1];
                    vertice1 = new Point(vertices[firstVertice].X, vertices[firstVertice].Y);
                    vertice2 = new Point(vertices[secondVertice].X, vertices[secondVertice].Y);
                    middlePoint = new Point((vertice1.X + vertice2.X) / 2, (vertice1.Y + vertice2.Y) / 2);
                    g.DrawLine(drawingPen, vertice1, middlePoint);
                    Thread.Sleep(500);
                    g.DrawLine(drawingPen, middlePoint, vertice2);
                    index++;
                }
            }
            else if (stateOfButton == 3)
            {
                currentSolution++;
                if (currentSolution == solutions.Count)
                {
                    button1.Text = "";
                    Refresh();
                    g.DrawString("No More Solutions", new Font("Arial", 30), drawingBrush, new PointF(200f, 175f));
                    stateOfButton = 4;
                }
                else
                {
                    stateOfButton--;
                    index = 0;
                    Refresh();
                    for (int i = 0; i < vertices.Count; i++)
                        g.FillRectangle(drawingBrush, vertices[i]);
                    button1.Text = "Next";
                }
            }
        }

        public bool IsClickOnVertice(Rectangle vertice, Point point)
        {
            return point.X > vertice.X && point.X < vertice.X + vertice.Width &&
                   point.Y > vertice.Y && point.Y < vertice.Y + vertice.Height;
        }

        public void GetAllEulerianRoutes()
        {
            Graph graphMatrix = new Graph(vertices.Count, edges.Count, (int[,])graph.Clone());
            solutions = graphMatrix.GetAllEulerianRoutes();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Refresh();
            isMouseDown = false;
            isDoneWithVertices = false;
            isAEdge = false;
            hasSolutions = false;
            isFinishedDrawing = false;

            heightAndWidth = 20;

            stateOfButton = 0;

            index = 0;

            currentSolution = 0;

            vertices = new List<Rectangle>();
            edges = new List<Edge>();

            button1.Text = "Done With Vertices";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}