using DIY.Foundation;
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

        public UITree(params UINode[] nodes )
        {
            _tree = new Dictionary<string, UINode>();
            //先绑定第一层子node
            foreach (UINode uiNode in nodes)
            {
                this[uiNode.name] = uiNode;
            }
        }

        public void Init(Transform root) {
            UINode[] uiNodes_origin = new UINode[] { };
            foreach (KeyValuePair<string,UINode> keyValue in _tree)
            {
                uiNodes_origin[uiNodes_origin.Length] = keyValue.Value;
            }

            foreach (UINode uiNode_origin in uiNodes_origin)
            {
                uiNode_origin.Init(root);
                UINode[] uiNode_childs = uiNode_origin.InitChilds();
                if (SafeUtil.CheckLegal_Array(uiNode_childs))
                {
                    foreach (UINode uiNode_child in uiNode_childs)
                    {
                        this[uiNode_child.name] = uiNode_child;
                    }
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

        public void Init(Transform parentTrans) { 
            transform = parentTrans.Find(this.name);
        }

        public UINode[] InitChilds() {
            UINode[] uiNodes = null;
            if (childs != null && childs.Count > 0)
            {
                uiNodes = new UINode[] { };
                //对子物体进行初始化
                foreach (KeyValuePair<string, UINode> item in childs)
                {
                    item.Value.Init(transform);
                    UINode[] uiNodesChilds = item.Value.InitChilds();
                    if (uiNodesChilds!=null && uiNodesChilds.Length > 0)
                    {
                        foreach (UINode node in uiNodesChilds)
                        {
                            uiNodes[uiNodes.Length] = node;
                        }
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



