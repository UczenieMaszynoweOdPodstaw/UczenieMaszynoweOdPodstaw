using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class FileManager
{
    const string BEST_MODEL_FILE_PATH = "Assets/bestModel.json";

    public static void SaveModelToJsonFile(SteeringModel model)
    {
        var json = JsonConvert.SerializeObject(model);
        File.WriteAllText(BEST_MODEL_FILE_PATH, json);
    }

    public static SteeringModel ReadModelFromJsonFile()
    {
        if (!File.Exists(BEST_MODEL_FILE_PATH))
            return null;
        var json = File.ReadAllText(BEST_MODEL_FILE_PATH);
        return JsonConvert.DeserializeObject<SteeringModel>(json);
    }
}
