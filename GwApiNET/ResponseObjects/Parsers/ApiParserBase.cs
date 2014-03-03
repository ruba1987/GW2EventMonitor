using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwApiNET.ResponseObjects.Parsers
{

    public abstract class ApiParserBase<T> : IApiResponseParserAsync<T>
    {
        public abstract T Parse(object apiResponse);
        public abstract Task<T> ParseAsync(object apiResponse);
    }
}
