using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class ProductManager : MonoBehaviour
{

    int index = 0;
    bool focusID;
    [Header("Nome ou ID")]
    public InputField ID;
    public Dropdown nome;

    [Space(10)][Header("Caixas e quantidades")]

    public InputField[] caixasOuQuantidades;

    [Space(10)]
    public Button metamorfo;
    CanvasController canvasController;
    DatabaseJson databaseJson;

    Popup popup;

    private void Awake() {
        popup = FindObjectOfType<Popup>();
        canvasController = FindObjectOfType<CanvasController>();
        databaseJson = FindObjectOfType<DatabaseJson>();
    }

    private void Start() {
        caixasOuQuantidades[0].contentType = InputField.ContentType.IntegerNumber;
        caixasOuQuantidades[1].onValueChanged.AddListener(Sobras);
        caixasOuQuantidades[0].text = "1";
        caixasOuQuantidades[1].text = "0";
    }

    private void Update() {
        if(!focusID)
        {
            nomeSelected();
            if(ID.isFocused) focusID = true;
        }
        if(focusID)
        {
            IDselected();
            if(!ID.isFocused) focusID = false;
        }
    }

    public void FindProduct(int type)
    {
        if(type == 1)
            metamorfo.GetComponentInChildren<Text>().text = "Adicionar";
        else
            metamorfo.GetComponentInChildren<Text>().text = "Remover";
        if(ID.text != "")
        {
            for(int i = 0; i < databaseJson.banco.produtos.Length; i++)
            {
                if (databaseJson.banco.produtos[i].id == int.Parse(ID.text))
                {
                index = i;
                canvasController.AlterarProduto2nd();
                }
            }
            if(index == 0)
            {
                popup.AbrirPopup("Nenhum Produto encontrado, verifique o ID!",true);
                ID.Select();
            }
            
        }
        else if(nome.captionText.text != "")
        {
            for(int i = 0; i < databaseJson.banco.produtos.Length; i++)
            {
                if (databaseJson.banco.produtos[i].nome == nome.captionText.text)
                {
                index = i;
                canvasController.AlterarProduto2nd();
                }
            }
        }
        else
        {
            popup.AbrirPopup("Nenhum campo Preenchido!", true);
            ID.Select();
        }
    }


    public void AlterarItem()
    {
        string textMorfo = "";
        int tempIndex = index;
        int count = int.Parse(caixasOuQuantidades[0].text) * 24 + int.Parse(caixasOuQuantidades[1].text);
         if(metamorfo.GetComponentInChildren<Text>().text == "Adicionar")
         {
            databaseJson.banco.produtos[tempIndex].amount += count;
            textMorfo = "Quantidades adicionadas com sucesso!";
         }
         else
         {
            databaseJson.banco.produtos[tempIndex].amount -= count;
            textMorfo = "Quantidades removidas com sucesso!";
         }
        index = 0;
        databaseJson.SaveJSON();
        popup.AbrirPopup(textMorfo, false);
        caixasOuQuantidades[0].text = "1";
        caixasOuQuantidades[1].text = "0";
        canvasController.AlterarProduto(GameObject.Find("Alterar Produto 2"));
    }

    void Sobras(string input)
    {
        string numbers ="";
        foreach(char c in input)
        {
            if(char.IsDigit(c)) numbers += c;
            if(int.Parse(numbers) > 23) numbers = "23";
        }
        caixasOuQuantidades[1].text = numbers;
    }

    void IDselected(){ if(nome.captionText.text != "") nome.value = 0;}
    void nomeSelected(){ if(ID.text != "") ID.text = "";}

    public void LoadDropdown()
    {
        List<string> options = new()
        {
            ""
        };
        foreach(var produto in databaseJson.banco.produtos) options.Add(produto.nome);
        options.Sort();
        nome.ClearOptions();
        nome.AddOptions(options);

    }

}
