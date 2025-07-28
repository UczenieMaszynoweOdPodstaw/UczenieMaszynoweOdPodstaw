using System.Collections.Generic;

public class Generation
{
    public List<SteeringModel> SteeringModels { get; set; }

    public Generation()
    {
        SteeringModels = new List<SteeringModel>();
    }

    public void InitRandom(int modelsCount)
    {
        for (int i = 0; i < modelsCount; i++)
        {
            var model = new SteeringModel();
            model.Network.InitRandom();
            SteeringModels.Add(model);
        }
    }
}
