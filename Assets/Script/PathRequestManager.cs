using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
public class PathRequestManager : MonoBehaviour
{
    Queue<PathResult> results = new Queue<PathResult>();//the Queue of the results
    static PathRequestManager instance;
    APathfinding pathfinding;//the APathfinding object itself
    bool isProcessingPath;
    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<APathfinding>();
    }
    void Update()
    {
        if (results.Count > 0)//if we have results(we found a path) ready then
        {
            int itemsInQueue = results.Count;//save the amount of results
            lock (results)//only one thread at a time
            {
                for (int i = 0; i < itemsInQueue; i++)//go on all the results
                {
                    PathResult result = results.Dequeue();//Dequeue the first results
                    result.callback(result.path, result.success);//call the method callback with the result.path and result.success (OnPathFound in PatrolLog and GralandChase)
                }
            }
        }
    }
    public static void RequestPath(PathRequest request)//a method to create and start a thread to find a path
    {
        ThreadStart threadStart = delegate 
        {
            instance.pathfinding.FindPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();

    }
    public void FinishedProcessingPath(PathResult result)// a method to call when the path was found 
    {
        lock (results)//only one thread at a time
        {
            results.Enqueue(result);
        }
    }
}
public struct PathResult//a struck for the PathResult 
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}
public struct PathRequest//a struck for the PathRequest 
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;
    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
    {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
    }

}