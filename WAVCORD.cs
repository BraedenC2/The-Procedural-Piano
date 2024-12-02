using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Procedural_Piano {
    public class WAVCORD : IInstrument {

        // This class will need 2 methods probably.

        // We need it to play the chord from the wav file, AND then play the note itself. 

        private readonly string _musicFilesLocation = "NOTES";

        public void PlayChord(List<Note> notes, int duration) {
            List<Thread> threads = new List<Thread>();

            // We COULD change these to var probably. But idk if Brian will mark us down for vars.
            // Up to you if u wanna change it. If we want to add more chords, we can ;D
            foreach (Note note in notes) {

                Note noteDuplicate = note;
                Thread thread = new Thread(() => PlayNote(noteDuplicate));
                threads.Add(thread);
                thread.Start();

            }

            // This will make sure that the program waits for the other notes to finish.
            foreach (Thread thread in threads) thread.Join();

            // This pauses randomly between chords
            Random random = new Random(); 
            Thread.Sleep(random.Next(0,duration));
        }

        private void PlayNote(Note note) {
            string fileName = $"{note} NOTE.WAV";


        }
    }
}
