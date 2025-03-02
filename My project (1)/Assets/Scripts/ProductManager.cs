using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;

public class ProductManager : MonoBehaviour
{
    int index = -1;
    [Header("Nome ou ID")]
    public InputField ID, dropdownSelect;
    public Dropdown nome;

    [Space(10)][Header("Caixas e quantidades")]

    public InputField[] caixasOuQuantidades;

    [Space(10)]
    public Button metamorfo;
    CanvasController canvasController;
    DatabaseJson databaseJson;

    bool active = false;
    LogJson log;

    Popup popup;

    GameObject adicionarAP, removerAP, pai;
    private void Awake() {
        log = FindObjectOfType<LogJson>(); 
        popup = FindObjectOfType<Popup>();
        canvasController = FindObjectOfType<CanvasController>();
        databaseJson = FindObjectOfType<DatabaseJson>();
        adicionarAP = ProcurarFilho(GameObject.Find("Canvas"), "Adicionar AP");
        removerAP = ProcurarFilho(GameObject.Find("Canvas"), "Remover AP");
        pai = ProcurarFilho(GameObject.Find("Canvas"), "Alterar Produto");
        
    }

    private void Start() {
        caixasOuQuantidades[1].onValueChanged.AddListener(valor=> Sobras(valor,true,1));
        caixasOuQuantidades[0].onValueChanged.AddListener(valor=> Sobras(valor,false,0));
        dropdownSelect.onValueChanged.AddListener(input => DropSelect(input));
        dropdownSelect.contentType = InputField.ContentType.Name;
        ID.contentType = InputField.ContentType.IntegerNumber;
        caixasOuQuantidades[0].text = "1";
        caixasOuQuantidades[1].text = "0";
    }

    private void Update() {
        if(ID.isFocused) IDselected();
        nome.onValueChanged.AddListener(nomeSelected);
        
        if(pai.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.KeypadPlus)) adicionarAP.GetComponent<Button>().onClick.Invoke();
            if(Input.GetKeyDown(KeyCode.KeypadMinus)) removerAP.GetComponent<Button>().onClick.Invoke();
        }
        if(canvasController.alterarProduto2.activeSelf) 
        {
            if(Input.GetKeyDown(KeyCode.Return) && popup.permiteEnter) metamorfo.onClick.Invoke();
        }
        TrocaCampo();
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
            if(index == -1)
            {
                popup.AbrirPopup("Nenhum Produto encontrado, verifique o ID!",true, false);
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
            popup.AbrirPopup("Nenhum campo Preenchido!", true, false);
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
            databaseJson.banco.produtos[tempIndex].MaiorQZero(count);
            int temp = databaseJson.banco.produtos[tempIndex].amount;          
            textMorfo = $"{databaseJson.banco.produtos[tempIndex].nome}: {(temp-count)/24} cx(s) e {(temp-count)%24} uni(s) + "+ 
                        $"{count/24} cx(s) e {count%24} unidade(s)  = {temp/24} cx(s) + {temp%24} uni(s)";
            if(count%24 == 0)
            {
                if(temp%24 == 0) log.LogTime($"Adicionados {count/24} caixas de {databaseJson.banco.produtos[tempIndex].nome} totalizando {temp/24} caixas");
                else log.LogTime($"Adicionados {count/24} caixas de {databaseJson.banco.produtos[tempIndex].nome} totalizando {temp/24} caixas + {temp%24} unidades.");
            }
            else
            {
                if(temp%24 == 0) log.LogTime($"Adicionados {count/24} caixas + {count%24} unidades de {databaseJson.banco.produtos[tempIndex].nome} totalizando {temp/24} caixas");
                else  log.LogTime($"Adicionados {count/24} caixas + {count%24} unidades de {databaseJson.banco.produtos[tempIndex].nome} totalizando {temp/24} caixas + {temp%24} unidades.");
            }
            StartCoroutine(TimerPopup(textMorfo,false,false));
            databaseJson.SaveJSON();
            
         }
         else
         {
            if(databaseJson.banco.produtos[tempIndex].amount < count) popup.AbrirPopup("Valor acima da quantia do estoque!", true, false);
            else
            {
                databaseJson.banco.produtos[tempIndex].MaiorQZero(-count);
                int temp = databaseJson.banco.produtos[tempIndex].amount;     
                textMorfo = $"{databaseJson.banco.produtos[tempIndex].nome}: {(temp+count)/24} cx(s) e {(temp+count)%24} uni(s) - "+ 
                        $"{count/24} cx(s) e {count%24} uni(s)  = {temp/24} cx(s) + {temp%24} uni(s)";
                if(count%24 == 0)
                {
                    if(temp%24 == 0) log.LogTime($"Removidos {count/24} caixas de {databaseJson.banco.produtos[tempIndex].nome} totalizando {temp/24} caixas");
                    else log.LogTime($"Removidos {count/24} caixas de {databaseJson.banco.produtos[tempIndex].nome} totalizando {temp/24} caixas + {temp%24} unidades.");
                }
                else
                {
                    if(temp%24 == 0) log.LogTime($"Removidos {count/24} caixas + {count%24} unidades de {databaseJson.banco.produtos[tempIndex].nome} totalizando {temp/24} caixas");
                    else  log.LogTime($"Removidos {count/24} caixas + {count%24} unidades de {databaseJson.banco.produtos[tempIndex].nome} totalizando {temp/24} caixas + {temp%24} unidades.");
                }
                databaseJson.SaveJSON();
                StartCoroutine(TimerPopup(textMorfo,false,false));
            }
         }
        index = -1;
        caixasOuQuantidades[0].text = "1";
        caixasOuQuantidades[1].text = "0";
        canvasController.AlterarProduto(GameObject.Find("Alterar Produto 2"));
    }

    void Sobras(string input, bool verifica,int i)
    {
        string numbers ="";
        //Lógica infernal pqp!!! <3
        foreach(char c in input)
            if(char.IsDigit(c)) numbers +=c;

        //Zera o numbers q irá receber o input
        //Caso seja vazio adicione zero
        if(numbers == "") 
        {
            active = true;
            numbers = "0";
        }

        //Verifica se o individuo digitou "01, 10, 20, 10 etc..."
        if(numbers.Length > 1 && active)
        {
            //Escolhe o número q não é zero
            foreach(char number in numbers)
            {
                if(number != 0)
                {
                    caixasOuQuantidades[i].text = number.ToString();
                    break;
                }
            }
            //Permite adicionar novamente digitos, ativando o loop que recebe os inputs
            active = false;
        }
        else
        {
            if(!verifica) caixasOuQuantidades[i].text = numbers;
            //Pra casos de "sobras"
            else verifica24(numbers);
        }
    }

    void verifica24(string numbers)
    {
        if(int.TryParse(numbers,out int a)) 
        {
            if(a > 23)
            {
                numbers = "23";
                caixasOuQuantidades[1].text = numbers;
            }
        }
    }

    void IDselected(){ if(nome.captionText.text != "") dropdownSelect.text = "";}
    void nomeSelected(int index){ if(ID.text != "") ID.text = "";}

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
    
    void DropSelect(string input)
    {
        //Caso queira selecionar o texto caso esteja escrito errado automaticamente, porém fica meio chato.
        //bool change = false;
        for(int i = 0; i < nome.options.Count; i++)
        {
            if(Regex.IsMatch(nome.options[i].text, @$"\A{input}\w*"))
            {
              //change = true;
                nome.value = i;
                break;
            }
        }
        
        /*if(!change)
        {
            //Gambiarra do GPT para selecionar o texto inteiro dentro do InputField Legacy
            dropdownSelect.caretPosition = dropdownSelect.text.Length;
            dropdownSelect.selectionAnchorPosition = 0;
        }*/
    }

    public void AbrirPopup()
    {
        string msg;
        if(metamorfo.GetComponentInChildren<Text>().text == "Adicionar")
            msg = $"Deseja Adicionar {caixasOuQuantidades[0].text}cx(s) e {caixasOuQuantidades[1].text}uni(s)?";
        else
            msg = $"Deseja Remover {caixasOuQuantidades[0].text}cx(s) e {caixasOuQuantidades[1].text}uni(s)?";
        popup.AbrirPopup(msg,false, true);
    }


    void TrocaCampo()
    {
        if(canvasController.alterarProduto.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.Tab)) 
            {
                if(ID.isFocused) dropdownSelect.Select();
                else ID.Select();
            }
        }
    }
    //Por que se fez necessário essa função? Não queria referenciar 15 objs na unity.
    //O Find só encontra objs ativos. Então aí está a resposta
     GameObject ProcurarFilho(GameObject obj,string nome)
    {

        for(int i = 0; i < obj.transform.childCount; i++)
        {
            if(obj.transform.GetChild(i).name == nome) return obj.transform.GetChild(i).gameObject;
            else
            {
                GameObject temp = ProcurarFilho(obj.transform.GetChild(i).gameObject, nome);
                if(temp != null) return temp;
            }
        }
        return null;
    }

    IEnumerator TimerPopup(string txt, bool erro, bool ctz)
    {
        yield return new WaitForSeconds(.25f);
        popup.AbrirPopup(txt,erro,ctz);
    }
}
