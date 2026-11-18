namespace AI_Learn_Myself;

public class Network
{
    private Layer[] layers;
    private double[][] answers;
    
    public Network(int inputcount, int[] LayerSizes)
    {
        
        layers = new Layer[LayerSizes.Length];
        for (int i = 0; i < LayerSizes.Length; i++)
        {
            layers[i] = new Layer(LayerSizes[i], i == 0 ? inputcount : LayerSizes[i - 1]);
        }
        
    }
    public double[] Run(double[] inputs)
    {
        answers = new  double[layers.Length+1][];
        answers[0] = inputs;
        for (int i = 1; i < layers.Length+1; i++)
        { 
            answers[i] = layers[i-1].Run(answers[i-1]);
        }

        return answers[layers.Length];
    }
    
    private void Learn(double[] targets)
    {
        double[][] errors = new double[layers.Length+1][];

        int last = layers.Length;
        errors[last] = new double[answers[last].Length];
        for (int j = 0; j < answers[last].Length; j++)
        {
            errors[last][j] = targets[j] - answers[last][j];
        }

        for (int s = last - 1; s > 0; s--)
        {
            double[] sums = layers[s].AddSum(errors[s+1]);
            errors[s] = new double[answers[s].Length];
            for (int j = 0; j < answers[s].Length; j++)
            {
                double h = answers[s][j];
                double slope = h * (1 - h);
                errors[s][j] = sums[j]*slope;
            }
        }

        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].Learn(answers[i], errors[i+1]);
        }
    }
    public double[] Train(double[] inputs, double[] targets)
    {
        double[] output = Run(inputs);
        Learn(targets);
        return output;
    }
}