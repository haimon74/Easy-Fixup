/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections;
using System.Text;

namespace ezFixUp.Classes
{
    public class BitVector
    {
        private byte[] m_data = new byte[0];
        private int m_maxoffset = -1;

        public BitVector()
        {
        }

        public BitVector(byte[] data)
        {
            m_data = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                m_data[i] = data[i];
            }
            m_maxoffset = (m_data.Length*8) - 1;
        }

        public BitVector(long val, int length)
        {
            AddData(val, length);
        }

        public void AddAscii(string val)
        {
            for (int i = 0; i < val.Length; i++)
            {
                AddData(val[i], 8);
            }
        }

        public void AddData(long val, int length)
        {
            int offset = m_maxoffset + 1;
            for (int i = 0; i < length; i++)
            {
                Set(offset + i, (val & (long) (1 << (length - i - 1))) != 0);
            }
        }

        public byte[] GetByteArray()
        {
            byte[] result = new byte[m_data.Length];
            for (int i = 0; i < m_data.Length; i++)
                result[i] = (byte) m_data[i];
            return result;
        }

        public int Length
        {
            get { return m_maxoffset + 1; }
        }

        public void Set(int offset, bool val)
        {
            int byteoffset = offset/8;
            int bitoffset = offset%8;

            if (byteoffset >= m_data.Length)
            {
                byte[] data = new byte[byteoffset + 1];
                for (int i = 0; i < m_data.Length; i++)
                {
                    data[i] = m_data[i];
                }
                m_data = data;
                m_maxoffset = offset;
            }
            else if (offset > m_maxoffset)
            {
                m_maxoffset = offset;
            }

            if (val)
                m_data[byteoffset] |= (byte) (1 << (7 - bitoffset));
            else
                m_data[byteoffset] &= (byte) (0xff - (1 << (7 - bitoffset)));
        }

        public bool Get(int offset)
        {
            if (offset > m_maxoffset)
            {
                throw new Exception("OutOfBound offset " + offset);
            }
            int byteoffset = offset/8;
            int bitoffset = offset%8;

            if (byteoffset >= m_data.Length)
            {
                throw new Exception("OutOfBound offset " + offset);
            }

            return (m_data[byteoffset] & (1 << (7 - bitoffset))) != 0;
        }

        public BitVector Range(int start, int length)
        {
            BitVector result = new BitVector();
            for (int i = start; i < (start + length); i++)
            {
                result.Set(i - start, Get(i));
            }
            return result;
        }

        public int LongestCommonPrefix(BitVector other)
        {
            int i = 0;
            while ((i <= other.m_maxoffset) && (i <= m_maxoffset))
            {
                if (other.Get(i) != Get(i))
                {
                    return i;
                }
                i++;
            }
            return i;
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= m_maxoffset; i++)
            {
                sb.Append((Get(i)) ? "1" : "0");
                if (i%8 == 7)
                    sb.Append(" ");
            }
            return sb.ToString();
        }
    }

    public class BitVectorTrie
    {
        public class Node
        {
            public ArrayList Children = null;
            public BitVector Key = null;
            public Object Data = null;
        }

        public Node Root = new Node();

        public void Add(BitVector key, object data)
        {
            Add(Root, key, data);
        }

        //
        // This Add method attach an object "data" to a node "n" using
        // the BitVector key as path.
        //
        private void Add(Node n, BitVector key, object data)
        {
            if (n.Key == null)
            {
                AddAsChildren(n, key, data);
                return;
            }

            //
            // First, calculate the longest common prefix for the key
            // and the BitVector stored in this node.
            //
            int longest = key.LongestCommonPrefix(n.Key);

            if (longest == n.Key.Length)
            {
                //
                // If the current node is a perfect prefix of the
                // key, then remove the prefix from the key, and
                // we continue our walk on the children.
                //
                key = key.Range(longest, key.Length - longest);
                AddAsChildren(n, key, data);
                return;
            }
            else
            {
                //
                // Here, n.Key and key share a common prefix. So we:
                //
                // - Create a new node with this common prefix
                //   held there,
                //
                // - make n.Key and a new node with key as
                // children of this new node.
                BitVector common = n.Key.Range(0, longest);

                Node c1 = new Node();
                c1.Key = n.Key.Range(longest, n.Key.Length - longest);
                c1.Data = n.Data;
                c1.Children = n.Children;

                Node c2 = new Node();
                c2.Key = key.Range(longest, key.Length - longest);
                c2.Data = data;

                n.Key = common;
                n.Data = null;
                n.Children = new ArrayList();
                n.Children.Add(c1);
                n.Children.Add(c2);

                return;
            }
        }

        //
        // The AddAsChildren() method create a new node with key
        // "key", attach a data "data" to it, and finally link it to
        // the node "n".
        //
        private void AddAsChildren(Node n, BitVector key, Object data)
        {
            //
            // If "n" has no children, just add a new one
            //
            if (n.Children == null)
            {
                n.Children = new ArrayList();
                Node nu = new Node();
                nu.Key = key;
                nu.Data = data;
                n.Children.Add(nu);
                return;
            }

            //
            // From here, the node n already has at least 1
            // children.


            // Check the one that has a common prefix with our key
            //(if there is none, the bestindex variable stays at -1).

            int bestindex = -1;
            int bestlength = 0;
            for (int i = 0; i < n.Children.Count; i++)
            {
                int b = ((Node) (n.Children[i])).Key.LongestCommonPrefix(key);

                if (b > bestlength)
                {
                    bestlength = b;
                    bestindex = i;
                }
            }

            //
            // The node n has no children that have a common prefix
            // with our key, so we create a new children node and
            // attach our data there.
            if (bestindex == -1)
            {
                Node c2 = new Node();
                c2.Key = key;
                c2.Data = data;
                n.Children.Add(c2);
                return;
            }
            else
            {
                // There is a children node that can hold our
                // data: continue our walk with this node.
                Add(((Node) n.Children[bestindex]), key, data);
                return;
            }
        }

        public object Get(BitVector key)
        {
            Node curnode = Root;
            while (curnode != null)
            {
                if (curnode.Children == null)
                    return null;

                // Get the best fitting index
                int bestindex = -1;
                int bestlength = 0;
                for (int i = 0; i < curnode.Children.Count; i++)
                {
                    int b = ((Node) (curnode.Children[i])).Key.LongestCommonPrefix(key);
                    if (b > bestlength)
                    {
                        bestlength = b;
                        bestindex = i;
                    }
                }

                if (bestindex != -1)
                {
                    key = key.Range(bestlength, key.Length - bestlength);
                    curnode = ((Node) curnode.Children[bestindex]);

                    if (key.Length == 0)
                        return curnode.Data;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        //
        // Returns the object held in the node that best matches our
        // key.
        //
        public object GetBest(BitVector key)
        {
            Node curnode = Root;
            while (curnode != null)
            {
                if (curnode.Children == null)
                    return curnode.Data;

                // Get the best fitting index
                int bestindex = -1;
                int bestlength = 0;
                for (int i = 0; i < curnode.Children.Count; i++)
                {
                    int b = ((Node) (curnode.Children[i])).Key.LongestCommonPrefix(key);
                    if (b > bestlength)
                    {
                        bestlength = b;
                        bestindex = i;
                    }
                }

                if (bestindex != -1)
                {
                    key = key.Range(bestlength, key.Length - bestlength);
                    curnode = ((Node) curnode.Children[bestindex]);

                    if (key.Length == 0)
                        return curnode.Data;
                }
                else
                {
                    return curnode.Data;
                }
            }

            return null;
        }
    }

    public class BitVectorReader
    {
        private BitVector m_data;
        private int m_offset;

        public BitVectorReader(BitVector v)
        {
            m_data = v;
            m_offset = 0;
        }

        public bool HasMoreData()
        {
            if (m_offset < m_data.Length)
                return true;
            return false;
        }

        public Int32 ReadInt32()
        {
            Int32 result = 0;
            for (int max = m_offset + 32, offset = 0;
                 (m_offset < max) && (m_offset < m_data.Length);
                 m_offset++, offset++)
            {
                if (m_data.Get(m_offset))
                    result |= (1 << (31 - offset));
            }
            return result;
        }


        public Int16 ReadInt16()
        {
            int result = 0;
            for (int max = m_offset + 16, offset = 0;
                 (m_offset < max) && (m_offset < m_data.Length);
                 m_offset++, offset++)
            {
                if (m_data.Get(m_offset))
                    result |= (1 << (15 - offset));
            }
            return (Int16) result;
        }


        public byte ReadByte()
        {
            byte result = 0;
            for (int max = m_offset + 8, offset = 0;
                 (m_offset < max) && (m_offset < m_data.Length);
                 m_offset++, offset++)
            {
                if (m_data.Get(m_offset))
                    result |= (byte) (1 << (7 - offset));
            }
            return result;
        }

        public String ReadAscii(int length)
        {
            StringBuilder buffer = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                buffer.Append((char) ReadByte());
            }
            return buffer.ToString();
        }
    }
}