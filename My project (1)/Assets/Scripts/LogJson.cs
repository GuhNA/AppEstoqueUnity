using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

[System.Serializable]
public class LogDaily
{
    public string[] logsDaily = {};
    public string day;
}
[System.Serializable]
public class Log
{
    public LogDaily[] logs = {};
}

public class LogJson : MonoBehaviour
{

    public Log logBanco = new();
    string jsonFile = "Logs.json";
    string jsonLogs;
    string filePath = "";
    void Start()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, jsonFile);
        jsonLogs = File.ReadAllText(filePath);

        //Se estiver vazio
        if(string.IsNullOrWhiteSpace(jsonLogs)) 
        {
            //Pega do banco de logs
            jsonLogs = JsonUtility.ToJson(logBanco,true);
            //Serializa
            File.WriteAllText(filePath, jsonLogs);
        }
        else 
            //Pega do arquivo e desserializa
            JsonUtility.FromJsonOverwrite(jsonLogs, logBanco);
        ResetLogs();
    }
    public void LogTime(string msg)
    {
        if(logBanco.logs.Length == 0 || logBanco.logs.Last().day != DateTime.Today.ToString())
        {
            LogDaily temp = new();
            temp.day = DateTime.Today.ToString();
            List<LogDaily> bancoTemp = logBanco.logs.ToList();
            bancoTemp.Add(temp);
            logBanco.logs = bancoTemp.ToArray();
        }
        //Converte objeto estático em objeto dinâmico adiciona um novo item e retorna.
        List<string> tempDaily = new();
        if(logBanco.logs.Last().logsDaily.Length > 0)
        {
            tempDaily = logBanco.logs.Last().logsDaily.ToList();
        }
        tempDaily.Add(DateTime.Now.ToString() + ": " + msg);
        logBanco.logs.Last().logsDaily = tempDaily.ToArray();
        print(logBanco.logs.Last().day);
        print(logBanco.logs.Last().logsDaily.Last());
        jsonLogs = JsonUtility.ToJson(logBanco, true);
        print(jsonLogs);
        File.WriteAllText(filePath, jsonLogs);

    }
    void ResetLogs()
    {
        //Calculando tempo em diferença de dias
        TimeSpan diferença = DateTime.Today - DateTime.Parse(logBanco.logs.First().day);
        if(diferença.Days > 6)
        {
            File.WriteAllText(filePath, "");
            logBanco = new();
        }
    }
}
