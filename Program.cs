using System.Diagnostics;
using System.Runtime.CompilerServices;
using The_Procedural_Piano;

namespace TheProceduralPiano;

static class Program {

    static List<IInstrument> instruments = new List<IInstrument> {
            new Piano()
        // We can add more later.
        };
    static List<string> musicSequence = new List<string>();

    private static void Main() {


        for (; ; ) {
            // We will need a switch statement here that allows user choices.
            // (It should called a display menu method. 
            // And then here should show a switch statement that allows user options.
            // Create, Play, Save, Load, Exit, and we can add others too. Like... samples maybe.

        }

    }

    static void DisplayMenu() {
        Console.WriteLine("\nProcedural Music Generator (Chords ONLY)");
        Console.WriteLine("1. Compose a new song");
        Console.WriteLine("2. Play current song");
        Console.WriteLine("3. Save");
        Console.WriteLine("4. Load");
        Console.WriteLine("5. Exit");
    }

    static string GetUserInput(string prompt) {
        string input;
        do {
            Console.Write(prompt);
            input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input)) Console.WriteLine("Input cannot be empty. Please try again.");

        } while (string.IsNullOrEmpty(input));
        return input;
    }


}