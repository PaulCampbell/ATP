using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATP.Domain
{
    public interface IDateProvider
    {
        DateTime Now();
    }

    public class DateProvider : IDateProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }

    }
}
