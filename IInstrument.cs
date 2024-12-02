using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Procedural_Piano {
    public interface IInstrument {

        // Using List will take THE Enum of Notes, and then take a duration of int.
        // This is pretty self explainitory. DONT ADD ANYMORE HERE. This interface is
        // done (unless theres bugs directly from here).
        void PlayChord(List<Note> notes, int duration);
    }
}
