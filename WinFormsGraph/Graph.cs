using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsGraph
{
    internal class Graph
    {
        #region Properties
        private readonly int _numberOfVertices;
        private int _amountOfEdges;
        private readonly int[,] _graphMatrix;
        private int[] _ranks; //Initialize function: GetRanksOfVerticesInGraph()
        private bool? _isEulerianGraph; //Initialize function: IsEulerianGraph()
        private List<List<int>> _connectedComponents; //Initialize function: BFS()

        /* All properties with Initializing function have one perpose, which is to store the return value of their
         initializing function so we don't have to run the same function over and over, but instead just check the 
        value of their properties.
        IMPORTANT: do not access those propperties directly, instead, use their initializing function.*/
        #endregion

        #region Ctor
        public Graph(int numberOfVertices, int amountOfEdges, int[,] graphMatrix)
        {
            _numberOfVertices = numberOfVertices;
            _amountOfEdges = amountOfEdges;
            _graphMatrix = graphMatrix;
            _ranks = null;
            _connectedComponents = null;
            _isEulerianGraph = null;
        }

        #endregion

        #region  Methods & Searches
        public int[] GetRanksOfVerticesInGraph()
        {
            if (_ranks != null)
                return _ranks;
            _ranks = new int[_numberOfVertices];
            for (int i = 0; i < _numberOfVertices; i++)
                _ranks[i] = 0;
            for (int i = 0; i < _numberOfVertices; i++)
            {
                for (int j = 0; j < _numberOfVertices; j++)
                {
                    _ranks[i] += _graphMatrix[i, j];
                }
            }
            return _ranks;
        }

        public List<List<int>> BFS()
        {
            if (_connectedComponents != null)
                return _connectedComponents;
            _connectedComponents = new List<List<int>>();
            List<int> components = new List<int>();
            int vertice;
            bool[] visited = new bool[_numberOfVertices];
            for (int i = 0; i < _numberOfVertices; i++)
            {
                visited[i] = false;
            }

            Queue<int> queue = new Queue<int>();
            for (int i = 0; i < _numberOfVertices; i++)
            {
                vertice = i;
                if (!visited[vertice])
                {
                    visited[vertice] = true;
                    queue.Enqueue(vertice);
                    components.Add(vertice);

                    while (queue.Count != 0)
                    {
                        vertice = queue.Dequeue();
                        for (int j = 0; j < _numberOfVertices; j++)
                        {
                            if (_graphMatrix[vertice, j] != 0 && !visited[j])
                            {
                                visited[j] = true;
                                queue.Enqueue(j);
                                components.Add(j);
                            }
                        }
                    }
                    _connectedComponents.Add(components);
                    components = new List<int>();
                }
            }
            return _connectedComponents;
        }

        public bool IsConnectdGraph()
        {
            return BFS().Count == 1;
        }

        public bool IsEulerianGraph()
        {
            if (_isEulerianGraph != null)
                return (bool)_isEulerianGraph;
            int counterVerticesWithOddRanks = 0;
            if (!IsConnectdGraph())
                return false;
            for (int i = 0; i < _numberOfVertices; i++)
            {
                if (GetRanksOfVerticesInGraph()[i] % 2 != 0)
                    counterVerticesWithOddRanks++;
            }
            _isEulerianGraph = counterVerticesWithOddRanks == 2 || counterVerticesWithOddRanks == 0;
            return (bool)_isEulerianGraph;
        }

        public List<int[]> GetAllEulerianRoutes()
        {
            if (!IsEulerianGraph())
                return null;
            List<int[]> solutions = new List<int[]>();
            Stack<int> iStack = new Stack<int>();
            int[] path;
            int i1 = 0, j1 = 0, firstI, maxArches = _amountOfEdges;

            for (int i = 0; i < _graphMatrix.GetLength(0); i++)
            {
                i1 = i;
                iStack.Push(i);
                firstI = i1;

                while (!(firstI == i1 && _amountOfEdges == maxArches && j1 == _graphMatrix.GetLength(1)))
                {
                    //back if
                    if (j1 == _graphMatrix.GetLength(0) || _amountOfEdges == 0)
                    {
                        _amountOfEdges++;
                        iStack.Pop();
                        j1 = i1;
                        i1 = iStack.Peek();
                        _graphMatrix[i1, j1]++;
                        _graphMatrix[j1, i1]++;
                        j1++;
                    }
                    //entry if
                    else if (_graphMatrix[i1, j1] != 0)
                    {
                        _amountOfEdges--;
                        _graphMatrix[i1, j1]--;
                        _graphMatrix[j1, i1]--;
                        i1 = j1;
                        j1 = 0;
                        iStack.Push(i1);
                    }
                    //move if
                    else
                        j1++;
                    //full path if
                    if (_amountOfEdges == 0)
                    {
                        path = iStack.ToArray();
                        Array.Reverse(path);
                        solutions.Add(path);
                    }
                }
                //reseting
                j1 = 0;
                maxArches = _amountOfEdges;
                while (iStack.Count() > 0)
                    iStack.Pop();
            }
            return solutions;
        }
        #endregion 
    }
}
