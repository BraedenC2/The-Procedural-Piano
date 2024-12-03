
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
        public async Task PlayInstrumentAsync(string instrumentName, List<Note> notes, int duration)
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

       

        public  Dictionary<string, IInstrument> GetInstrament()
        {
        return instruments;
           }
           
}
}