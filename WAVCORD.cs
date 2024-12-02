﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

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
            string path = Path.Combine(_musicFilesLocation, fileName);

            if (File.Exists(path)) {
                try {
                    // IN ORDER FOR THIS TO WORK HERE AND HERE FROM NOW ON, YOU NEED TO DOWNLOAD
                    // THE NAudio.Wave NeGet!!!! 

                    // Basically, this uses the class from NAudio that reads an audio stream.
                    // it's pretty old, so if there are bugs, and everything seems to work, then
                    // we should maybe switch to a different NuGet library.

                    using (MediaFoundationReader reader = new MediaFoundationReader(path))
                    using (WaveOutEvent output = new WaveOutEvent()) {
                        output.Init(reader);
                        output.Play();
                        while (output.PlaybackState == PlaybackState.Playing) {
                            Thread.Sleep(50);
                        }
                    }

                } catch (Exception e) {
                    Debug.WriteLine($"Error at method PlayNote in WAVCORD Class. " +
                        $"\n Path: {path}: {e.Message}.");
                }
            } else {
                Debug.WriteLine($"Error at method PlayNote in WAVCORD Class." +
                    $"\nSound file cannot be found fsr: {path}");
            }

        }
    }
}