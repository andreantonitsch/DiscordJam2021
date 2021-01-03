using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Armor,
        Damage,
        Spread
    }

    public bool Passthrough = false;
    public float Intensity = 0.0f;
    public Type type;

    public GameObject AttachmentPrefab;

    public void Apply(Node n)
    {
        switch (type)
        {
            case Type.Armor:
                n.CurrentStats.DamageResistance += Intensity;
                break;
            case Type.Damage:
                n.CurrentStats.AttackPower += Intensity;
                break;
            case Type.Spread:
                n.CurrentStats.CorruptingPower += Intensity;
                break;
        }

    }

    public void Attach_Attachments(Node n)
    {
        var o = Instantiate(AttachmentPrefab);
        o.transform.parent = n.transform;
        o.GetComponent<AttachmentAnimator>().Target = n.transform;
    }
}
