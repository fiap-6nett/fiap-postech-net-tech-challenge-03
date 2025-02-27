﻿using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

namespace Fiap.TechChallenge.Core.Contracts.Commands.CommandResults
{
    public class CriarContatoCommandResult : CommandResult
    {
        public Guid CorrelationId { get; set; }
        public DateTime DataRecebimento { get; set; } = DateTime.UtcNow;
    }
}
