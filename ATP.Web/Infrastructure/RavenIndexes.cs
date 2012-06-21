using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATP.Domain.Models;
using Raven.Client.Indexes;
using Raven.Abstractions.Indexing;

namespace ATP.Web.Infrastructure
{
    public class RavenIndexes
    {
        public class Places_By_List : AbstractIndexCreationTask<Place>
        {
            public Places_By_List()
            {
                Map = places => from place in places
                                select new { place };
                Index(x => x.List, FieldIndexing.Analyzed);
            }
        }
    }
}