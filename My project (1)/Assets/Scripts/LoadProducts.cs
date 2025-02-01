using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadProducts : MonoBehaviour
{

    int redValue;
    public GameObject prodPrefab, idBox, typeBox, nomeBox, quantBox;

    public RectTransform content;


    DatabaseJson json;

    private void Awake() {
        json = GetComponent<DatabaseJson>();
        redValue = json.banco.redValue;
    }
    public void PainelTamanho(GridLayoutGroup block, RectTransform content, int tamBanco)
    {
        float novaAltura = tamBanco  * block.cellSize.y;
        content.sizeDelta = new Vector2(content.sizeDelta.x, novaAltura);
    }
    public void PainelTamanho(GridLayoutGroup block, RectTransform content, int tamBanco, int offset)
    {
        float novaAltura = tamBanco  * block.cellSize.y + offset;
        content.sizeDelta = new Vector2(content.sizeDelta.x, novaAltura);
    }

    //Necessario para trabalhar com objetos complexos e realizar sorts
    class OrdenadorNome : IComparer<Produto>
    {
        public int Compare(Produto x, Produto y)
        {
            //Ordenando por ordem alfabética
            if(x.nome.Length > y.nome.Length)
            {
                for(int i = 0; i < y.nome.Length; i++)
                {
                    if(y.nome[i] > x.nome[i]) return -1;
                    else if(y.nome[i] < x.nome[i]) return 1;
                }
            }
            else
            {
                 for(int i = 0; i < x.nome.Length; i++)
                {
                    if(x.nome[i] > y.nome[i]) return 1;
                    else if(x.nome[i] < y.nome[i]) return -1;
                }
            }
            return 0;
        }
    }
    class OrdenadorQuantia : IComparer<Produto>
    {
        public int Compare(Produto x, Produto y)
        {
            return x.amount.CompareTo(y.amount);
        }
    }


    public void LoadList()
    {
        //Limpa lista
        for(int i = 0; i < idBox.transform.childCount; i++)
        {
            Destroy(idBox.transform.GetChild(i).gameObject);
            Destroy(typeBox.transform.GetChild(i).gameObject);
            Destroy(nomeBox.transform.GetChild(i).gameObject);
            Destroy(quantBox.transform.GetChild(i).gameObject);
        }

        //Ordena em ID e separa entre as categorias.
        List<Produto> produtos= new(json.banco.produtos);
        produtos.Sort(new OrdenadorNome());
        List<Produto> recheados = produtos.FindAll((produtos) => produtos.type == "Recheado");
        List<Produto> leite = produtos.FindAll((produtos) => produtos.type == "Leite");
        List<Produto> fruta = produtos.FindAll((produtos) => produtos.type == "Fruta");
        List<Produto> zero = produtos.FindAll((produtos) => produtos.type == "Zero");
        List<Produto> premium = produtos.FindAll((produtos) => produtos.type == "Premium");
        AddList(fruta);
        AddList(leite);
        AddList(recheados);
        AddList(zero);
        AddList(premium);
        PainelTamanho(idBox.GetComponent<GridLayoutGroup>(), content, json.banco.produtos.Length);
    }
    public void LoadList(GameObject nome, GameObject quantia, RectTransform content, GameObject textPrefab)
    {
        //Limpa lista
        for(int i = 0; i < nome.transform.childCount; i++)
        {
            Destroy(nome.transform.GetChild(i).gameObject);
            Destroy(quantia.transform.GetChild(i).gameObject);
        }

        //Ordena em ID e separa entre as categorias.
        List<Produto> produtos= json.banco.AlmostEmpty(redValue);
        produtos.Sort(new OrdenadorQuantia());
        foreach(var produto in produtos)
        {
            GameObject prodNome = Instantiate(textPrefab, nome.transform);
            prodNome.GetComponentInChildren<Text>().text = produto.nome;
            GameObject prodQuant = Instantiate(prodPrefab, quantia.transform);
            prodQuant.GetComponentInChildren<Text>().text = produto.amount.ToString();
        }
        
        PainelTamanho(nome.GetComponent<GridLayoutGroup>(), content, produtos.Count, 10);
    }


    void AddList(List<Produto> type)
    {
         foreach(var produto in type)
        {
            GameObject prodID = Instantiate(prodPrefab, idBox.transform);
            prodID.GetComponentInChildren<Text>().text = produto.id.ToString();
            GameObject prodNome = Instantiate(prodPrefab, nomeBox.transform);
            prodNome.GetComponentInChildren<Text>().text = produto.nome;
            GameObject prodType = Instantiate(prodPrefab, typeBox.transform);
            prodType.GetComponentInChildren<Text>().text = produto.type;
            GameObject prodQuant = Instantiate(prodPrefab, quantBox.transform);
            int caixas = produto.amount/24;
            if(caixas > 0) prodQuant.GetComponentInChildren<Text>().text = $"{caixas} cxs + {produto.amount%24} uni";
            else prodQuant.GetComponentInChildren<Text>().text = $"{produto.amount} uni";
            GameObject[] prodStatus = new GameObject[4]{prodID, prodNome, prodType, prodQuant};
            if(produto.amount <= redValue)
                foreach (var prod in prodStatus) prod.GetComponentInChildren<Text>().color = Color.red;

        }
    }
}
