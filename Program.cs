using NAudio.Wave;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using The_Procedural_Piano;

namespace TheProceduralPiano;

static class Program
{
    private static int _counter = 0;
    static List<IInstrument> instruments = new List<IInstrument> {
            new Piano()
        // We can add more later.
        };
    static List<string> musicSequence = new List<string>();
    private static async Task obtainMusic(IPEndPoint enpoint)
    {
        string _name = "Nostes";
        using Socket listener = new(
            enpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        listener.Bind(enpoint);
        listener.Listen();

        for (; ; )
        {
            try
            {
                Console.WriteLine("Listening...");
                var handler = await listener.AcceptAsync();
                // this would get the notes
                var buffer = new byte[2048];
                var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                string? response = Encoding.UTF8.GetString(buffer, 0, received);

                //this wirtes to a file.
                string path = $"{_name}{_counter}";
                await File.WriteAllTextAsync(path, response);

                var encodingByte = Encoding.UTF8.GetBytes("thank you");
                await handler.SendAsync(encodingByte, 0);
                try
                {
                    handler.Shutdown(SocketShutdown.Both);
                }
                finally
                {
                    handler.Close();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("there was an error present");
            }
        }
        try
        {
            listener.Shutdown(SocketShutdown.Both);
        }
        finally
        {
            listener.Close();

        }
    }
    private static void Main()
    {
        IPEndPoint theIp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4444);
        // this will fun the file reciver in the back ground

        Task.Run(async () => await obtainMusic(theIp));

        for (; ; )
        {
            DisplayMenu();
            string? choice_str = Console.ReadLine()?.Trim();
            int choice = int.Parse(choice_str);
            switch (choice)
            {

                case (1):
                    CreateSong();
                    break;
                case (2):
                    Play();
                    break;
                case (3):
                    Save(GetUserInput("Name: "));
                    break;
                case (4):
                    Load(GetUserInput($"File name: "));
                    break;
                case (5):
                    int anOption = GetIntInput("please enter how many instraments you would like to play");
                    for (int i = 0; i < anOption; i++)
                    {
                        Load(GetUserInput($"File name: "));
                        Play();
                    }
                    break;
                case (6):
                    Task.Run(() =>
                    {
                        Load(GetUserInput($"File name: "));
                        Play();
                    });
                    break;
                case (7):
                    Console.WriteLine("Thank you for joining us");
                    return;
                default:
                    Console.WriteLine("this was not an option");
                    break;
            }

            static void Load(string fileName)
            {
                if (File.Exists(fileName))
                {
                    try
                    {
                        musicSequence = File.ReadAllLines(fileName).ToList();
                        Console.WriteLine($"Song loaded from {fileName}!");

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error. {fileName}");
                        Debug.WriteLine($"Loading problem exception: {e}");
                    }
                }
                else
                {
                    Console.WriteLine("File not found");
                }

            }

            static void DisplayMenu()
            {
                Console.WriteLine("\nProcedural Music Generator (Chords ONLY)");
                Console.WriteLine("1. Compose a new song");
                Console.WriteLine("2. Play current song");
                Console.WriteLine("3. Save");
                Console.WriteLine("4. Load");
                Console.WriteLine("5. Play multiple instraments");
                Console.WriteLine("6. play in the back ground");
                Console.WriteLine("7. Exit");
            }

            static string GetUserInput(string prompt)
            {
                string input;
                do
                {
                    Console.Write(prompt);
                    input = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(input)) Console.WriteLine("Input cannot be empty. Please try again.");

                } while (string.IsNullOrEmpty(input));
                return input;
            }

            static void CreateSong()
            {
                musicSequence.Clear();

                int songLength = GetIntInput("Enter No. of chords: ");
                IInstrument selectedInstrument = ChooseInstrument();

                Random random = new Random();

                for (int i = 0; i < songLength; i++)
                {

                    int chordSize = random.Next(3, 5);
                    List<Note> chord = new List<Note>();

                    for (int j = 0; j < chordSize; j++)
                    {

                        Note note = (Note)random.Next(Enum.GetValues(typeof(Note)).Length);
                        if (!chord.Contains(note))
                        {
                            chord.Add(note);
                        }
                    }

                    int duration = random.Next(200, 500);
                    musicSequence.Add($"{string.Join(", ", chord.Select(n => n.ToString()))},{duration}");

                }

                Console.WriteLine("Composed!");

            }

            static int GetIntInput(string prompt)
            {
                int value;
                while (!int.TryParse(GetUserInput(prompt), out value) || value <= 0)
                {
                    Console.WriteLine("Invalid input. Must enter a positive integer.");
                }
                return value;
            }

            static IInstrument ChooseInstrument()
            {
                while (true)
                {
                    Console.WriteLine("Available Instruments: ");
                    for (int i = 0; i < instruments.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {instruments[i].GetType().Name}");
                    }

                    int choice = GetIntInput("Choose an instrument (enter number): ");

                    if (choice > 0 && choice <= instruments.Count)
                    {
                        return instruments[choice - 1];
                    }
                    else
                    {
                        Console.WriteLine("Invalid instrument selected.");
                    }
                }
            }

            static void Play()
            {

                if (!musicSequence.Any())
                {

                    Console.WriteLine("There is no song composed yet! Please load or create!");
                    return;

                }

                IInstrument selectedInstrument = ChooseInstrument();

                foreach (string chordString in musicSequence)
                {
                    int lastCommaIndex = chordString.LastIndexOf(',');
                    if (lastCommaIndex == -1)
                    {
                        Console.WriteLine($"Invalid chord string format: {chordString}");
                        continue;
                    }

                    string notesPart = chordString.Substring(0, lastCommaIndex);
                    List<Note> notes = notesPart.Split(',').Select(noteStr => Enum.Parse<Note>(noteStr)).ToList();

                    string durationPart = chordString.Substring(lastCommaIndex + 1);
                    if (!int.TryParse(durationPart, out int duration))
                    {
                        Console.WriteLine($"Invalid duration");
                        Debug.WriteLine($"Invalid at: {chordString}");
                        continue;
                    }

                    selectedInstrument.PlayChord(notes, duration);
                }
                Console.WriteLine("Played!");
            }

            static void Save(string fileName)
            {
                try
                {
                    File.WriteAllLines(fileName, musicSequence);
                    Console.WriteLine($"Song saved to {fileName}.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error saving");
                    Debug.WriteLine($"At the Save method there is a exception: {e}");
                }
            }
        }
    }
}


    