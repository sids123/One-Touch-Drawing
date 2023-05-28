using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsGraph
{
    internal class Edge : Form
    {
        public Point point1 { get; private set; }
        public Point point2 { get; private set; }
        public int StartingVertice { get; private set; }
        public int FinishingVertice { get; private set; }

        public Edge(Point point1, Point point2, int startingVertice, int finishingVertice)
        {
            this.point1 = point1;
            this.point2 = point2;
            StartingVertice = startingVertice;
            FinishingVertice = finishingVertice;
        }
    }
}
