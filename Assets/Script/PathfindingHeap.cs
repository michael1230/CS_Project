using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;//the array of the elements
    int currentItemCount;//the amount of elements
    public Heap(int maxHeapSize)//the constructor 
    {
        items = new T[maxHeapSize];
    }
    public void Add(T item)//a method to add element
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }
    public T RemoveFirst()//a method to remove the first element
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }
    public void UpdateItem(T item)//a method to update the element
    {
        SortUp(item);
    }
    public int Count//a getter
    {
        get
        {
            return currentItemCount;
        }
    }
    public bool Contains(T item)//a method to check if the item T exists in the heap
    {
        return Equals(items[item.HeapIndex], item);
    }
    void SortDown(T item)//a method to fix the array(tree) after we removed an element 
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;
            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
    void SortUp(T item)//a method to fix the array(tree) after we update an element 
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }
    void Swap(T itemA, T itemB)//a method to swap two elements  
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
