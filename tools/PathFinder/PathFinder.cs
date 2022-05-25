using System.Collections.Generic;


// NOTE: don't use this
// FIXME: not working correctly

namespace dove {
public class DFinder
{
    internal class PNode{
        public PNode(INode _node, float _F, PNode _previous) {
            inode = _node;
            F = _F;
            previous = _previous;
        }    
        public float F;
        public PNode previous;
        public INode inode;
    }

    internal class Path {
        public LinkedList<PNode> Open { get => open; }
        private LinkedList<PNode> open = new LinkedList<PNode>();

        public LinkedList<PNode> Close { get => close;}
        private LinkedList<PNode> close = new LinkedList<PNode>();

        public Dictionary<int, INode> used = new Dictionary<int, INode>();
        
        public void AddToOpen(PNode pnode) {
            if (open.Count == 0) {
                open.AddLast(pnode);
                used[pnode.inode.Hash()] = pnode.inode;
            } else {
                for (var ite = open.Last; ite != null; ite = ite.Previous) {
                    if (ite.Value.F < pnode.F) {
                        open.AddAfter(ite, pnode);
                        used[pnode.inode.Hash()] = pnode.inode;
                        return;
                    }
                }
                open.AddLast(pnode);
                used[pnode.inode.Hash()] = pnode.inode;
            }
        }
        public void MoveToClose(PNode pnode) {
            close.AddLast(pnode);
            open.Remove(pnode);
        }
    }

    public static LinkedList<T> FindPath<T> (T _start, T _end) where T : class, INode{
        if (!_start.IsWalkable() || !_end.IsWalkable())
            return null;
      
        INode start = _start as INode;
        INode end = _end as INode;

        Path path = new Path();

        path.AddToOpen(new PNode(start, start.CalcCost(_end), null));

        int iteCount = 0;
        while(true) {
            if (path.Open.Count == 0 && iteCount != 0) return null; // fail

            var bestNode = path.Open.First.Value;
            var around = bestNode.inode.GetAround();

            path.MoveToClose(bestNode);

            if (bestNode.inode == end) {
                LinkedList<T> output = new LinkedList<T>();

                PNode pnode = path.Close.Last.Value;
                while (true) {
                    if (pnode != null) {
                        output.AddFirst(pnode.inode as T);
                        pnode = pnode.previous;
                    } else {
                        break;
                    }
                }
                return output;
            }
            foreach (INode node in around)
            {
                if (node.IsWalkable() && !path.used.ContainsKey(node.Hash()))
                    path.AddToOpen(new PNode(node, node.CalcCost(start) + node.CalcCost(end), bestNode));
            }
            iteCount++;
        }
    }
}

public interface INode {
    List<INode> GetAround();
    bool IsWalkable();
    float CalcCost(INode target);
    int Hash();
}
}
