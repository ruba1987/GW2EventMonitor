using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwApiNET.ResponseObjects.Parsers
{
    /// <summary>
    /// Null Parser.  Does not perform any parsing, instead will return the default value of type T.
    /// </summary>
    /// <typeparam name="T">return type</typeparam>
    public class NullParser<T> : IApiResponseParserAsync<T>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public NullParser()
        {

        }

        /// <summary>
        /// Parses the response object.
        /// <remarks>Does not actually parse.</remarks>
        /// </summary>
        /// <param name="apiResponse">raw response from the GW2 Server</param>
        /// <returns>default(T)</returns>
        public T Parse(object apiResponse)
        {
            return default(T);
        }

        public Task<T> ParseAsync(object apiResponse)
        {
            TaskCompletionSource<T> tsk = new TaskCompletionSource<T>();
            tsk.SetResult(default(T));
            return tsk.Task;
        }
    }
}
