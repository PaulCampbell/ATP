using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATP.Web.Resources
{
    public class PagableSortableList<T> : Resource
    {
        public int TotalResults { get; set; }
        public int RestultsPerPage { get; set; }
        public int PageNumber { get; set; }
        public List<T> Items { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }

        public PagableSortableList(int totalResults, int restultsPerPage, int pageNumber, List<T> items, 
            string sortBy, string sortDirection)
        {
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