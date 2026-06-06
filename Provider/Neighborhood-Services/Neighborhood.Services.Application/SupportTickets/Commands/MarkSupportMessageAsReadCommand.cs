using MediatR;
using Neighborhood.Services.Application.SupportTickets.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.SupportTickets.Commands
{
    public class MarkSupportMessageAsReadCommand
     : IRequest<SupportMessageDto>
    {
        public int MessageId { get; set; }
    }
}
