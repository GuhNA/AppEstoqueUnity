using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ProductManager : MonoBehaviour
{

    int index = 0, i = 0;
    bool _isActive = false;
    public bool isActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }

    [Header("Nome ou ID")]
    public InputField[] nomeOrID;

    [Space(10)][Header("Caixas e quantidades")]

    public InputField[] caixasOuQuantidades;

    [Space(10)]
    public Button metamorfo;
    CanvasController canvasController;
    DatabaseJson databaseJson;

    private void Awake() {
        canvasController = FindObjectOfType<CanvasController>();
    }

    private void Start() {

        databaseJson = FindObjectOfType<DatabaseJson>();
        caixasOuQuantidades[0].contentType = InputField.ContentType.IntegerNumber;
        caixasOuQuantidades[1].onValueChanged.AddListener(Sobras);
    }

    private void Update() {
        SelectCampo();
    }
    public void FindProduct(int type)
    {
        if(nomeOrID[0].text == "")
        {
            nomeOrID[0].text = "9999";
        }
        for(int i = 0; i < databaseJson.banco.produtos.Length; i++)
        {
            if (databaseJson.banco.produtos[i].id == int.Parse(nomeOrID[0].text) || databaseJson.banco.produtos[i].nome == nomeOrID[1].text)
            {
                index = i;
                canvasController.AlterarProduto(canvasController.mainMenu);
                if(type == 1)
                {
                    metamorfo.GetComponentInChildren<Text>().text = "Adicionar";
                    canvasController.AlterarProduto2nd();
                }
                else
                {
                    metamorfo.GetComponentInChildren<Text>().text = "Remover";
                    canvasController.AlterarProduto2nd();
                }


            }
        }
        if(index == 0)
            //Adicionar condição de erro
            print("Não existe este produto no banco");
    }


    public void AlterarItem(int type)
    {

        int tempIndex = index;
        int count = int.Parse(caixasOuQuantidades[0].text) * 24 + int.Parse(caixasOuQuantidades[1].text);
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
        caixasOuQuantidades[1].text = numbers;
    }

    void SelectCampo()
    {
        if(isActive)
        {
            InputField[] inputs;
            if(canvasController.alterarProduto.activeSelf)
                inputs = nomeOrID;
            else
                inputs = caixasOuQuantidades;
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                if(i < 1)
                    i++;
                else
                    i = 0;
                inputs[i].Select();
                
            }
        }

    }
}
