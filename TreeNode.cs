using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuayThaiTraining
{
    class TreeNode : IEnumerable<TreeNode>
    {
        private readonly Dictionary<string, TreeNode> _children =
                                        new Dictionary<string, TreeNode>();

        public readonly string root;
        public TreeNode Parent { get; private set; }

        public TreeNode(string root)
        {
            this.root = root;
        }

        public TreeNode GetChild(string root)
        {
            return this._children[root];
        }

        public void Add(TreeNode item)
        {
            if (item.Parent != null)
            {
                item.Parent._children.Remove(item.root);
            }

            item.Parent = this;
            this._children.Add(item.root, item);

        }

        public IEnumerator<TreeNode> GetEnumerator()
        {
            return this._children.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count
        {
            get { return this._children.Count; }
        }
    }

}
