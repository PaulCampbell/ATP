using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ATP.Web.Resources
{
     [DataContract]
    public class PagableSortableList<T> 
    {
          [DataMember]
        public int TotalResults { get; set; }
          [DataMember]
        public int RestultsPerPage { get; set; }
          [DataMember]
        public int PageNumber { get; set; }
          [DataMember]
        public List<T> Items { get; set; }
          [DataMember]
        public string SortBy { get; set; }
          [DataMember]
        public string SortDirection { get; set; }

          [DataMember]
        public List<ResourceLink> Actions { get; set; }

   

        public PagableSortableList(int totalResults, int restultsPerPage, int pageNumber, List<T> items, 
            string sortBy, string sortDirection)
        {
            Actions = new List<ResourceLink>();

            TotalResults = totalResults;
            RestultsPerPage = restultsPerPage;
            PageNumber = pageNumber;
            Items = items;
            SortBy = sortBy;
            SortDirection = sortDirection;

            AddActionLinks();

        }

        private void AddActionLinks()
        {
            if(PageNumber > 1)
                Actions.Add(new ResourceLink { Action = "Previous", 
                    Uri = string.Format("/?PageNumber={0}&ResultsPerPage={1}&" +
                                        "SortBy={2}&SortDirection={3}",
                                        PageNumber - 1, RestultsPerPage,
                                        SortBy, SortDirection)});

            if(PageNumber * RestultsPerPage <= TotalResults)
                Actions.Add(new ResourceLink
                {
                    Action = "Next",
                    Uri = string.Format("/?PageNumber={0}&ResultsPerPage={1}&" +
                                        "SortBy={2}&SortDirection={3}",
                                        PageNumber + 1, RestultsPerPage,
                                        SortBy, SortDirection)
                });

        }


    }
}