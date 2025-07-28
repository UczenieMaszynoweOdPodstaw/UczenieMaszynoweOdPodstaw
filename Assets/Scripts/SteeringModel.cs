public class SteeringModel
{
    public NeuralNetwork Network { get; set; }
    public float Reward { get; set; }

    public SteeringModel()
    {
        Network = new NeuralNetwork();
    }
}
