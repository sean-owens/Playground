namespace beeGameDiceSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            int numOfPlayers = 2;
            int numOfDicePerRoll = 5;
            int numOfSimulations = 1000;

            Simulation testD6Sim = new Simulation(numOfPlayers, "D6");
            Simulation testD8Sim = new Simulation(numOfPlayers, "D8");

            //5 Dice
            testD6Sim.RunDiceSim(numOfDicePerRoll, numOfSimulations);
            testD8Sim.RunDiceSim(numOfDicePerRoll, numOfSimulations);

            //6 Dice
            testD6Sim.RunDiceSim(numOfDicePerRoll+1, numOfSimulations);
            testD8Sim.RunDiceSim(numOfDicePerRoll+1, numOfSimulations);

            //7 Dice
            testD6Sim.RunDiceSim(numOfDicePerRoll+2, numOfSimulations);
            testD8Sim.RunDiceSim(numOfDicePerRoll+2, numOfSimulations);

            //8 Dice
            testD6Sim.RunDiceSim(numOfDicePerRoll+3, numOfSimulations);
            testD8Sim.RunDiceSim(numOfDicePerRoll+3, numOfSimulations);

            testD6Sim.RunDiceSim(numOfDicePerRoll+4, numOfSimulations);
            testD8Sim.RunDiceSim(numOfDicePerRoll+4, numOfSimulations);
        }
    }
}
