﻿using MediatR;
using Microsoft.Extensions.Logging;
using RSoft.Entry.Contracts.Commands;
using RSoft.Entry.Contracts.Models;
using DomainEntry = RSoft.Entry.Core.Entities.Entry;
using RSoft.Entry.Core.Ports;
using RSoft.Lib.Design.Application.Commands;
using System.Threading;
using System.Threading.Tasks;
using RSoft.Entry.Application.Extensions;
using RSoft.Lib.Design.Application.Handlers;

namespace RSoft.Entry.Application.Handlers
{

    /// <summary>
    /// Get entry by id command handler
    /// </summary>
    public class GetEntryByIdCommandHandler : GetByKeyCommandHandlerBase<GetEntryByIdCommand, EntryDto, DomainEntry>, IRequestHandler<GetEntryByIdCommand, CommandResult<EntryDto>>
    {

        #region Local objects/variables

        private readonly IEntryDomainService _entryDomainService;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new command handler instance
        /// </summary>
        /// <param name="entryDomainService">Entry domain/core service</param>
        /// <param name="logger">Logger object</param>
        public GetEntryByIdCommandHandler(IEntryDomainService entryDomainService, ILogger<GetEntryByIdCommandHandler> logger) : base(logger)
        {
            _entryDomainService = entryDomainService;
        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override async Task<DomainEntry> GetEntityByKeyAsync(GetEntryByIdCommand request, CancellationToken cancellationToken)
            => await _entryDomainService.GetByKeyAsync(request.Id, cancellationToken);

        ///<inheritdoc/>
        protected override EntryDto MapEntity(DomainEntry entity)
            => entity.Map();

        #endregion

        #region Handlers

        /// <summary>
        /// Command handler
        /// </summary>
        /// <param name="request">Command request data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task<CommandResult<EntryDto>> Handle(GetEntryByIdCommand request, CancellationToken cancellationToken)
            => await RunHandler(request, cancellationToken);

        #endregion

    }
}
