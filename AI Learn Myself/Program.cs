using AI_Learn_Myself.Preproccesing;

namespace AI_Learn_Myself;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("How many runs should we train it for?\n");
        int rounds = int.Parse(Console.ReadLine());

        Network net = new Network(4, [16, 20]);
        
        //printing
        Printer.PrintConverterHeader();
        
        //train and run a 4 bit binary to decimal neural net
        net.Train4BitBinToDec(rounds); 
        int correct = net.Test4BitBinToDec();
        
        Printer.PrintScore(correct, 16);
    }
}