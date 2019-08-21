using System;
using Informer.Models;

namespace Informer.Utils
{
    public class SchemaEventArgs: EventArgs
    {
        public Schema Schema;

        public SchemaEventArgs(Schema schema) 
        {
            Schema = schema;
        }


    }
}
