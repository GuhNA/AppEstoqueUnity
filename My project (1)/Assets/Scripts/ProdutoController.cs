using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

public class ProdutoController : MonoBehaviour
{
    public int id, amount;
    public string nome, type;
    int i = 0;
    DatabaseJson json;
    
    [Space(10)]
    public InputField[] inputs;
    public Dropdown boxType;

    public Button salvar;

    private void Awake() {
        json = GetComponent<DatabaseJson>();    
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
        id = int.Parse(inputs[0].text);
        type = boxType.captionText.text;
        amount = int.Parse(inputs[2].text);
        nome = inputs[1].text;

        Produto produto  = new();
        produto.id = id;
        produto.nome = nome;
        produto.type = type;
        produto.amount = amount;


        ArrayUtility.Add(ref json.banco.produtos, produto);
        foreach(var input in inputs)
            input.text = "";

        json.AdicionarProdutoAoJSON();
    }


    void TrocaCampo()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(!inputs[0].isFocused && i == 0)
                {
                    print("Entrei");
                    inputs[0].Select();
                }
            else
            {
                if(i < 2)
                {
                    i++;
                    inputs[i].Select();
                }
                else if(i < 4)
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
        if(Input.GetKeyDown(KeyCode.Return))
        {
            salvar.onClick.Invoke();
            i = 0;
            inputs[i].Select();
        }
    }
    
}
