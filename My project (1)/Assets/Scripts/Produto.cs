using System;
using UnityEngine;

[System.Serializable]
public class Produto
{
    public int id, amount;
    public string type, nome;

    public void MaiorQZero(int value)
    {
        amount = (int)Mathf.Clamp(amount+value, 0, Mathf.Infinity);
    }

}