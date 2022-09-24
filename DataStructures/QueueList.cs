using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures
{
    public class QueueList<T> : IEnumerable<T>
    {
        private QueueListNode<T> first;
        private QueueListNode<T> last;
        private int count;

        public QueueListNode<T> First => first;
        public QueueListNode<T> Last => last;
        public int Count => count;

        public void AddFirst(T value) => PrivateAddFirst(new QueueListNode<T>(value));

        public void AddFirst(QueueListNode<T> newNode)
        {
            ValidateNewNode(newNode);

            PrivateAddFirst(newNode);
        }

        private void PrivateAddFirst(QueueListNode<T> newNode)
        {
            newNode.next = first;
            if (last == null) last = newNode;
            else first.prev = newNode;

            first = newNode;

            newNode.list = this;
            count++;
        }

        public void AddLast(T value) => PrivateAddLast(new QueueListNode<T>(value));

        public void AddLast(QueueListNode<T> newNode)
        {
            ValidateNewNode(newNode);

            PrivateAddLast(newNode);
        }

        private void PrivateAddLast(QueueListNode<T> newNode)
        {
            if (first == null)
            {
                PrivateAddFirst(newNode);
                return;
            }

            last.next = newNode;
            newNode.prev = last;
            last = last.next;

            newNode.list = this;
            count++;
        }

        public bool Remove(T value)
        {
            QueueListNode<T> node = Find(value);
            if (node != null)
            {
                PrivateRemoveNode(node);
                return true;
            }
            return false;
        }

        private QueueListNode<T> Find(T value)
        {
            QueueListNode<T> currentNode = first;
            while (currentNode != null)
            {
                if (currentNode.Value.Equals(value)) return currentNode;
                currentNode = currentNode.next;
            }
            return null;
        }

        public void Remove(QueueListNode<T> node)
        {
            ValidateExistsNode(node);
            PrivateRemoveNode(node);
        }

        public void RemoveFirst()
        {
            if (first == null) { throw new InvalidOperationException("The QueueList is empty"); }
            PrivateRemoveNode(first);
        }

        public void RemoveLast()
        {
            if (first == null) { throw new InvalidOperationException("The QueueList is empty"); }
            PrivateRemoveNode(last);
        }

        private void PrivateRemoveNode(QueueListNode<T> node)
        {
            if (count == 1) { first = last = null; }
            else if (first == node)
            {
                first = first.next;
                first.prev = null;
            }
            else if (last == node)
            {
                last = last.prev;
                last.next = null;
            }
            else
            {
                node.next.prev = node.prev;
                node.prev.next = node.next;
            }

            node.next = node.prev = null;
            node.list = null;
            count--;
        }

        private void ValidateNewNode(QueueListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("Node cannot be null.");
            }

            if (node.list != null)
            {
                throw new ArgumentNullException("The QueueList node already belongs to a QueueList.");
            }
        }


        private void ValidateExistsNode(QueueListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("Node cannot be null.");
            }

            if (node.list != this)
            {
                throw new InvalidOperationException("The QueueList node does not belong to current QueueList.");
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            QueueListNode<T> currentNode = first;
            while (currentNode != null)
            {
                yield return currentNode.Value;
                currentNode = currentNode.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
