using System;
using System.Collections.Generic;

namespace LeoClient.Unity.Network
{
    public class LeoClientHelper
    {
        private Queue<Action> mainThreadQueue = new Queue<Action>();
        public void EnqueueOnMainThread(Action action)
        {
            lock (mainThreadQueue)
            {
                mainThreadQueue.Enqueue(action);
            }
        }

        public void ProcessQueue()
        {
            while (true)
            {
                Action action = null;
                lock (mainThreadQueue)
                {
                    if (mainThreadQueue.Count > 0)
                        action = mainThreadQueue.Dequeue();
                }

                if (action == null) break;
                action.Invoke();
            }
        }
    }
}