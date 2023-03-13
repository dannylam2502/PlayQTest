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
    public const int MAX_NUM = 100; // maximum UIs
    public const float SCROLL_OFF_SET = 50; // the offset from which we reset the scroll
    public const int UPDATE_STEP = 5; // the update speed when scroll up/down

    private Container container;
    private Vector2 scrollPosition; // the current scrollPosition
    private Vector2 oldScrollPosition; // the last frame scrollPosition, use this to detect scroll up or down activity

    private LinkedList<Tuple<int, bool>> cacheData;
    private int totalCount;
    private int counter;

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
        minSize = new Vector2(300, 400);

        for (int i = 0; i < MAX_NUM && i < totalCount; i++) // display maximum MAX_NuM nodes at a time for performance
        {
            bool nodeValue = container.Value;
            cacheData.AddLast(new Tuple<int, bool>(i, nodeValue));
            MoveForward();
        }
    }

    private void MoveForward()
    {
        counter++;
        if (counter >= totalCount)
        {
            counter -= totalCount;
        }
        container.MoveForward();
    }

    private void MoveBackward()
    {
        counter--;
        if (counter < 0)
        {
            counter = totalCount;
        }
        container.MoveBackward();
    }

    private void OnGUI()
    {
        GUILayout.Label("total = " + totalCount + " Current Node: " + counter);
        // scroll view to display the nodes
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        // use the position to calculate the offset for the infinite scrollview
        // if we reach a certain threshold, reset its position
        if (scrollPosition.y > position.height - SCROLL_OFF_SET)
        {
            scrollPosition.y = SCROLL_OFF_SET;
            oldScrollPosition = scrollPosition; // update it to follow so no update on this frame
        }
        else if (scrollPosition.y < SCROLL_OFF_SET)
        {
            scrollPosition.y = position.height - SCROLL_OFF_SET;
            oldScrollPosition = scrollPosition; // update it to follow so no update on this frame
        }

        if (oldScrollPosition != scrollPosition)
        {
            Debug.LogFormat("Old = {0}, New = {1}", oldScrollPosition, scrollPosition);
            if (oldScrollPosition.y < scrollPosition.y)
            {
                // scroll down
                // do 5 times
                for (int i = 0; i < 5; i++)
                {
                    cacheData.RemoveFirst();
                    cacheData.AddLast(new Tuple<int, bool>(counter, container.Value));
                    MoveForward();
                }
            }
            else
            {
                // scroll up
                for (int i = 0; i < 5; i++)
                {
                    cacheData.RemoveLast();
                    cacheData.AddFirst(new Tuple<int, bool>(counter, container.Value));
                    MoveBackward();
                }
            }
        }

        GUILayout.Space(10);

        foreach (var data in cacheData)
        {
            GUILayout.Toggle(data.Item2, data.Item1.ToString());
        }

        oldScrollPosition = scrollPosition;
        GUILayout.EndScrollView();
    }
}