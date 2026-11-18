namespace AI_Learn_Myself.Preproccesing;

static class HelperFunctions
{
    public static void Train4BitBinToDec(this Network net, int rounds)
    {
        double[] inputs;
        double[] targets;
        for(int i = 0; i < rounds; i++)
        {
            inputs = (i % 16).ToBits(4);
            targets = (i % 16).DigitsToTargets(2);
            net.Train(inputs, targets);
        }
    }
    public static int Test4BitBinToDec(this Network net)
    {
        int correct = 0;
        for (int n = 0; n < 16; n++)
        {
            int answer = net.Run(n.ToBits(4)).OutputsToNumber();
            if (answer == n)
            {
                correct++;
            }
            //printing
            Printer.PrintConverterRow(n, n.ToBits(4), answer);
        }
        return correct;
    }
    public static bool Compare(this double[] array1, double[] array2)
    {
        bool same = true;
        if(array1.Length != array2.Length)
        {
            return false;
        }
        for(int i =  0; i < array1.Length && same; i++)
        {
            same = array1[i] != array2[i] ? false : true;
        }
        return same;
    }
    public static double[] ToBits(this int number, int size)
    {
        double[] output = new double[size];
        for (int i = 0; i < size; i++)
        {
            output[i] = (number >> i) & 1;
        }
        return output;
    }

    public static double[] DigitsToTargets(this int number, int digits)
    {
        double[] output = new double[digits * 10];
        for (int i = 0; i < digits; i++)
        {
            int pos = (int)(number % Math.Pow(10, i + 1) / Math.Pow(10, i));
            output[pos + 10 * i] = 1;
        }
        return output;
    }
    public static int OutputsToNumber(this double[] number)
    {
        int output = 0;
        int digits = number.Length / 10;
        double highestvalue;
        int highestdigit;
        
        for (int i = 0; i < digits; i++)
        {
            highestvalue = 0;
            highestdigit = 0;
            for(int j =  0; j < 10; j++)
            {
                if(highestvalue < number[j + i * 10])
                {
                    highestvalue = number[j + i * 10];
                    highestdigit = (int) (j * Math.Pow(10, i));
                }
            }
            output += highestdigit;
        }
        return output;
    }
}