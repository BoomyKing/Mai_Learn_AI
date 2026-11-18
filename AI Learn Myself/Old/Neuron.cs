namespace AI_Learn_Myself;
class Neuron
{
    private double w1, w2, b;
    private double delta = 0.5;

    // read-only access so the Printer class can show the weights
    public double W1 => w1;
    public double W2 => w2;
    public double B => b;
    public double Delta => delta;
    
    public Neuron()
    {
        Random rng = new Random();
        w1 = rng.NextDouble() * 2 - 1;
        w2 = rng.NextDouble() * 2 - 1;
        b = rng.NextDouble() * 2 - 1;
    }

    // ---------- training runs ----------

    public void TrainAnd(int rounds)
    {
        Console.WriteLine("\n========== AND ==========");
        Printer.PrintWeights(this);   // starting weights
        for (int r = rounds; r > 0; r--)
            LearnAnd(r % 2 == 0, r % 4 >= 2);

        Printer.PrintHeader();
        foreach (bool a in new[] { false, true })
            foreach (bool c in new[] { false, true })
                Printer.PrintRow(this, a, c, a && c);
        Console.WriteLine();
        Printer.PrintWeights(this);   // final weights
    }
    
    public void TrainOr(int rounds)
    {
        Console.WriteLine("\n========== OR ==========");
        Printer.PrintWeights(this);
        for (int r = rounds; r > 0; r--)
            LearnOr(r % 2 == 0, r % 4 >= 2);

        Printer.PrintHeader();
        foreach (bool a in new[] { false, true })
            foreach (bool c in new[] { false, true })
                Printer.PrintRow(this, a, c, a || c);
        Console.WriteLine();
        Printer.PrintWeights(this);
    }
    public void TrainXorOld(int rounds)
    {
        Neuron hidden1 = new Neuron();
        Neuron output = new Neuron();
        Console.WriteLine("\n========== XOR ==========");
        Printer.PrintWeights(output);
        for (int r = rounds; r > 0; r--)
        {
            LearnOr(r % 2 == 0, r % 4 >= 2);
            hidden1.LearnNand(r % 2 == 0, r % 4 >= 2);
            
        }
        for (int r = rounds; r > 0; r--)
        {
            output.LearnAnd(Run(r % 2 == 0, r % 4 >= 2),hidden1.Run(r % 2 == 0, r % 4 >= 2));
        }
        
        Printer.PrintXorHeader();
        foreach (bool a in new[] { false, true })
        foreach (bool c in new[] { false, true })
            Printer.PrintXorRow(output, a, c, Run(a,c), hidden1.Run(a,c), a != c);
        Console.WriteLine();
        Printer.PrintWeights(output);
    }
    
    public void TrainXor(int rounds)
    {
        Neuron hidden1 = new Neuron();
        Neuron hidden2 = new Neuron();
        int every = Math.Max(1, rounds / 10);   // print progress 10 times
        Printer.PrintXorProgressHeader();
        for (int r = rounds; r > 0; r--)
        {
            LearnXor(r % 2 == 0, r % 4 >= 2, hidden1,hidden2);
            int done = rounds - r + 1;
            if (done % every == 0)
                Printer.PrintXorProgress(done, this, hidden1, hidden2);
        }
        Printer.PrintXorHeader();
        foreach (bool a in new[] { false, true })
        foreach (bool c in new[] { false, true })
            Printer.PrintXorRow(this, a, c, hidden1.Run(a,c), hidden2.Run(a,c), a != c);
        Console.WriteLine();
        Printer.PrintWeights(this);
    }
    public double Run(bool I1, bool I2)
    {
        double x1 = I1 ? 1 : 0;
        double x2 = I2 ? 1 : 0;
        double c = w1 * x1 + w2 * x2 + b;
        return 1 / (1 + Math.Exp(-c));
    }
    public double Run(double I1, double I2)
    {
        double c = w1 * I1 + w2 * I2 + b;
        return 1 / (1 + Math.Exp(-c));
    }
    public void LearnAnd(bool I1, bool I2)
    {
        double runout = Run(I1, I2);
        bool guess = runout >= 0.5;
        double error = (I1&&I2 ? 1 : 0) - runout;
        w1 += delta * (I1 ? error : 0);
        w2 += delta * (I2 ? error : 0); 
        b += delta * error;
        Printer.PrintStep(this, (I1&&I2)== guess, I1, I2);
    }
    public void LearnAnd(double I1, double I2)
    {
        bool x1 = I1 >= 0.5;
        bool x2 = I2 >= 0.5;
        double runout = Run(I1, I2);
        bool guess = runout >= 0.5;
        double error = (x1&&x2 ? 1 : 0) - runout;
        w1 += delta * (error * I1);
        w2 += delta * (error * I2);
        b += delta * error;
        Printer.PrintStep(this, (x1&&x2)== guess, I1, I2);
    }
    public void LearnOr(bool I1, bool I2)
    {
        double runout = Run(I1, I2);
        bool guess = runout >= 0.5;
        double error = (I1||I2 ? 1 : 0) - runout;
        w1 += delta * (I1 ? error : 0);
        w2 += delta * (I2 ? error : 0); 
        b += delta * error;
        Printer.PrintStep(this, (I1||I2)== guess, I1, I2);
    }
    public void LearnNand(bool I1, bool I2)
    {
        double runout = Run(I1, I2);
        bool guess = runout >= 0.5;
        double error = (I1&&I2 ? 0 : 1) - runout;
        w1 += delta * (I1 ? error : 0);
        w2 += delta * (I2 ? error : 0); 
        b += delta * error;
        Printer.PrintStep(this, !(I1&&I2)== guess, I1, I2);
    }

    public void LearnXor(bool I1, bool I2, Neuron H1, Neuron H2)
    {
        double x1 = H1.Run(I1, I2);
        double x2 = H2.Run(I1, I2);
        double runout = Run(x1, x2);
        bool guess = runout >= 0.5;
        double in1 = I1 ? 1 : 0;
        double in2 = I2 ? 1 : 0;
        double error = ((I1 != I2) ? 1 : 0) - runout;
        
        //assign blame for Backpropagation
        double blameH1 = delta * x1 * (1 - x1) * error * w1;
        double blameH2 = delta * x2 * (1 - x2) * error * w2;
        
        //Backpropagate hidden node 1
        H1.w1 += blameH1 * in1;
        H1.w2 += blameH1 * in2;        
        H1.b += blameH1;
        
        //Backpropagate hidden node 2
        H2.w1 += blameH2 * in1;
        H2.w2 += blameH2 *in2;
        H2.b += blameH2;
        
        //Train output node
        w1 += delta * error * x1;
        w2 += delta * error * x2;
        b += delta * error;
        
    }

}