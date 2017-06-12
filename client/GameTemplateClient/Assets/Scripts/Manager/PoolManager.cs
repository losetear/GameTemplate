using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameTemplate
{
    public static class PoolManager
    {
    }

    public class Pool<T> where T : class
    {
        private readonly Func<T> create;
        private readonly Action<T> kill;
        private readonly Action<T> setToSleep;
        private readonly Action<T> wakeUp;
        private readonly List<T> poolObjects;
        private int firstSleepingObjectIndex;

        /// <summary>
        /// The number total objects in the pool (awake and asleep).
        /// </summary>
        /// <value>The capacity.</value>
        public int Capacity
        {
            get { return poolObjects.Count; }
        }

        /// <summary>
        /// Returns whether there is a sleeping object available.
        /// </summary>
        /// <value><c>true</c> if this a sleeping object is available; otherwise, <c>false</c>.</value>
        public bool IsObjectAvailable
        {
            get { return firstSleepingObjectIndex < Capacity; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pool{T}"/> class.
        /// </summary>
        /// <param name="initialCount">The initial number of objects to create.</param>
        /// <param name="create">A function that creates a new object of type T.</param>
        /// <param name="kill">The function that destroys an object of type T.</param>
        /// <param name="setToSleep">A function called when an object is set to sleep.</param>
        /// <param name="wakeUp">A function called when an object is woken up.</param>
        public Pool(int initialCount, Func<T> create, Action<T> kill, Action<T> setToSleep, Action<T> wakeUp)
        {
            this.create = create;
            this.kill = kill;
            this.setToSleep = setToSleep;
            this.wakeUp = wakeUp;

            poolObjects = new List<T>();
            firstSleepingObjectIndex = 0;

            Create(initialCount);
        }

        public void AllToSleep()
        {
            firstSleepingObjectIndex = 0;
        }

        public void ClearPool()
        {
            poolObjects.Clear();
            firstSleepingObjectIndex = 0;
        }

        /// <summary>
        /// Gets a new object from the pool.
        /// </summary>
        /// <returns>A freshly awakened object.</returns>
        /// <exception cref="InvalidOperationException">No items in pool</exception>
        public T GetNewObject()
        {
            if (!IsObjectAvailable)
            {
                var obj = create();
                poolObjects.Add(obj);
            }
            var newObj = poolObjects[firstSleepingObjectIndex];
            firstSleepingObjectIndex++;
            if (wakeUp != null)
                wakeUp(newObj);
            return newObj;

            //throw new InvalidOperationException("No items in pool");
        }

        /// <summary>
        /// Releases the specified object back to the pool.
        /// </summary>
        /// <param name="obj">The object to release.</param>
        public void Release(T obj)
        {
            SetToSleep(obj);
        }

        /// <summary>
        /// Increases thew capacity of the pool. 
        /// </summary>
        /// <param name="increment">The number of new pool objects to add.</param>
        public void IncCapacity(int increment)
        {
            Create(increment);
        }

        /// <summary>
        /// Decreases the capacity of the pool.
        /// </summary>
        /// <param name="decrement">The number of pool objects to kill.</param>
        public void DecCapacity(int decrement)
        {
            int remainingObjectsCount = Mathf.Max(0, Capacity - decrement);

            //Kill objects that are awake first
            var objectsToKill = poolObjects.Skip(remainingObjectsCount);

            foreach (var obj in objectsToKill)
            {
                Kill(obj);
            }
        }

        private void Kill(T obj)
        {
            SetToSleep(obj); //Kill object humanely	
            poolObjects.Remove(obj);
            kill(obj);
        }

        private void Create(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = create();
                poolObjects.Add(obj);

                SetToSleep(obj);
            }
        }

        private void SetToSleep(T obj)
        {
            int index = poolObjects.IndexOf(obj);

            if (index < firstSleepingObjectIndex)
            {
                setToSleep(obj);

                int lastAwakeIndex = firstSleepingObjectIndex - 1;

                SwapObjects(lastAwakeIndex, index);

                firstSleepingObjectIndex = lastAwakeIndex;
            }
        }

        private void SwapObjects(int index1, int index2)
        {
            var tmp = poolObjects[index1];
            poolObjects[index1] = poolObjects[index2];
            poolObjects[index2] = tmp;
        }
    }
}

