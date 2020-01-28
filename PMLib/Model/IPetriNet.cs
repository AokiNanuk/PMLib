using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model
{
    public interface IPetriNet
    {
        List<ITransition> Transitions { get; }

        List<IPlace> Places { get; }

        IPlace StartPlace { get; }

        IPlace EndPlace { get; }

    }
}
