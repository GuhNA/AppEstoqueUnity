using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BancoDeDados
{
    public Produto[] produtos = {};

    public int redValue;

    public List<Produto> AlmostEmpty(int redValue)
    {
        List<Produto> temp = new List<Produto>();
        foreach(var produto in produtos)
        {
            if(produto.amount <= redValue) temp.Add(produto);
        }
        return temp;
    }
}

