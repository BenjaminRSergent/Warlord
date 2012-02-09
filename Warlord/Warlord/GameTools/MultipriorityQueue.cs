using System;
using System.Collections.Generic;

namespace GameTools
{
    class MultipriorityQueue<value, data> where value : IComparable
    {
        private List<KeyValuePair<value, data>> priorityList;

        public MultipriorityQueue()
        {
            priorityList = new List<KeyValuePair<value, data>>();
        }

        public void Add(value theValue, data theData)
        {
            int insertIndex = 0;

            foreach(KeyValuePair<value, data> pair in priorityList)
            {
                if(pair.Key.CompareTo(theValue) <= 0)
                    insertIndex++;
                else
                    break;
            }

            priorityList.Insert(insertIndex, new KeyValuePair<value, data>(theValue, theData));
        }

        public List<data> GetAndRemove(value cieling)
        {
            List<data> dataList = new List<data>();

            foreach(KeyValuePair<value, data> pair in priorityList)
            {
                if(pair.Key.CompareTo(cieling) <= 0)
                {
                    dataList.Add(pair.Value);
                }
                else
                    break;
            }

            while(priorityList.Count != 0 && priorityList[0].Key.CompareTo(cieling) <= 0)
                priorityList.RemoveAt(0);

            return dataList;
        }

        public int Count
        {
            get
            {
                return priorityList.Count;
            }
        }
    }
}
