using UnityEngine;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using System;

public class LimitedList<T> : List<T>
{
    private readonly int max_element_count;

    public LimitedList(int _max_element_count)
    {
        max_element_count = _max_element_count;
    }

    public void Add(T _item)
    {
        if (this.Count < max_element_count)
        {
            base.Add(_item);
        }
        else
        {
            throw new InvalidOperationException("This List Doesn't Accept More Elements. Remove Old Elements or Make a New List");
        }
    }
}
