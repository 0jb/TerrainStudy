using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Test : MonoBehaviour
{
    int[] array;
    int index = 0;
    private Thread t;

    // Use this for initialization
    void Start()
    {
        MetodoEditor();
        t.Start();
    }

    private void MetodoEditor()
    {
        t = new System.Threading.Thread(ThreadMethod);
        array = new int[] { 1, 2, 1, 2, 1, 2 };
    }

    private void ThreadMethod()
    {
        if (array != null && array.Length > 0)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Debug.Log("A " + index);//pre delay
                System.Threading.Thread.Sleep(1000);
                Debug.Log("B " + index);//pos delay        
                index++;
            }

        }
    }
}