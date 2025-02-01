using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class ProdutoController : MonoBehaviour
{
    public int id, amount;
    public string nome, type;
    int i = 0;
    bool _isActive = false;

    #region encapsulamento
    public bool isActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }
    #endregion
    DatabaseJson json;
    
    [Space(10)]
    public InputField[] inputs;
    public Dropdown boxType;

    public Button salvar;

    Popup popup;
    private void Awake() {
        popup = FindObjectOfType<Popup>();
        json = FindObjectOfType<DatabaseJson>();    
    }
    void Start()
    {
        inputs[0].contentType = InputField.ContentType.IntegerNumber;
        inputs[1].contentType = InputField.ContentType.Name;
        inputs[2].contentType = InputField.ContentType.IntegerNumber;
    }
    private void Update() {
        TrocaCampo();
    }
    public void AddProduto()
    {
        if(inputs[0].text != "" && inputs[1].text != "" && inputs[2].text != "")
        {
            foreach(var prod in json.banco.produtos)
            {
                if(prod.id.ToString() == inputs[0].text || prod.nome == inputs[1].text) 
                {
                    inputs[0].text = ""; inputs[1].text = ""; inputs[2].text = "";
                    popup.AbrirPopup("Erro: Produto já existente!",true);
                    return;
                }
            }
            
            id = int.Parse(inputs[0].text);
            type = boxType.captionText.text;
            amount = int.Parse(inputs[2].text);
            nome = inputs[1].text;
            Produto produto = new()
            {
                id = id,
                nome = nome,
                type = type,
                amount = amount
            };


            //Necessário transformar o banco em lista
            List<Produto> attBanco = json.banco.produtos.ToList();
            attBanco.Add(produto);
            json.banco.produtos = attBanco.ToArray();
            foreach(var input in inputs)
                input.text = "";

            json.AdicionarProdutoAoJSON(produto.id.ToString(), nome, produto.amount.ToString(), type);
            i = 0;
            inputs[i].Select();
            boxType.value = 0;
        }
        else
        {
            popup.AbrirPopup("Erro: Campo Vazio!", true);
        }
    }


    void TrocaCampo()
    {
        if(isActive)
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                if(!inputs[0].isFocused && i == 0)
                    {
                        inputs[0].Select();
                    }
                else
                {
                    if(i < 2)
                    {
                        i++;
                        inputs[i].Select();
                    }
                    else if(i < 7)
                    {
                        i++;
                        boxType.value+=1;
                    }
                    else
                    {
                        i = 0;
                        inputs[i].Select();
                        boxType.value = 0;
                    }
                }
            }
            if(i < 3)
            {
                if(inputs[0].isFocused) i = 0;
                else if(inputs[1].isFocused) i = 1;
                else if(inputs[2].isFocused) i = 2;
            }
            boxType.onValueChanged.AddListener(dropdownIndex);
            if(Input.GetKeyDown(KeyCode.Return))
            {
                salvar.onClick.Invoke();
            }

        }
    }
    void dropdownIndex(int index)
    {
        if(index != 0)
            i = 3 + index;
    }
    
}
