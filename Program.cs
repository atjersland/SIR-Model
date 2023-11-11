using CommandLine;
class Program{
    public class State{
        public int Day { get; set; }
        public double S { get; set; } // Susceptible
        public double I { get; set; } // Infectious
        public double R { get; set; } // Recovered

        public State(int day, double s, double i, double r){
            Day = day;
            S = s;
            I = i;
            R = r;
        }
    }
    class Options{
        [Option('d', "days", Required = false, HelpText = "Numbers of days the simulation runs for. Defaults to 100.")]
        public string days { get; set; }

        [Option('i', "infectionRate", Required = false, HelpText = "Rate at which susceptible become infected. Defaults to 0.0.")]
        public string infectionRate { get; set; }

        [Option('r', "recoveryRate", Required = false, HelpText = "Rate at which infected people recover from the infection. Defaults to 0.0.")]
        public string recoveryRate { get; set; }

        [Option('s', "resusceptibleRate", Required = false, HelpText = "Rate at which recovered people lose their immunity and become susceptible. Defaults to 0.0.")]
        public string resusceptibleRate { get; set; }

        [Option('S', "initialSusceptible", Required = false, HelpText = "Initial susceptible population. Defaults to v.")]
        public string initialSusceptible { get; set; }

        [Option('I', "initialInfected", Required = false, HelpText = "Initial infected population. Defaults to 0.0.")]
        public string initialInfected { get; set; }

        [Option('R', "initialRecovered", Required = false, HelpText = "Initial recovered population. Defaults to 0.0.")]
        public string initialRecovered { get; set; }

        [Option('o', "outputPath", Required = false, HelpText = "Path to the simulation output file. Defaults to \"output.csv\"")]
        public string outputPath { get; set; }
        
    }
    static void Main(string[] args){
        int days = 100;

        string outputPath = "output.csv";

        double infectionRate = 0.0;
        double recoveryRate = 0.0;
        double resusceptibleRate = 0.00;

        double s0 = 0.0; // Initial susceptible
        double i0 = 0.0;  // Initial infectious
        double r0 = 0.0;  // Initial recovered

        //assign parsed args to variables
        var result = Parser.Default.ParseArguments<Options>(args)
            .WithParsed(options =>
            {
                days = Int32.Parse(options.days);

                outputPath = options.outputPath;

                infectionRate = Convert.ToDouble(options.infectionRate);
                recoveryRate = Convert.ToDouble(options.recoveryRate);
                resusceptibleRate = Convert.ToDouble(options.resusceptibleRate);

                s0 = Convert.ToDouble(options.initialSusceptible);
                i0 = Convert.ToDouble(options.initialInfected);
                r0 = Convert.ToDouble(options.initialRecovered);
            });

        if (result.Tag == ParserResultType.NotParsed){
            // Help text requested, or parsing failed. Exit.
            return;
        }

        //create initial state for storing previous day's state.
        State previousState = new State(0, s0, i0, r0);

        using(StreamWriter sw = new StreamWriter(outputPath)){
            //write csv column headers
            sw.WriteLine("Day,Susceptible,Infected,Recovered");

            //record initial state as day 0
            sw.WriteLine($"{previousState.Day},{previousState.S},{previousState.I},{previousState.R}");
            for (int day = 1; day <= days; day++){

                //calculate new populations
                double newS = previousState.S + (resusceptibleRate * previousState.R - infectionRate * previousState.I * previousState.S);
                double newI = previousState.I + (infectionRate * previousState.I * previousState.S - recoveryRate * previousState.I);
                double newR = previousState.R + (recoveryRate * previousState.I - resusceptibleRate * previousState.R);

                //Normalize the values to attempt to sum to 100%
                double sum = newS + newI + newR;
                newS /= sum;
                newI /= sum;
                newR /= sum;

                //save new state
                previousState = new State(day, newS, newI, newR);

                //write current state to file
                sw.WriteLine($"{previousState.Day},{previousState.S},{previousState.I},{previousState.R}");
            }
        }
        Console.WriteLine("Simulation Finished. Written to " + outputPath);  
    }
}