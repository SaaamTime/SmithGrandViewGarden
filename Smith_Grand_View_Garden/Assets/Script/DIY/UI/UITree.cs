using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIY.UI {

    public class UITree {
        private Dictionary<string, UINode> _tree;
        public UINode this[string name] {
            set {
                _tree.Add(name, value);
            }
            get {
                return _tree[name];
            }
        }

        public UITree(Transform root, params UINode[] nodes )
        {
            _tree = new Dictionary<string, UINode>();
            foreach (UINode uiNode in nodes)
            {
                //这里仅初始化第一层，剩下的分支自行初始化
                UINode[] uiNodes = uiNode.Init(root);
                foreach (UINode uiNode_temp in uiNodes)
                {
                    this[uiNode_temp.name] = uiNode_temp;
                }
            }
        }
    }

    public class UINode
    {
        public string name;
        public UINode parent;
        public Transform transform;
        public Dictionary<string,UINode> childs;
        public Component target;
        //只绑定本叶级的组件，剩余的子叶级不再处理
        public UINode(string name,params UINode[] childs)
        {
            this.name = name;
            if (childs != null && childs.Length > 0)
            {
                this.childs = new Dictionary<string, UINode>();
                foreach (UINode item in childs)
                {
                    this.childs.Add(item.name, item);
                }
            }
        }
        public UINode[] Init(Transform parentTrans) {
            transform = parentTrans.Find(this.name);
            UINode[] uiNodes = new UINode[] { this };
            if (childs != null && childs.Count > 0)
            {
                //对子物体进行初始化
                foreach (KeyValuePair<string, UINode> item in childs)
                {
                    foreach (UINode node in item.Value.Init(transform))
                    {
                        uiNodes[uiNodes.Length-1] =node;
                    }
                }

            }
            //Debug.Log("UINode初始化完毕" + name);
            return uiNodes;
        }
        public void BindEvent_Button(Action action) {
            if (this.target == null)
            {
                this.target = UIUtil.Button.Bind(transform, action);
            }
            else {
                UIUtil.Button.BindEvent((Button)this.target, action);
            }
        }

        public void AutoBindImage() {
            if (this.target==null)
            {
                this.target = UIUtil.Image.Bind(transform);
            }
        }

        public void ChangeSpriteColor(Color color) {
            AutoBindImage();
            ((Image)this.target).color = color;
        }
    }
}



