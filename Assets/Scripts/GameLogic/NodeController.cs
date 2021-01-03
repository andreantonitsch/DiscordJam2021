using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : EventListener
{
    static NodeController instance;
    public static NodeController Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("NodeController").GetComponent<NodeController>();
            return instance;
        }
    }

    public EventHandler eh;
    public BaseParameters bp;
    public Transform NodeHolder;

    public Material FreeCityMaterial;
    public Material CorruptCityMaterial;

    public Dictionary<int, Node> IDs;
    public List<Node> Nodes;

    public List<Node> null_parents;

    public List<Node> CorruptNodes;

    public GameObject NodePrefab;

    public PointSampler sampler;

    public Node RootNode;

    // Start is called before the first frame update
    void Start()
    {
        bp = BaseParameters.Instance;
        IDs = new Dictionary<int, Node>();
        Nodes = new List<Node>();
        null_parents = new List<Node>();
        sampler = PointSampler.Instance;
        eh = EventHandler.Instance;

        eh.Sub(Event.EventType.CorruptNode, this);
        eh.Sub(Event.EventType.NodeDestroyed, this);
    }



    public void SpawnNodes()
    {
        List<Vector2> positions = sampler.SamplePoints();

        // Spawn Root Mycelium Node
        RootNode = CreateNode(positions[0], true);
        eh.Sub(Event.EventType.NodeAttackTick, RootNode);
        RootNode.ID = 0;
        Nodes.Add(RootNode);
        IDs.Add(0, RootNode);
        CorruptNodes.Add(RootNode);
        null_parents.Add(RootNode);

        int ids = 1;
        for (int i = 1; i < positions.Count; i++)
        {
            var n = CreateNode(positions[i]);
            n.ID = ids;
            Nodes.Add(n);
            IDs.Add(ids, n);
            ids++;
        }

        float dist = bp.NeighborDistance;
        do
        {
            foreach (var n in Nodes)
                n.FindNeighbors(dist);
            dist += 0.01f;
        } while (!CheckNeighbors());

        eh.Push(new Event(Event.EventType.NodesSpawned));
    }

    public bool CheckNeighbors()
    {
        bool has_neigh = true;
        foreach (var n in Nodes)
            has_neigh &= n.Neighbors.Count > 0;

        return has_neigh;
    }


    public void FillNode(Node n, Vector2 position, bool Corrupt = false)
    {
        var base_stat_block = new NodeStats();
        base_stat_block.Init();
        n.BaseStats = base_stat_block;

        n.transform.position = new Vector3(position.x, position.y, n.transform.position.z);
        n.transform.parent = NodeHolder;
        n.PowerUpSlots = Random.Range(bp.NodeSlotsRange.x, bp.NodeSlotsRange.y);

        if (Corrupt)
        {
            n.Free = false;
            n.Corruption = n.BaseStats.CorruptionHP;
        }

        n.UpdateStats();

    }

    public void UpdatePowerUps()
    {

    }

    public Node CreateNode(Vector2 pos, bool Corrupt = false)
    {
        Node n = GameObject.Instantiate(NodePrefab).GetComponent<Node>();
        FillNode(n, pos, Corrupt);
        n.transform.parent = NodeHolder;
        n.Init();

        n.Free = !Corrupt;

        if (Corrupt)
            n.GetComponent<SpriteRenderer>().material = CorruptCityMaterial;
        else
        {
            eh.Sub(Event.EventType.SoldierSpawnTick, n);
            n.GetComponent<SpriteRenderer>().material = FreeCityMaterial;
        }

        return n;
    }


    public void CleanupNode(Node destroyed_node)
    {
        if (destroyed_node == null)
            return;
        foreach (var n in destroyed_node.Children)
        {
            if (n == null)
                continue;

            n.Parent = null;
            null_parents.Add(n);
        }

        foreach(var n in Nodes)
        {
            n.Neighbors.Remove(destroyed_node);
        }
        if (destroyed_node.Parent != null)
            destroyed_node.Parent.Children.Remove(destroyed_node);

        IDs.Remove(destroyed_node.ID);
        Nodes.Remove(destroyed_node);

        null_parents.Remove(destroyed_node);

        CorruptNodes.Remove(destroyed_node);


         Destroy(destroyed_node.gameObject);

    }

    public void CorruptNode(Node parent, Node child)
    {
        child.GetComponent<SpriteRenderer>().material = CorruptCityMaterial;
        parent.Children.Add(child);
        child.Parent = parent;
        child.Free = false;

        CorruptNodes.Add(child);

    }

    public override void Consume(Event e)
    {

        switch (e.Type)
        {
            case Event.EventType.CorruptNode:
                IDs.TryGetValue(e.i_val1, out Node parent);
                IDs.TryGetValue(e.i_val2, out Node child);

                CorruptNode(parent, child);
                break; 
            case Event.EventType.NodeDestroyed:
                IDs.TryGetValue(e.i_val1, out Node destroyed_node);
                CleanupNode(destroyed_node);
                break; 
            case Event.EventType.NodeRecaptured:
                
                break;
            case Event.EventType.UpdatePowerUps:
                UpdatePowerUps();
                break;
            default:
                break;
        }

    }
}
