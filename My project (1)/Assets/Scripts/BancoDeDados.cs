using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BancoDeDados
{
    public Produto[] produtos = {};
    public int redValue = 72;

    public List<Produto> AlmostEmpty()
    {

        List<Produto> temp = new();
        foreach(var produto in produtos)
        {
            if(produto.nome == "Ninhotella" && produto.amount <= 360) temp.Add(produto);
            if(produto.nome == "Duo" && produto.amount <= 240) temp.Add(produto);
            if(produto.amount <= redValue) temp.Add(produto);
        }
        return temp;
    }

}

