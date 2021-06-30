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
    }

    // init order of numbers 0-n repeated m times
    void InitOrder(int n)
    {
        for (int i = 0; i < order.Length; i++)
        {
            order[i] = (i % n);
        }
    }

    // Randomized initialized array
    public void Randomize()
    {
        for (int i = order.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
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
