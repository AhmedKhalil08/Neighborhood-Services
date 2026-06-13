using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Domain.AvilabilitiesException;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.AvilabilitiesException.Interfaces
{
    public interface IAvailabilityExceptionRepository : IGenericRepository<AvailabilityException , int>
    {
        Task<bool> IsDateExists( string technicianId , DateOnly date , int? exceptionId = null);
    }
}
