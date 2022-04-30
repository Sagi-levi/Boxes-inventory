using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DoubleLinkedList<T>
    {
        public Node Start = null;
        public Node End = null;
        private int count;

        public int Count { get { return count; } }

        //Adds value to the beggining of the list
        public void AddFirst(T value) //O(1)
        {
            Node newNode = new Node(value);
            if (Start != null)
                Start.prev = newNode;
            newNode.next = Start;
            Start = newNode;
            if (Start.next == null)
            {
                End = newNode;
            }
            count++;
        }

        //removes value from the beginnging
        public bool RemoveFirst(out T removedValue) // O(1)
        {
            if (Start == null)
            {
                removedValue = default(T);
                return false;
            }
            count--;
            removedValue = Start.Data;
            Start = Start.next;
            if (Start == null)
            {
                End = null;
                return true;
            }
            Start.prev = null;
            return true;
        }

        //Adds value to the last of the list
        public void AddLast(T value) //O(1)
        {
            if (Start == null)
            {
                AddFirst(value);
                return;
            }
            Node newNode = new Node(value);
            End.next = newNode;
            End.next.prev = End;
            End = End.next;
            count++;
        }

        //removes value from the last
        public bool RemoveLast(out T removedValue)//O(1)
        {
            if (Start == null)
            {
                removedValue = default(T);
                return false;
            }
            removedValue = End.Data;
            End = End.prev;
            count--;
            if (End == null)
            {
                Start = null;
                return true;
            }
            End.next = null;
            return true;
        }

        public bool GetValueAt(int position, out T value)
        {
            value = default(T);

            if (position >= count) return false;

            Node tmp = Start;
            for (int i = 0; i < position; i++) tmp = tmp.next;
            value = tmp.Data;
            return true;
        }//position -zero based position // O(n)

        public bool AddAt(int position, T value) //position -zero based position // O(n)
        {
            //not allowing to add with this func instead of start addFirst or addLast
            if (position <= 1||position>=count) throw new NullReferenceException("position must be positive or zero");
            
            //no list
            if (Start == null) return false;

            Node tmp = Start;
            for (int i = 1; tmp != null && i < position; i++) tmp = tmp.next;

            Node newNode = new Node(value);

            newNode.prev = tmp;
            newNode.next = tmp.next;
            tmp.next.prev = newNode;
            tmp.next = newNode;
            count++;
            return true;
        }
        /// <summary>
        /// removes on object by his node in o(1)
        /// </summary>
        /// <param name="node"></param>
        public void RemoveByNode(Node node)
        {
            if (node.Data.Equals(End.Data))
            {
                RemoveLast(out _);
                return;
            }
            if (node.Data.Equals(Start.Data))
            {
                RemoveFirst(out _);
                return;
            }
            node.next.prev = node.prev;
            node.prev.next = node.next;
            count--;
        }
        /// <summary>
        /// reposition of a node in o(1) 
        /// </summary>
        /// <param name="node"></param>
        public void RePositeToEnd(Node node)
        {
            if (node.Data.Equals(End.Data))
            {
                return;
            }
            if (node.Data.Equals(Start.Data))
            {
                Start = node.next;
                node.next.prev = null;
                AddLast(node.Data);
            }
            else
            {
                node.next.prev = node.prev;
                node.prev.next = node.next;
                AddLast(node.Data); 
            }
        }

        public bool IsEmpty()
        {
            return (Start == null);
        }
        public override string ToString()
        {
            Node tmp = Start;
            StringBuilder sb = new StringBuilder();
            sb.Append("these are all the values: ");

            while (tmp != null)
            {
                sb.Append($"{tmp.Data} ");
                tmp = tmp.next;
            }

            return sb.ToString();
        }

        public class Node
        {
            public T Data { get; set; }
            public Node next;
            public Node prev;

            public Node(T data)
            {
                this.Data = data;
                next = null;
                prev = null;
            }
        }
    }
}
