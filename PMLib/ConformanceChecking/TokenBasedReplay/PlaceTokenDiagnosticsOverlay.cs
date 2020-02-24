﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.ConformanceChecking.TokenBasedReplay
{
    class PlaceTokenDiagnosticsOverlay
    {
        public uint Tokens { get; private set; } = 0;

        public uint Produced { get; private set; } = 0;

        public uint Consumed { get; private set; } = 0;

        public uint Missing { get; private set; } = 0;

        public uint Remaining { get; private set; } = 0;

        public void ConsumeToken()
        {
            if (Tokens == 0)
            {
                Missing++;
                return;
            }
            Tokens--;
            Consumed++;
        }

        public void ProduceToken()
        {
            Tokens++;
            Produced++;
        }

        public void SetRemaining()
        {
            Remaining = Tokens;
            Tokens = 0;
        }
    }
}
