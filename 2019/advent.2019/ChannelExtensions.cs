using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace advent._2019
{
    public static class ChannelExtensions
    {
        public static T MustRead<T>(this ChannelReader<T> reader)
        {
            return reader.TryRead(out T value) ?
                value :
                throw new InvalidOperationException("nothing to read");
        }

        public static void MustWrite<T>(this ChannelWriter<T> writer, T value)
        {
            if (!writer.TryWrite(value))
            {
                throw new InvalidOperationException("cannot write");
            }
        }
    }
}
