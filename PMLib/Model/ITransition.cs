using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model
{
    public interface ITransition
    {
        List<IPlace> InputPlaces { get; }
        List<IPlace> OutputPlaces { get; }
        string Id { get; }
        string Activity { get; }

        bool Fire();
    }
}
