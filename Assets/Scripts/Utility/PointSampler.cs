using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSampler : MonoBehaviour
{
    public GameObject DebugObject;
    static PointSampler instance;
    public static PointSampler Instance 
    {
        get 
        {
            if (instance == null)
                instance = GameObject.Find("PointSampler").GetComponent<PointSampler>();
            return instance;   
        } 
    }

    public BaseParameters bp;

    public float Radius = 0.1f;
    public int Attempts = 30;

    public void Start()
    {
        bp = BaseParameters.Instance;
    }

    // Update is called once per frame
    public List<Vector2> SamplePoints()
    {
        var Domain = bp.Domain;
        var offset = bp.Offset;

        var sampler = new PoissonDiscSampler(Domain.y - Domain.x - 0.1f, Domain.w - Domain.z - 0.1f, Radius) ;

        List<Vector2> l = new List<Vector2>();

        foreach(Vector2 s in sampler.Samples())
        {
            l.Add(s + new Vector2(Domain.x +0.05f, Domain.z + 0.05f));
        }

        return l;
    }
}
