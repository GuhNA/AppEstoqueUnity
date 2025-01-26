using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

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

    private void Awake() {
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
        print("entrei");
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
        i = 0;
        inputs[i].Select();
        boxType.value = 0;
    }


    void TrocaCampo()
    {
        if(isActive)
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                print(i);
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
            }
        }
    }
    
}
