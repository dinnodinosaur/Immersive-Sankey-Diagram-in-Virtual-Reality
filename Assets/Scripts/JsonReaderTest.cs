using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Linq;
using UnityEditor;
using System.IO;
using System;
using UnityEngine.Windows;

public class JsonReaderTest : MonoBehaviour
{
    public GameObject Jsonreader;
    private RectTransform graphContainer;
    public NodesStructure[] NodesStructures;
    public LinksStructure[] LinksStructures;
    [Range(0,1)]
    public int second;
    double x0 = 1, y0 = 1, x1 = 800, y1 = 2000; // boundary of the Sankey in Unity 
    [Header("Attributes")]
    [Range(1, 100)]
    public double nodeWidth = 30; // nodeWidth
    [Range(0, 100)]
    public double nodePadding = 15;
    double py; // nodePadding
    int id;
    [Range(0,1)]
    public int if_ILP = 0;

    [Range(0,2)]
    public int LowOrHigh = 0;
    //
    public int[][] order = new int[][]
    {
        new int[] {0, 1, 2, 8, 7, 6, 5, 3, 4},
        new int[] {0, 1, 4, 2, 3},
        new int[] {0, 4, 1, 2, 3},
        new int[] {2, 1, 3, 0},
        new int[] {0, 1},
        new int[] {0, 2, 1, 3},
        new int[] {1, 0},
    };

    public int[][] order2 = new int[][]
    {
        new int[] {4, 2, 0, 3, 1, 8, 6, 7, 5, 10, 9},
        new int[] {4, 0, 1, 2, 5, 3, 6},
        new int[] {1, 0, 2},
        new int[] {4, 3, 0, 5, 6, 2, 1},
    };

    public int[][] order3 = new int[][]
    {
        new int[] {8, 4, 7, 9, 5, 1, 0, 10, 3, 6, 2},
        new int[] {2, 1, 0},
        new int[] {2, 3, 1, 0},
        new int[] {0, 4, 3, 1, 2},
        new int[] {0, 1},
        new int[] {4, 3, 0, 2, 1}
    };


    [Space(20)]
    public aligns align = aligns.justify;
    IComparable<NodesStructure> sort;
    IComparable<LinksStructure> linkSort;
    [Space(30)]
    public UnityEngine.Object jsFile;
    int links;
    public int iterations = 25;
    [Range(10, 100)]
    public double kz;

    public enum aligns { justify, left, right, center };
    void Start()
    {
        loadDate("");

    }

    public void loadDate(string transportFilepath)
    {
        Boolean FileRightFlag = true;
        if(second == 0){
            graphContainer = transform.parent.gameObject.GetComponent<RectTransform>();
        }
        else if(second == 1){
            graphContainer = transform.parent.parent.gameObject.GetComponent<RectTransform>();
        
        }
        x0 = 10;
        y0 = 10;
        x1 = x0 + graphContainer.sizeDelta.x - 10;
        y1 = graphContainer.sizeDelta.y + y0 - 20;
        string filepath = "";
        if (transportFilepath != "")
        {
            filepath = transportFilepath;
            if (filepath != "")
            {

                if (!filepath.EndsWith(".json"))
                {
                    Debug.Log(filepath + " is not a json file");
                    FileRightFlag = false;
                }

            }
        }
        else
        {
            if (jsFile != null)
            {
                if (transportFilepath == "")
                {
                    filepath = UnityEditor.AssetDatabase.GetAssetPath(jsFile);
                }
                if (!filepath.EndsWith(".json"))
                {
                    Debug.Log(filepath + " is not a json file");
                    FileRightFlag = false;
                }

            }

        }

        if (filepath != "" && FileRightFlag == true)
        {
            StreamReader streamreader = new StreamReader(filepath);
            JsonReader js = new JsonReader(streamreader);
            Root r = JsonMapper.ToObject<Root>(js);
            NodesStructures = new NodesStructure[r.nodes.Count];
            for (int i = 0; i < r.nodes.Count; i++)
            {

                NodesStructures[i] = new NodesStructure();
                NodesStructures[i].name = r.nodes[i].name;
                NodesStructures[i].nodePosition = -1;
                NodesStructures[i].layer = 999;
                NodesStructures[i].index = i;
                NodesStructures[i].SourceLinks = new List<LinksStructure>();
                NodesStructures[i].TargetLinks = new List<LinksStructure>();
            }
            LinksStructures = new LinksStructure[r.links.Count];
            for (int i = 0; i < r.links.Count; i++)
            {
                LinksStructures[i] = new LinksStructure();
                LinksStructures[i].value = r.links[i].value;
                LinksStructures[i].index = i;
                LinksStructures[i].SourceNode = NodesStructures[r.links[i].source];
                LinksStructures[i].TargetNode = NodesStructures[r.links[i].target];
                NodesStructures[r.links[i].source].SourceLinks.Add(LinksStructures[i]);
                NodesStructures[r.links[i].target].TargetLinks.Add(LinksStructures[i]);

            }
            for (int i = 0; i < NodesStructures.Length; i++)
            {
                NodesStructures[i].getvalue();
                // Debug.Log(i+ NodesStructures[i].name);
            }
         
            ComputeNodeHeights();
            ComputeNodeDepths();
            ComputeNodeBreadths();
            ComputeLinkBreadths();
            Jsonreader.SetActive(true);
            StartCoroutine(WaitForSeconds(3, () =>
            {
                gameObject.SetActive(false);
            }));
            if(second == 0){
                gameObject.transform.parent.GetComponent<NodeShow>().reloadFlag = true;
            }
            else if(second == 1){
                gameObject.transform.parent.parent.GetComponent<NodeShow>().reloadFlag = true;
            }
            
        }
    }

    public static IEnumerator WaitForSeconds(float duration, Action action = null)
    {
        yield return new WaitForSeconds(duration);
        action?.Invoke();
    }

    void Update()
    {

        ComputeNodeHeights();
        ComputeNodeDepths();

        ComputeNodeBreadths();
        ComputeLinkBreadths();

        gameObject.SetActive(false);
    }
    public void ComputeNodeBreadths()
    {
        ColumnNodes[] columns = computeNodeLayers();
        int max1 = columns.Max(c => c.Columnnode.Count);
        py = Math.Min(nodePadding, (y1 - y0) / (max1 - 1));
        initializeNodeBreadths(columns);
        for (var i = 0; i < iterations; ++i)
        {
            var alpha = Math.Pow(0.99f, i);
            var beta = Math.Max(1 - alpha, (i + 1) / iterations);
            if(if_ILP == 0){
                relaxRightToLeft(columns, alpha, beta);
                relaxLeftToRight(columns, alpha, beta);
            }
        }
    }
    private void relaxLeftToRight(ColumnNodes[] columns, double alpha, double beta)
    {
        for (int i = 1, n = columns.Length; i < n; ++i)
        {
            var column = columns[i];
            foreach (var target in column.Columnnode)
            {
                double y = 0;
                double w = 0;
                foreach (LinksStructure a in target.TargetLinks)
                {
                    double v = a.value * (target.layer - a.SourceNode.layer);
                    y += targetTop(a.SourceNode, target) * v;
                    w += v;
                }

                if (!(w > 0)) continue;
                var dy = (y / w - target.y0) * alpha;
                target.y0 += dy;
                target.y1 += dy;
                reorderNodeLinks(target);
            }
            if (sort == null) column.Columnnode.Sort(ascendingBreadth);
            resolveCollisions(column, beta);
        }
    }
    public void relaxRightToLeft(ColumnNodes[] columns, double alpha, double beta)
    {
        for (int n = columns.Length, i = n - 2; i >= 0; --i)
        {
            ColumnNodes column = columns[i];
            foreach (NodesStructure source in column.Columnnode)
            {
                double y = 0;
                double w = 0;

                foreach (LinksStructure a in source.SourceLinks)
                {
                    var v = a.value * (a.TargetNode.layer - source.layer);
                    y += sourceTop(source, a.TargetNode) * v;
                    w += v;
                }

                if (!(w > 0)) continue;
                var dy = (y / w - source.y0) * alpha;

                source.y0 += dy;
                source.y1 += dy;
                reorderNodeLinks(source);
            }
            if (sort == null) column.Columnnode.Sort(ascendingBreadth);
            resolveCollisions(column, beta);
        }

    }
    private double targetTop(NodesStructure source, NodesStructure target)
    {
        var y = source.y0 - (source.SourceLinks.Count - 1) * py / 2;
        foreach (LinksStructure a in source.SourceLinks)
        {
            if (a.TargetNode.Equals(target))
            {
                break;
            }
            y += a.width + py;
        }
        foreach (LinksStructure a in target.TargetLinks)
        {
            if (a.SourceNode.Equals(source))
            {
                break;
            }
            y -= a.width;
        }
        return y;
    }



    private void resolveCollisions(ColumnNodes nodes, double alpha)
    {
        var i = nodes.Columnnode.Count >> 1;
        var subject = nodes.Columnnode[i];
        resolveCollisionsBottomToTop(nodes, subject.y0 - py, i - 1, alpha);
        resolveCollisionsTopToBottom(nodes, subject.y1 + py, i + 1, alpha);
        resolveCollisionsBottomToTop(nodes, y1, nodes.Columnnode.Count - 1, alpha);
        resolveCollisionsTopToBottom(nodes, y0, 0, alpha);
    }

    private void resolveCollisionsTopToBottom(ColumnNodes nodes, double y, int i, double alpha)
    {
        for (; i < nodes.Columnnode.Count; ++i)
        {
            var node = nodes.Columnnode[i];
            var dy = (y - node.y0) * alpha;
            if (dy > 1e-6)
            {
                node.y0 += dy;
                node.y1 += dy;
            }
            y = node.y1 + py;
        }
    }

    private void resolveCollisionsBottomToTop(ColumnNodes nodes, double y, int i, double alpha)
    {
        for (; i >= 0; --i)
        {
            var node = nodes.Columnnode[i];
            var dy = (node.y1 - y) * alpha;
            if (dy > 1e-6)
            {
                node.y0 -= dy;
                node.y1 -= dy;
            }
            y = node.y0 - py;
        }
    }

    private void reorderNodeLinks(NodesStructure source)
    {
        foreach (LinksStructure a in source.TargetLinks)
        {

            a.SourceNode.SourceLinks.Sort(ascendingTargetBreadth);
        }
        foreach (LinksStructure a in source.SourceLinks)
        {
            a.TargetNode.TargetLinks.Sort(ascendingSourceBreadth);
        }
    }

    public double sourceTop(NodesStructure Sourcenode, NodesStructure Targetnode)
    {
        var y = Targetnode.y0 - (Targetnode.TargetLinks.Count - 1) * py / 2;
        foreach (LinksStructure a in Targetnode.TargetLinks)
        {
            if (a.SourceNode.Equals(Sourcenode))
            {
                break;
            }
            y += a.width + py;
        }
        foreach (LinksStructure a in Sourcenode.SourceLinks)
        {
            if (a.TargetNode.Equals(Targetnode))
            {
                break;
            }
            y -= a.width;
        }
        return y;
    }

    public void initializeNodeBreadths(ColumnNodes[] columns)
    {
        
        int time = 0;
        double ky = columns.Min(c => (y1 - y0 - ((c.Columnnode.Count - 1) * py)) / c.Columnnode.Sum(b => b.value));
        int[] currentOrder;
        foreach (var a in columns)
        {
            if(if_ILP == 1){
                if(LowOrHigh == 1){
                    currentOrder = order2[time];
                }
                else if (LowOrHigh == 0)
                {
                    currentOrder = order[time];
                }
                else
                {
                    currentOrder = order3[time];
                }

                    // 创建一个新列表来按照新顺序存放元素
                    List<NodesStructure> reorderedColumnNodes = new List<NodesStructure>();

                    // 根据 order 数组中的索引添加元素到新列表
                    foreach (int index in currentOrder)
                    {
                        if (index >= 0 && index < a.Columnnode.Count)
                        {
                            reorderedColumnNodes.Add(a.Columnnode[index]);
                        }
                    }
                    // 替换原 Columnnode 列表为新排序的列表
                    a.Columnnode = reorderedColumnNodes;
                    time++;
            
                var sortedNodes = a.Columnnode
                        .Select((node, index) => new { Node = node, Index = index })
                        .OrderByDescending(item => item.Node.value)
                        .ToList();
                for (int i = 0; i < sortedNodes.Count; i++)
                {
                    a.Columnnode[sortedNodes[i].Index].nodePosition = i + 1;

                }
            }

            
            // a.Columnnode.Sort((node1, node2) => node1.value.CompareTo(node2.value));
            var y = y0;
            foreach (var b in a.Columnnode)
            {
                b.y0 = y;
                b.y1 = (y + b.value * ky);
                y = b.y1 + py;
                foreach (var link in b.SourceLinks)
                {
                    link.width = (link.value * ky);
                }
            }
            y = (y1 - y + py) / (a.Columnnode.Count + 1);
            for (int i = 0; i < a.Columnnode.Count; i++)
            {
                var node = a.Columnnode[i];
                node.y0 += y * (i + 1);
                node.y1 += y * (i + 1);
                //Debug.Log(a.Columnnode[i].name);
                // Debug.Log(a.Columnnode[i].nodePosition);
                // Debug.Log("node.y0"+y0);
                // Debug.Log("node.y1"+y1);
            }
            
            reorderLinks(a.Columnnode);
        }

        
    }


    private void reorderLinks(List<NodesStructure> columnnode)
    {
        if (!(linkSort != null))
        {
            foreach (var node in columnnode)
            {
                node.SourceLinks.Sort(ascendingTargetBreadth);
                node.TargetLinks.Sort(ascendingSourceBreadth);

            }
        }
    }
    public void ComputeNodeDepths()
    {
        List<NodesStructure> current = new List<NodesStructure>(NodesStructures);
        List<NodesStructure> next = new List<NodesStructure>();
        int x = 0;
        while (current.Count != 0)
        {
            foreach (NodesStructure a in current)
            {
                a.depth = x;
                if (a.SourceLinks != null)
                {
                    foreach (LinksStructure b in a.SourceLinks)
                    {
                        next.Add(b.TargetNode);
                    }
                }
            }
            if (++x > NodesStructures.Length)
            {
                //Debug.Log("ERROR  x>n");
            }
            current = next;
            next = new List<NodesStructure>();
        }
    }
    public void ComputeLinkBreadths()
    {
        foreach (var node in NodesStructures)
        {
            var y0 = node.y0;
            var y1 = y0;
            //3d y start at 0
            var y0_3D = 0.00;
            var y1_3D = y0_3D;
            foreach (var link in node.SourceLinks)
            {
                link.y0 = y0 + link.width / 2;
                y0 += link.width;

                link.y0_3D = y0_3D + link.width / 2;
                y0_3D += link.width;
            }
            foreach (var link in node.TargetLinks)
            {
                link.y1 = y1 + link.width / 2;
                y1 += link.width;

                link.y1_3D = y1_3D + link.width / 2;
                y1_3D += link.width;
            }
        }
    }

    async public void ComputeNodeHeights()
    {
        List<NodesStructure> current = new List<NodesStructure>(NodesStructures);
        List<NodesStructure> next = new List<NodesStructure>();

        int x = 0;
        while (current.Count != 0)
        {
            foreach (NodesStructure a in current)
            {
                a.height = x;
                if (a.TargetLinks != null)
                {
                    foreach (LinksStructure b in a.TargetLinks)
                    {

                        next.Add(b.SourceNode);
                    }
                }
                // Debug.Log("height"+NodesStructures[1].height);
                
            }
          
            if (++x > NodesStructures.Length)
            {
                //Debug.Log("ERROR  x>n"); 
            }
            current = next;
            next = new List<NodesStructure>();
            
        }
    }
    public ColumnNodes[] computeNodeLayers()
    {
        int max = 0;
        for (int i = 0; i < NodesStructures.Length; i++)
        {
            if (NodesStructures[i].depth > max) max = NodesStructures[i].depth;
        }
        int x = NodesStructures.Max(c => c.depth) + 1;
        double kx = (x1 - x0 - nodeWidth) / (x - 1);
        ColumnNodes[] columns = new ColumnNodes[x];

        foreach (NodesStructure a in NodesStructures)
        {
            double temp = 0;
            switch (align)
            {
                case aligns.justify:
                    temp = justify(a, x);
                    break;
                case aligns.center:
                    temp = center(a);
                    break;
                case aligns.right:
                    temp = right(a, x);
                    break;
                case aligns.left:
                    temp = left(a);
                    break;
                default:
                    break;

            }
            int i = Math.Max(0, Math.Min(x - 1, (int)Math.Floor(temp)));
            a.layer = i;
            a.x0 = (x0 + i * kx);
            a.x1 = a.x0 + nodeWidth;
            push(a, i, columns);
        }
        if (sort != null) foreach (var column in columns)
            {
                // column.Columnnode.Sort((IComparer<NodesStructure>)sort);
            }
        return columns;

    }

    private double left(NodesStructure a)
    {
        return a.depth;
    }

    private double right(NodesStructure a, int x)
    {
        return x - 1 - a.height;
    }

    private double center(NodesStructure a)
    {
        return a.TargetLinks.Count != 0 ? a.depth
    : a.SourceLinks.Count != 0 ? a.SourceLinks.Min(c => c.TargetNode.depth) - 1
    : 0;
    }

    public void push(NodesStructure a, int i, ColumnNodes[] b)
    {
        if (b[i] == null)
        {
            a.yDepth = 0;
            b[i] = new ColumnNodes();
            b[i].Columnnode = new List<NodesStructure>();
            b[i].Columnnode.Add(a);
        }
        else
        {
            a.yDepth = b[i].Columnnode.Count;
            b[i].Columnnode.Add(a);
        }
    }

    public int justify(NodesStructure a, int n)
    {
        return a.SourceLinks.Count > 0 ? a.depth : n - 1;
    }


    int ascendingSourceBreadth(LinksStructure a, LinksStructure b)
    {
        if (ascendingBreadth(a.SourceNode, b.SourceNode) == 0) { return a.index - b.index; }
        return ascendingBreadth(a.SourceNode, b.SourceNode);
    }

    int ascendingTargetBreadth(LinksStructure a, LinksStructure b)
    {
        if (ascendingBreadth(a.TargetNode, b.TargetNode) == 0) { return a.index - b.index; }
        return ascendingBreadth(a.TargetNode, b.TargetNode);
    }

    int ascendingBreadth(NodesStructure a, NodesStructure b)
    {

        if ((a.y0 - b.y0) == 0) return 0;
        if ((a.y0 - b.y0) < 0) return -1;
        if ((a.y0 - b.y0) > 0) return 1;
        return (int)(a.y0 - b.y0);
    }
}

public class NodesItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
    }

public class NodesStructure
{
    public double nodePosition { get; set;}
    public string name { get; set; }
    //x0:
    public double x0 { get; set; }
    //x1:
    public double x1 { get; set; }
    //y0:
    public double y0 { get; set; }
    //y1:
    public double y1 { get; set; }



    public int index { get; set; }
    public int depth { get; set; }
    public int height{ get; set; }
    public int layer { get; set; }
    public double value { get; set; }
    public int yDepth { get; set; }

    public List<LinksStructure> SourceLinks
    {
        get;set;
    }
    public List<LinksStructure> TargetLinks { get; set; }

    public void getvalue()
    {
        double sourceValue = 0,targetValue=0;
        if(SourceLinks != null)
        {
            for(int i = 0; i < SourceLinks.Count; i++)
            {
                sourceValue += SourceLinks[i].value;
            }
        }
        else
        {
            this.depth = 0;
        }

        if (TargetLinks != null)
        {
            
            for (int i = 0; i < TargetLinks.Count; i++)
            {
                targetValue += TargetLinks[i].value;
            }
            
        }
        this.value = Math.Max(sourceValue, targetValue);
    }

}


public class ColumnNodes
{
    public List<NodesStructure> Columnnode { get; set; }
}


public class LinksStructure
{
    public double value { get; set; }
    public double y0 { get; set; }
    public double y1 { get; set; }
    public double y0_3D { get; set; }
    public double y1_3D { get; set; }
    public double width { get; set; }
    public int index { get; set; }
    public NodesStructure SourceNode { get; set; }
    public NodesStructure TargetNode { get; set; }
}

public class LinksItem
    {
        public int source { get; set; }
        public int target { get; set; }
        public double value { get; set; }
    }

    public class Root
    {
        public List<NodesItem> nodes { get; set; }
        public List<LinksItem> links { get; set; }
    }

