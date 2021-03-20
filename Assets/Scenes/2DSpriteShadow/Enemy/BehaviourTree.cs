using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.BT
{
    //public class BehaviourTree : MonoBehaviour
    //{
    //    // 하나 초이스
    //    private Selector root = new Selector();
        
    //    // 다 돈다.
    //    private Sequence seq = new Sequence();

    //    private void Start()
    //    {
    //        root.AddChild(new BTCondition(() => { Debug.Log("Selector"); return false; ; }));
    //        root.AddChild(seq);
            


    //        seq.AddChild(new BTCondition(() => {
    //            Debug.Log("여러가지 조건식..");
    //            return false;
    //        }));
    //        seq.AddChild(new BTAction(() => { Debug.Log("Action!"); return true; }));
            
    //    }

    //    private void Update()
    //    {
    //        root.OnUpdate();
    //    }
    //}

    /// <summary>
    /// 조건식 (Selector, Sequence에 따라 결과가 달라짐) 노드
    /// </summary>
    public class BTCondition : Node
    {
        public System.Func<bool> action;
        public BTCondition() { }
        public BTCondition(System.Func<bool> action)
        {
            this.action = action;
        }


        public override bool OnUpdate()
        {
             return action.Invoke();
        }
    }

    /// <summary>
    /// 조건식이 체크된 다음에 실질적인 행동을 시켜주는 노드
    /// </summary>
    public class BTAction : Node
    {
        public System.Func<bool> action;
        public BTAction() { }
        public BTAction(System.Func<bool> action)
        {
            this.action = action;
        }

        // Only return True
        public override bool OnUpdate()
        {
            return action.Invoke();
        }
    }

    public abstract class Node
    {
        public Stack<Node> nodes = new Stack<Node>();
        public int StackCount
        {
            get
            {
                if (nodes == null)
                    return 0;
                return nodes.Count;
            }
        }

        public void AddChild(Node node)
        {
            nodes.Push(node);
        }

        public abstract bool OnUpdate();
    }

    /// <summary>
    /// 하나라도 TRUE면 결과도 TRUE (TRUE시 즉시 리턴)
    /// </summary>
    public class Selector : Node
    {
        public override bool OnUpdate()
        {
            int curIndex = 0;
            while (curIndex < StackCount)
            {
                var child = nodes.ToArray()[curIndex++];

                if (child.OnUpdate() == true)
                    return false;
            }
            
            return true;
        }
    }

    /// <summary>
    /// 하나라도 FALSE면 결과도 FALSE (FALSE시 즉시 리턴)
    /// </summary>
    public class Sequence : Node
    {
        public override bool OnUpdate()
        {
            int curIndex = 0;
            while (curIndex < StackCount)
            {
                var child = nodes.ToArray()[curIndex++];

                if (child.OnUpdate() == false)
                    return false;
            }

            return true;
        }
    }
}