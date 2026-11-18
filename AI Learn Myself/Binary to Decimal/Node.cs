namespace AI_Learn_Myself;

public class Node
{
    
    private double[] weights;
    private double bias;
    private double delta = 0.5;

    public Node(int size)
    {
        Random rng = new Random();
        weights = new double[size];
        bias = rng.NextDouble() * 2 - 1;
        
        for (int i = 0; i < size; i++)
        {
            weights[i] = rng.NextDouble() * 2 - 1;
        }
        
    }
    public double Run(double[] input)
    {
        double total = 0;
        for (int i = 0; i < input.Length; i++)
        {
            total += input[i] * weights[i];
        }    
        
        return 1 / (1 + Math.Exp(-(total + bias)));
    }

    public void Learn(double error, double[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            weights[i] += error * delta * inputs[i];
        }
        bias += error * delta;
    }
    
    public void AddSum(double[] sums, double blame)
    {
        for (int i = 0; i < sums.Length; i++)
        {
            sums[i] += blame * weights[i];   
        }
    }
}