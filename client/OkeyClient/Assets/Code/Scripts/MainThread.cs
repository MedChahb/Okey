using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> queue = new Queue<Action>();
    private static MainThreadDispatcher instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void Enqueue(Action action)
    {
        lock (queue)
        {
            queue.Enqueue(action);
        }
    }

    private void Update()
    {
        while (queue.Count > 0)
        {
            Action action = null;
            lock (queue)
            {
                if (queue.Count > 0)
                {
                    action = queue.Dequeue();
                }
            }
            action?.Invoke();
        }
    }
}
