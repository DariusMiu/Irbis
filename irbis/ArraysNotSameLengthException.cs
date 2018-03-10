using System;
namespace Irbis
{
    class ArraysNotSameLengthException : Exception
    {
        string _Message;

        public ArraysNotSameLengthException()
        { _Message = "Arrays are not all the same length."; }
        public ArraysNotSameLengthException(string message) : base(message)
        { _Message = message; }
        public ArraysNotSameLengthException(string message, Exception inner) : base(message, inner)
        { _Message = message; }

        public override string Message
        { get { return _Message; } }
    }
}
