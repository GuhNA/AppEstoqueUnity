using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System;
using System.Threading;

public class ProductManager : MonoBehaviour
{
    int index = -1;
    [Header("Nome ou ID")]
    public InputField ID;
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

    private void Awake() {
        log = FindObjectOfType<LogJson>(); 
        popup = FindObjectOfType<Popup>();
        canvasController = FindObjectOfType<CanvasController>();
        databaseJson = FindObjectOfType<DatabaseJson>();
    }

    private void Start() {
        caixasOuQuantidades[1].onValueChanged.AddListener(valor=> Sobras(valor,true,1));
        caixasOuQuantidades[0].onValueChanged.AddListener(valor=> Sobras(valor,false,0));
        caixasOuQuantidades[0].text = "1";
        caixasOuQuantidades[1].text = "0";
    }

    private void Update() {

        if(ID.isFocused) IDselected();
        nome.onValueChanged.AddListener(nomeSelected);
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
            popup.AbrirPopup(textMorfo, false);
            databaseJson.SaveJSON();
            
         }
         else
         {
            if(databaseJson.banco.produtos[tempIndex].amount < count) popup.AbrirPopup("Valor acima da quantia do estoque!", true);
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
                popup.AbrirPopup(textMorfo, false);
            }
         }
        index = -1;
        caixasOuQuantidades[0].text = "1";
        caixasOuQuantidades[1].text = "0";
        canvasController.AlterarProduto(GameObject.Find("Alterar Produto 2"));
    }

    void Sobras(string input, bool verifica,int i)
    {
        //Lógica infernal pqp!!! <3

        //Zera o numbers q irá receber o input
        string numbers ="";
        //Caso seja vazio adicione zero
        if(input == "") 
        {
            active = true;
            caixasOuQuantidades[i].text = "0";
        }

        //trabalhará somente quando o active não estiver ativo
        foreach(char c in input)
        {
            if(!active)
            {
                if(char.IsDigit(c)) numbers += c;
            }
            else break;
        }
        //Pra casos de "sobras"
        if(verifica) verifica24(numbers);

        //Verifica se o individuo digitou "01, 10, 20, 10 etc..."
        if(input.Length > 1 && active)
        {
            //Escolhe o número q não é zero
            foreach(char number in input)
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

    void IDselected(){ if(nome.captionText.text != "") nome.value = 0;}
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
    

}
