using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class Container
{
    private class Node
    {
        public Node Next;
        public Node Prev;
        public bool Value;

        public Node(Node prev)
        {
            var randomGen = new System.Random();
            Value = randomGen.Next(2) < 1;
            Prev = prev;
        }
    }

    private Node current;

    public Container(int count = 0)
    {
        if (count < 1)
        {
            var randomGen = new System.Random(DateTime.Now.Millisecond);
            count = randomGen.Next(1, 9999); //Could be up to Int32.MaxValue, reduced for sake of test memory
        }

        Node prev = null;
        for (int i = 0; i < count; i++)
        {
            var currentNode = new Node(prev);
            if (prev != null)
            {
                prev.Next = currentNode;
            }
            if (current == null)
            {
                current = currentNode;
            }
            prev = currentNode;
        }
        prev.Next = current;
        current.Prev = prev;
    }

    public bool Value
    {
        get { return current.Value; }
        set { current.Value = value; }
    }

    public void MoveForward()
    {
        current = current.Next;
    }

    public void MoveBackward()
    {
        current = current.Prev;
    }
}


public class ContainerEditor : EditorWindow
{
    public const int MAX_NUM = 50; // maximum UIs
    public const float SCROLL_OFF_SET = 50; // the offset from which we reset the scroll
    public const int UPDATE_STEP = 1; // the update speed when scroll up/down

    private Container container;

    private LinkedList<Tuple<int, bool>> cacheData; // the current data set to display, use linkedlist since it's fit
    private int totalCount; // number of elements in the container
    private int counter; // track the position of the "current" node in container

    [MenuItem("TaskTwo/Container Window")]
    public static void ShowWindow()
    {
        GetWindow<ContainerEditor>("Container Window");
    }

    private void OnEnable()
    {
        // create a new container with random node count as the default data source
        totalCount = UnityEngine.Random.Range(1, 9999);
        counter = 0;
        container = new Container(totalCount);
        cacheData = new LinkedList<Tuple<int, bool>>();
        //minSize = new Vector2(300, 200); // set window minimum size, we need some space

        for (int i = 0; i < MAX_NUM && i < totalCount; i++) // display maximum MAX_NUM nodes at a time for performance
        {
            bool nodeValue = container.Value;
            cacheData.AddLast(new Tuple<int, bool>(i, nodeValue));
            MoveForward();
        }
    }

    // wrapper function, when we move the container, we update the counter as well
    private void MoveForward()
    {
        counter++;
        if (counter >= totalCount)
        {
            counter -= totalCount;
        }
        container.MoveForward();
    }

    // wrapper function, when we move the container, we update the counter as well
    private void MoveBackward()
    {
        counter--;
        if (counter < 0)
        {
            counter = totalCount;
        }
        container.MoveBackward();
    }

    private void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        GUILayout.Label("total = " + totalCount + " Current Node: " + counter); // debug label
        
        GUILayout.BeginVertical();
        Event e = Event.current;
        if (e.type == EventType.ScrollWheel && e.delta.y > 0)
        {
            //Debug.Log("Scrolling down");
            // scroll down
            // do 5 times, remove first and add the new element to the last pos of the list
            for (int i = 0; i < UPDATE_STEP; i++)
            {
                cacheData.RemoveFirst();
                cacheData.AddLast(new Tuple<int, bool>(counter, container.Value));
                MoveForward();
            }
        }
        else if (e.type == EventType.ScrollWheel && e.delta.y < 0)
        {
            //Debug.Log("Scrolling up");
            // scroll up, remove last and add the new element to the first pos of the list
            for (int i = 0; i < UPDATE_STEP; i++)
            {
                cacheData.RemoveLast();
                cacheData.AddFirst(new Tuple<int, bool>(counter, container.Value));
                MoveBackward();
            }
        }
        GUILayout.Space(10);

        // draw data
        foreach (var data in cacheData)
        {
            GUILayout.Toggle(data.Item2, data.Item1.ToString());
        }

        GUILayout.EndVertical();
    }
}