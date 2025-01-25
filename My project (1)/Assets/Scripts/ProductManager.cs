using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ProductManager : MonoBehaviour
{
    [Header("Nome ou ID")]
    public InputField prodName;
    public InputField prodID;

    [Space(10)][Header("Caixas e quantidades")]
    public InputField caixas;
    public InputField quantidades;
    int index = 0;

    [Space(10)]
    public Button metamorfo;

    CanvasController canvasController;
    DatabaseJson databaseJson;
    private void Awake() {
        canvasController = FindObjectOfType<CanvasController>();
    }

    private void Start() {

        databaseJson = FindObjectOfType<DatabaseJson>();
        caixas.contentType = InputField.ContentType.IntegerNumber;
        quantidades.onValueChanged.AddListener(Sobras);
    }


    public void FindProduct(int type)
    {
        for(int i = 0; i < databaseJson.banco.produtos.Length; i++)
        {
            if (databaseJson.banco.produtos[i].id == int.Parse(prodID.text) || databaseJson.banco.produtos[i].nome == prodName.text)
            {
                index = i;
                canvasController.AlterarProduto();
                if(type == 1)
                    metamorfo.GetComponentInChildren<Text>().text = "Adicionar";
                else
                    metamorfo.GetComponentInChildren<Text>().text = "Remover";


            }
        }
        if(index == 0)
            //Adicionar condição de erro
            print("Não existe este produto no banco");
    }


    public void AlterarItem(int type)
    {

        int tempIndex = index;
        int count = int.Parse(caixas.text) * 24 + int.Parse(quantidades.text);
         if(type == 1)
            databaseJson.banco.produtos[tempIndex].amount += count;
         else
            databaseJson.banco.produtos[tempIndex].amount -= count;
        index = 0;
        databaseJson.SaveJSON();
    }

    void Sobras(string input)
    {
        string numbers ="";
        foreach(char c in input)
        {
            if(char.IsDigit(c)) numbers += c;
            if(int.Parse(numbers) > 23) numbers = "23";
        }
        quantidades.text = numbers;
    }
}
