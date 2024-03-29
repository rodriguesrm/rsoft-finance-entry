﻿using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using RSoft.Entry.Contracts.Commands;
using RSoft.Entry.Core.Entities;
using RSoft.Entry.Core.Ports;
using RSoft.Finance.Contracts.Events;
using RSoft.Lib.Design.Application.Commands;
using RSoft.Lib.Design.Application.Handlers;
using RSoft.Lib.Design.Infra.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Entry.Application.Handlers
{

    /// <summary>
    /// Create category command handler
    /// </summary>
    public class CreateCategoryCommandHandler : CreateCommandHandlerBase<CreateCategoryCommand, Guid?, Category>, IRequestHandler<CreateCategoryCommand, CommandResult<Guid?>>
    {

        #region Local objects/variables

        private readonly IUnitOfWork _uow;
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly IBusControl _bus;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new handler instance
        /// </summary>
        /// <param name="categoryDomainService">Category domain service object</param>
        /// <param name="logger">Logger object</param>
        /// <param name="uow">Unit of work controller object</param>
        /// <param name="bus">Message bus control</param>
        public CreateCategoryCommandHandler(ICategoryDomainService categoryDomainService, ILogger<CreateCategoryCommandHandler> logger, IUnitOfWork uow, IBusControl bus) : base(logger)
        {
            _categoryDomainService = categoryDomainService;
            _uow = uow;
            _bus = bus;
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Command handler
        /// </summary>
        /// <param name="request">Request command data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task<CommandResult<Guid?>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            => await RunHandler(request, cancellationToken);

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override Category PrepareEntity(CreateCategoryCommand request)
            => new() { Name = request.Name };

        ///<inheritdoc/>
        protected override async Task<Guid?> SaveAsync(Category entity, CancellationToken cancellationToken)
        {
            entity = await _categoryDomainService.AddAsync(entity, cancellationToken);
            _ = await _uow.SaveChangesAsync(cancellationToken);
            await _bus.Publish(new CategoryCreatedEvent(entity.Id, entity.Name), cancellationToken);
            return entity.Id;
        }

        #endregion

    }
}
