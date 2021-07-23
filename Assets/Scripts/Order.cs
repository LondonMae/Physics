/* London Bielicke 
 * 6/28/2021
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    // instantiate array
    private int[] order;

    // constructor 
    public Order(int n, int mult)
    {
        order = new int[n * mult];
        InitOrder(n);
        Randomize(order.Length);
    }

    // init order of numbers 0-n repeated m times
    void InitOrder(int n)
    {
        for (int i = 0; i < order.Length; i++)
        {
            order[i] = (i % n);
        }
    }

    // randomize initialized array by dividing into sections
    public void Randomize(int n)
    {
        for (int i = 0; i < n; i+=3)
        {
            RandomizeSection(i,i+3);
        }
    }

    // Randomized section of array
    public void RandomizeSection(int start, int end)
    {
        for (int i = end - 1; i > start; i--)
        {
            int j = Random.Range(start, i + 1);
            int temp = order[i];
            order[i] = order[j];
            order[j] = temp;
        }
    }

    // return order array
    public int[] GetOrder()
    {
        return order;
    }
}
