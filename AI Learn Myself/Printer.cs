namespace AI_Learn_Myself;

// All console output lives here, so Neuron.cs only has the learning logic.
static class Printer
{
    // ---------- weights ----------

    public static void PrintWeights(Neuron n)
    {
        Console.WriteLine($" w1 = {n.W1,8:F4}   w2 = {n.W2,8:F4}   b = {n.B,8:F4}   delta = {n.Delta:F4}");
    }

    // ---------- training log ----------

    // One line per training step: was the guess right, which case, and the weights after learning.
    public static void PrintStep(Neuron n, bool right, bool I1, bool I2)
    {
        Console.Write($" {(right ? "right" : "wrong")} on   ({I1,-5}, {I2,-5})  →");
        PrintWeights(n);
    }

    // Same, for a neuron whose inputs are numbers (e.g. the outputs of other neurons).
    public static void PrintStep(Neuron n, bool right, double I1, double I2)
    {
        Console.Write($" {(right ? "right" : "wrong")} on   ({I1,6:F4}, {I2,6:F4})  →");
        PrintWeights(n);
    }

    // ---------- training progress: XOR network ----------

    public static void PrintXorProgressHeader()
    {
        Console.WriteLine();
        Console.WriteLine(" Training progress (network output for each case):");
        Console.WriteLine(" round          FF         FT         TF         TT");
    }

    // One line: the whole network's output for each of the four cases.
    public static void PrintXorProgress(int round, Neuron output, Neuron h1, Neuron h2)
    {
        Console.Write($" {round,-8}");
        foreach (bool a in new[] { false, true })
            foreach (bool c in new[] { false, true })
            {
                double o = output.Run(h1.Run(a, c), h2.Run(a, c));
                Console.Write($"   {o,8:F4}");
            }
        Console.WriteLine();
    }

    // ---------- results table: single neuron ----------

    public static void PrintHeader()
    {
        Console.WriteLine();
        Console.WriteLine(" I1     I2     Output  Guess  Correct");
        Console.WriteLine(" -----  -----  ------  -----  -------");
    }

    public static void PrintRow(Neuron n, bool a, bool c, bool target)
    {
        double o = n.Run(a, c);
        bool guess = o >= 0.5;
        Console.WriteLine($" {a,-5}  {c,-5}  {o,-6:F4}  {guess,-5}  {(guess == target ? "yes" : "NO")}");
    }

    // ---------- results table: XOR network ----------

    public static void PrintXorHeader()
    {
        Console.WriteLine();
        Console.WriteLine("  I1     I2      H1      H2    Output  Guess  Correct");
        Console.WriteLine(" -----  -----  ------  ------  ------  -----  -------");
    }

    public static void PrintXorRow(Neuron output, bool a, bool c, double h1Out, double h2Out, bool target)
    {
        double o = output.Run(h1Out, h2Out);
        bool guess = o >= 0.5;
        Console.WriteLine($" {a,-5}  {c,-5}  {h1Out,-6:F4}  {h2Out,-6:F4}  {o,-6:F4}  {guess,-5}  {(guess == target ? "yes" : "NO")}");
    }

    //printing: binary to decimal converter

    public static string BitsToString(double[] bits)
    {
        string text = "";
        for (int i = bits.Length - 1; i >= 0; i--)
        {
            text += bits[i] >= 0.5 ? "1" : "0";
        }
        return text;
    }

    public static void PrintConverterHeader()
    {
        Console.WriteLine();
        Console.WriteLine(" Number  Binary  Answer  Correct");
        Console.WriteLine(" ------  ------  ------  -------");
    }

    public static void PrintConverterRow(int number, double[] bits, int answer)
    {
        Console.WriteLine($" {number,-6}  {BitsToString(bits),-6}  {answer,-6}  {(answer == number ? "yes" : "NO")}");
    }

    public static void PrintScore(int correct, int total)
    {
        Console.WriteLine();
        Console.WriteLine($" Score: {correct}/{total}");
    }
}
