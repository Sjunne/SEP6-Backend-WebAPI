using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Actors
{
    public interface IActorRepository
    {
        List<Actor> GetActorsByKeyword(string title);
    }
}