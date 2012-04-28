using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATP.Web.Infrastructure
{
    public interface IAutomapper
    {
        T Map<TInput,T>(TInput model);
    }

    public class Automapper : IAutomapper
    {
        public T Map<TInput,T>(TInput model)
        {
            return AutoMapper.Mapper.Map<TInput, T>(model);
        }
    }
}