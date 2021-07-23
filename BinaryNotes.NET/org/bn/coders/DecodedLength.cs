using System;
using System.IO;

namespace org.bn.coders
{
    public class DecodedLength : IDisposable
    {
        internal Stream stream;
        internal int value;
        internal int size;
        internal int numberOfIndefiniteLengthMarkers;

        public int Value
        {
            get
            {
                return value - (this.numberOfIndefiniteLengthMarkers * 2);
            }

            set
            {
                this.value = value;
            }
        }

        public bool IsIndefiniteLength
        {
            get
            {
                return (numberOfIndefiniteLengthMarkers > 0);
            }

        }

        virtual public int Size
        {
            get
            {
                return size;
            }

            set
            {
                this.size = value;
            }

        }

        private DecodedLength(Stream stream)
        {
            this.stream = stream;
        }

        public DecodedLength(Stream stream, int result) : this(stream)
        {
            Value = result;
        }

        public DecodedLength(Stream stream, int result, int size) : this(stream, result)
        {
            Size = size;
        }

        public DecodedLength(Stream stream, int result, int size, int numberOfUndefinedLengthMarkers) : this(stream, result, size)
        {
            this.numberOfIndefiniteLengthMarkers = numberOfUndefinedLengthMarkers;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (numberOfIndefiniteLengthMarkers > 0)
                    {
                        stream.ReadByte();
                        stream.ReadByte();
                        numberOfIndefiniteLengthMarkers = 0;
                    }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
