using System.IO;
using UnityEngine;

public class DatabaseJson : MonoBehaviour
{
    public BancoDeDados banco;
    string jsonFileName = "data.json";
    string jsonData;
    string filePath;

    Popup popup;

    LogJson log;

    

    private void Awake() {
        log = FindObjectOfType<LogJson>(); 
        popup = FindObjectOfType<Popup>();
        filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        //Se não existir, criar um banco e guardar no JSON
        if (!File.Exists(filePath))
        {
            banco = new();
            jsonData = JsonUtility.ToJson(banco);
            File.WriteAllText(filePath, jsonData);
        }
        jsonData = File.ReadAllText(filePath);

        if(!string.IsNullOrWhiteSpace(jsonData))
            JsonUtility.FromJsonOverwrite(jsonData, banco);
        else
            banco = new();
    }

    public void SaveJSON()
    {
        File.WriteAllText(filePath, JsonUtility.ToJson(banco));
    }
    public void AdicionarProdutoAoJSON(string id, string nome, string quantia, string type)
    {
        // Caminho do arquivo JSON
        filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        // Ler o arquivo JSON
        if (File.Exists(filePath))
        {

            // Ler Conteúdo do Json
            jsonData = File.ReadAllText(filePath);
            if(!string.IsNullOrWhiteSpace(jsonData))
            {
                //Serializar de volta para JSON
                string updatedJsonData = JsonUtility.ToJson(banco, true);
                //Salvar o arquivo
                File.WriteAllText(filePath, updatedJsonData);

                popup.AbrirPopup($"Criado: {id}, {nome}, {quantia}, {type}.", false, false);
                log.LogTime($"Criado: {id}, {nome}, {quantia}, {type}.");
            }
            else
            {
                string newJsonData = JsonUtility.ToJson(banco, true);
                File.WriteAllText(filePath, newJsonData);
            }
        }
        else
            popup.AbrirPopup("Arquivo JSON não encontrado!",true, false);
    }
}
