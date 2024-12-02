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
            DisplayMenu();
            string? choice_str = Console.ReadLine()?.Trim();
            int choice = int.Parse(choice_str); 
            switch (choice)
            {

                case (1):
                    CreateSong();
                    break;
                case (2):
                    break;
                case (3):
                    break;
                case (4):
                    
                    break;
                case (5):
                    Console.WriteLine("Thank you for joining us");
                    ;
                    break;
                
                default:
                    Console.WriteLine("this was not an option");
                    break;
            }
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

    static void CreateSong() {
        musicSequence.Clear();

        int songLength = GetIntInput("Enter No. of chords: ");
        IInstrument selectedInstrument = ChooseInstrument();

        Random random = new Random();

        for (int i = 0; i < songLength; i++) {

            int chordSize = random.Next(3, 5);
            List<Note> chord = new List<Note>();

            for (int j = 0; j < chordSize; j++) {

                Note note = (Note)random.Next(Enum.GetValues (typeof(Note)).Length);
                if (!chord.Contains(note)) {
                    chord.Add(note);
                }
            }

            int duration = random.Next(200, 500);
            musicSequence.Add($"{string.Join(", ", chord.Select(n=> n.ToString()))},{duration}");

        }

        Console.WriteLine("Composed!");

    }

    static int GetIntInput(string prompt) {
        int value;
        while (!int.TryParse(GetUserInput(prompt), out value) || value <= 0) {
            Console.WriteLine("Invalid input. Must enter a positive integer.");
        }
        return value;
    }

    static IInstrument ChooseInstrument() {
        while (true) {
            Console.WriteLine("Available Instruments: ");
            for (int i = 0; i < instruments.Count; i++) {
                Console.WriteLine($"{i + 1}. {instruments[i].GetType().Name}");
            }

            int choice = GetIntInput("Choose an instrument (enter number): ");

            if (choice > 0 && choice <= instruments.Count) {
                return instruments[choice - 1];
            } else {
                Console.WriteLine("Invalid instrument selected.");
            }
        }
    }

}