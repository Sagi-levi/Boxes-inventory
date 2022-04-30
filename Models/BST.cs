using System;

namespace Models
{
    public delegate void SingleItemAction<T>(T item);
    public class BST<T> where T : IComparable<T>
    {
        private Node _root { get; set; }
        /// <summary>
        /// adding an object to the tree
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value)
        {
            if (_root == null)
            {
                _root = new Node(value);
                return;
            }

            Node tmp = _root;
            while (true)
            {
                if (value.CompareTo(tmp.data) < 0) //value < tmp.data
                {
                    if (tmp.left == null)//add value to left and return
                    {
                        tmp.left = new Node(value);
                        tmp.left.up = tmp;
                        break;
                    }
                    tmp = tmp.left;
                }
                else
                {
                    if (tmp.right == null)//add value to right and return
                    {
                        tmp.right = new Node(value);
                        tmp.right.up = tmp;
                        break;
                    }
                    tmp = tmp.right;
                }
            }
        }

        public void ScanInOrder(SingleItemAction<T> itemAction)
        {
            if (_root == null) return;
            ScanInOrder(_root, itemAction);
        }

        private void ScanInOrder(Node tmpRoot, SingleItemAction<T> itemAction) // O(n)
        {
            if (tmpRoot.left != null) ScanInOrder(tmpRoot.left, itemAction);
            itemAction(tmpRoot.data);
            if (tmpRoot.right != null) ScanInOrder(tmpRoot.right, itemAction);
        }
        /// <summary>
        /// delete an item from the tree by the conventions of deleting
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Delete(T data)
        {
            Node tmp = _root;
            if (tmp == null) return false;
            while (tmp.data.CompareTo(data) != 0) //find the wanted to remove node
            {
                if (tmp.data.CompareTo(data) > 0)
                {
                    tmp = tmp.left;
                }
                else
                {
                    tmp = tmp.right;
                }
                if (tmp == null) return false;
            }
            if (tmp.right == null && tmp.left == null) //leaf
            {
                if (tmp.up == null)    //removing root
                {
                    _root = null;
                    return true;
                }
                if (tmp.up.data.CompareTo(tmp.data) > 0)
                {
                    tmp.up.left = null;
                }
                else
                {
                    tmp.up.right = null;
                }
                tmp.up = null;
            }
            if (tmp.right == null && tmp.left != null) //one side to handle
            {
                if (tmp.up == null)    //removing root
                {
                    _root = tmp.left;
                    _root.up = null;
                    return true;
                }
                if (tmp.up.data.CompareTo(tmp.data) > 0)
                {
                    tmp.up.left = tmp.left;
                    tmp.left.up = tmp.up;
                }
                else
                {
                    tmp.up.right = tmp.left;
                    tmp.left.up = tmp.up;
                }

            }
            if (tmp.right != null && tmp.left == null) //one side to handle
            {
                if (tmp.up == null)    //removing root
                {
                    _root = tmp.right;
                    _root.up = null;
                    return true;
                }
                if (tmp.up.data.CompareTo(tmp.data) > 0)
                {
                    tmp.up.left = tmp.right;
                    tmp.right.up = tmp.up;
                }
                else
                {
                    tmp.up.right = tmp.right;
                    tmp.right.up = tmp.up;
                }

            }
            if (tmp.right != null && tmp.left != null) //Two side to handle
            {                
                Node ToBeReplace = tmp;
                Node replacer;
                tmp = tmp.right;
                while (tmp.left != null)
                {
                    tmp = tmp.left;
                }
                replacer = tmp;
                if (replacer.right == null)
                {
                    ToBeReplace.data = replacer.data;
                    replacer.up.left = null;
                }
                else
                {
                    ToBeReplace.data = replacer.data;
                    replacer.up.left = replacer.right;
                    replacer.right.up = replacer.up;
                    replacer.right = null;
                    replacer.up = null;
                }
            }
            return true;
        }
        /// <summary>
        /// gets the deepest path of the tree
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int PublicGetDepth()
        {
            return GetDepth(_root);
        }

        private int GetDepth(Node root)
        {
            if (root == null)
            {
                return 0;
            }
            else
            {
                int ldepth = GetDepth(root.left);
                int rdepth = GetDepth(root.right);
                if (rdepth > ldepth)
                {
                    return rdepth + 1;
                }
                else return ldepth + 1;
            }
        }


        //public bool SearchRecursive(T itemToSearch, out T foundItem)
        //{
        //    foundItem = default(T);
        //    if (root == null) return false;
        //    Node tmp = root;
        //    return SearchRecursive(itemToSearch, out foundItem, tmp);
        //}
        //private bool SearchRecursive(T itemToSearch, out T foundItem, Node tmp)
        //{
        //    foundItem = default(T);
        //    if (tmp == null) return false;
        //    else
        //    {
        //        if (tmp.data.CompareTo(itemToSearch) > 0) return SearchRecursive(itemToSearch, out foundItem, tmp.left);
        //        else if (tmp.data.CompareTo(itemToSearch) < 0) return SearchRecursive(itemToSearch, out foundItem, tmp.right);
        //        else
        //        {
        //            foundItem = tmp.data;
        //            return true;
        //        }
        //    }
        //}

        //private bool SearchEqualOrBigger(T itemToSearch, out T foundItem,Node tmp, T closestNow)
        //{
        //    foundItem = default(T);
        //    if (root == null) return false;
        //    while (true)
        //    {
        //        if (tmp.data.CompareTo(itemToSearch) > 0)
        //        {
        //            closestNow = tmp.data;
        //            tmp = tmp.left;
        //        }
        //        else if (tmp.data.CompareTo(itemToSearch) < 0)
        //        {
        //            tmp = tmp.right;
        //        }
        //        else
        //        {
        //            foundItem = tmp.data;
        //            return true;
        //        }
        //    }
        //}
        public bool SearchEqualOrBigger(T itemToSearch, out T foundItem)//allways return something, the specific item we wanted or bigger(null if there is nothing big enough)
        {
            foundItem = default(T);
            if (_root == null) return false;
            Node tmp = _root;
            return SearchEqualOrBigger(itemToSearch, out foundItem, tmp, default(T));
        }
        private bool SearchEqualOrBigger(T itemToSearch, out T foundItem, Node tmp, T closestNow) 
        {
            foundItem = closestNow;
            if (tmp == null) return true;
            else
            {
                if (tmp.data.CompareTo(itemToSearch) > 0)
                {
                    closestNow = tmp.data;
                    return SearchEqualOrBigger(itemToSearch, out foundItem, tmp.left, closestNow);
                }

                else if (tmp.data.CompareTo(itemToSearch) < 0)
                {
                    return SearchEqualOrBigger(itemToSearch, out foundItem, tmp.right, closestNow);
                }
                else
                {
                    foundItem = tmp.data;
                    return true;
                }
            }
        }


        public bool SearchNextBigger(T itemToSearch, out T foundItem)//returns the next bigger value by the value inserted
        {
            foundItem = default(T);
            if (_root == null) return false;
            Node tmp = _root;
            return SearchNextBigger(itemToSearch, out foundItem, tmp, default(T));
        }
        private bool SearchNextBigger(T itemToSearch, out T foundItem, Node tmp, T closestNow) 
        {
            foundItem = closestNow;
            if (tmp == null) return true;
            else
            {
                if (tmp.data.CompareTo(itemToSearch) > 0)
                {
                    closestNow = tmp.data;
                    return SearchNextBigger(itemToSearch, out foundItem, tmp.left, closestNow);
                }

                else if (tmp.data.CompareTo(itemToSearch) < 0)
                {
                    return SearchNextBigger(itemToSearch, out foundItem, tmp.right, closestNow);
                }
                else
                {
                    if (tmp.right!=null)
                    {
                        tmp = tmp.right;
                        while (tmp.left != null)
                            tmp = tmp.left;
                        foundItem = tmp.data;
                    }
                    return true;
                }
            }
        }
        public bool IsDepthIsOne()
        {
            return (_root.left == null && _root.right == null);  //more usefull for preformence then check if depth is one
        }
        /// <summary>
        /// search if an item in the tree exist
        /// </summary>
        /// <param name="itemToSearch"></param>
        /// <param name="foundItem"></param>
        /// <returns></returns>
        public bool Search(T itemToSearch, out T foundItem)
        {
            foundItem = default(T);
            if (_root == null) return false;

            Node tmp = _root;
            while (true)
            {
                if (itemToSearch.CompareTo(tmp.data) < 0) 
                {
                    tmp = tmp.left;
                    if (tmp == null)
                    {
                        return false;
                    }
                    if (itemToSearch.CompareTo(tmp.data) == 0)
                    {
                        foundItem = tmp.data;
                        return true;
                    }
                }
                else
                {
                    if (itemToSearch.CompareTo(tmp.data) == 0)
                    {
                        foundItem = tmp.data;
                        return true;
                    }
                    tmp = tmp.right;
                    if (tmp == null)
                    {
                        return false;
                    }
                }
            }
        }

        public class Node
        {
            public T data;
            public Node up;
            public Node left;
            public Node right;

            public Node(T data)
            {
                this.data = data;
                left = right = null;
            }
        }
    }
}
