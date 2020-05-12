using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model
{
    /// <summary>
    /// Interface for transitions in Petri Net.
    /// </summary>
    public interface ITransition
    {
        List<IPlace> InputPlaces { get; }
        List<IPlace> OutputPlaces { get; }
        string Id { get; }
        string Activity { get; }
    }
}
