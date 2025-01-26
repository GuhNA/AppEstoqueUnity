using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadProducts : MonoBehaviour
{
    GridLayoutGroup idBlock;

    public GameObject prodPrefab, idBox, typeBox, nomeBox, quantBox;
    public RectTransform content;

    DatabaseJson json;

    private void Awake() {
        json = GetComponent<DatabaseJson>();
        idBlock = idBox.GetComponent<GridLayoutGroup>();
    }

    public void PainelTamanho()
    {
        int linhas = idBlock.transform.childCount / idBlock.constraintCount;
        float alturaCelula = idBlock.cellSize.y + idBlock.spacing.y;
        float novaAltura = (linhas-1) * alturaCelula;
        print(novaAltura);
        content.sizeDelta = new Vector2(content.sizeDelta.x, novaAltura);
    }


    //Necessario para trabalhar com objetos complexos e realizar sorts
    class OrdenadorID : IComparer<Produto>
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
        produtos.Sort(new OrdenadorID());
        List<Produto> recheados = produtos.FindAll((produtos) => produtos.type == "Recheado");
        List<Produto> leite = produtos.FindAll((produtos) => produtos.type == "Leite");
        List<Produto> fruta = produtos.FindAll((produtos) => produtos.type == "Fruta");
        AddList(fruta);
        AddList(leite);
        AddList(recheados);
        PainelTamanho();
    }


    void AddList(List<Produto> type)
    {
         foreach(var produto in type)
        {
            GameObject prodID = Instantiate(prodPrefab, idBlock.transform);
            prodID.GetComponentInChildren<Text>().text = produto.id.ToString();
            GameObject prodNome = Instantiate(prodPrefab, nomeBox.transform);
            prodNome.GetComponentInChildren<Text>().text = produto.nome;
            GameObject prodType = Instantiate(prodPrefab, typeBox.transform);
            prodType.GetComponentInChildren<Text>().text = produto.type;
            GameObject prodQuant = Instantiate(prodPrefab, quantBox.transform);
            prodQuant.GetComponentInChildren<Text>().text = produto.amount.ToString();
        }
    }
}
