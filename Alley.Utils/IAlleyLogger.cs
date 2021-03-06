﻿using Alley.Utils.Models;

namespace Alley.Utils
{
    public interface IAlleyLogger
    {
        void Error(string message);
        void Information(string message);
        void Warning(string message);
        void LogResult(IResult result);
    }
}