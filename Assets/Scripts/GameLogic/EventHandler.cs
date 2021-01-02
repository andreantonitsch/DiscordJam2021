using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{

    static EventHandler instance;
    public static EventHandler Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("EventHandler").GetComponent<EventHandler>();
            return instance;
        }
    }

    public Dictionary<Event.EventType, List<EventListener>> Subs = new Dictionary<Event.EventType, List<EventListener>>();


    public Queue<Event> event_queue = new Queue<Event>();
    public void Queue(Event e)
    {
        event_queue.Enqueue(e);
    }

    public void Push(Event e)
    {
        var type = e.Type;
        bool has_subs = Subs.TryGetValue(type, out List<EventListener> event_subs);
        if (!has_subs)
            return;

        foreach(EventListener el in event_subs)
        {
            el.Consume(e);
        }
    }

    public void Sub(Event.EventType type, EventListener el)
    {
        bool has_subs = Subs.TryGetValue(type, out List<EventListener> event_subs);

        if (has_subs)
            event_subs.Add(el);
        else
        {
            var sub_list = new List<EventListener>();
            sub_list.Add(el);
            Subs.Add(type, sub_list);
        }
    }

    public void Unsub(Event.EventType type, EventListener el)
    {
        bool has_subs = Subs.TryGetValue(type, out List<EventListener> event_subs);
        if (!has_subs)
            return;
        else
        {
            event_subs.Remove(el);
        }

    }

}
