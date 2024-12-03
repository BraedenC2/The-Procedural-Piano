
namespace The_Procedural_Piano
{
    public class AudioManager 
    {
        private Dictionary<string, IInstrument> instruments;

        public AudioManager() 
        {
            instruments = new Dictionary<string, IInstrument>();
            instruments["pinao"] = new Piano();

        }
        public async Task PlayInstrumentAsync(string instrumentName,List<Note> notes, int duration)
        {
            if (instruments.TryGetValue(instrumentName, out var audioSource))
            {
                await audioSource.PlayChord(notes, duration);
            }
            else
            {
                throw new ArgumentException($"Instrument {instrumentName} not found");
            }
        }
        public void AddIntrument(string name, IInstrument instrument)
        {
            instruments[name] = instrument;
        }

        public async Task PlayMultipleInstruments(Dictionary<string,List<Note>>InstramentNotes, int duration )
        {
            List<Task> tasks = new List<Task>();
           foreach( var kvp in InstramentNotes)
            {
                tasks.Add(PlayInstrumentAsync(kvp.Key, kvp.Value, duration));
            }

            await Task.WhenAll(tasks);
        }
    }
}