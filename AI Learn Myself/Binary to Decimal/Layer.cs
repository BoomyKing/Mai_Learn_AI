namespace AI_Learn_Myself;

public class Layer
{
    private Node[] neurons;
    int weightsCount;
    public Layer(int neuronCount,int weightsCount)
    {
        this.weightsCount = weightsCount;
        neurons = new Node[neuronCount];
        for(int i = 0; i < neurons.Length; i++)
        {
            neurons[i] = new Node(weightsCount);
        }
    }
    public double[] Run(double[] input)
    {
        int count = neurons.Length;
        double[] output = new double[count];
        for (int i = 0; i < count; i++)
        {
            output[i] = neurons[i].Run(input);
        }
        return output;
    }
    public void Learn(double[] inputs, double[] errors)
    {
        for(int i = 0; i< neurons.Length; i++)
        {
            neurons[i].Learn(errors[i], inputs);
        }
    }

    public double[] AddSum(double[] blames)
    {
        double[] sums = new double[weightsCount];
        for(int i = 0; i < neurons.Length; i++)
        {
            neurons[i].AddSum(sums, blames[i]);
        }
        return sums;
    }
}