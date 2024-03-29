﻿using MediatR;
using Microsoft.Extensions.Logging;
using RSoft.Entry.Contracts.Commands;
using RSoft.Entry.Core.Entities;
using RSoft.Entry.Core.Ports;
using RSoft.Lib.Design.Application.Commands;
using RSoft.Lib.Design.Infra.Data;
using System.Threading;
using System.Threading.Tasks;
using RSoft.Lib.Design.Application.Handlers;
using MassTransit;
using RSoft.Finance.Contracts.Events;

namespace RSoft.Entry.Application.Handlers
{

    /// <summary>
    /// Create category command handler
    /// </summary>
    public class UpdateCategoryCommandHandler : UpdateCommandHandlerBase<UpdateCategoryCommand, bool, Category>, IRequestHandler<UpdateCategoryCommand, CommandResult<bool>>
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
        /// <param name="uow">Unit of work controller object</param>
        /// <param name="logger">Logger object</param>
        /// <param name="bus">Messaging bus control</param>
        public UpdateCategoryCommandHandler(ICategoryDomainService categoryDomainService, IUnitOfWork uow, ILogger<CreateCategoryCommandHandler> logger, IBusControl bus) : base(logger)
        {
            _categoryDomainService = categoryDomainService;
            _uow = uow;
            _bus = bus;
        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override async Task<Category> GetEntityByKeyAsync(UpdateCategoryCommand request, CancellationToken cancellationToken)
            => await _categoryDomainService.GetByKeyAsync(request.Id, cancellationToken);

        ///<inheritdoc/>
        protected override void PrepareEntity(UpdateCategoryCommand request, Category entity)
        {
            entity.Name = request.Name;
        }

        ///<inheritdoc/>
        protected override async Task<bool> SaveAsync(Category entity, CancellationToken cancellationToken)
        {
            _ = _categoryDomainService.Update(entity.Id, entity);
            _ = await _uow.SaveChangesAsync(cancellationToken);
            await _bus.Publish(new CategoryChangedEvent(entity.Id, entity.Name), cancellationToken);
            return true;
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Command handler
        /// </summary>
        /// <param name="request">Request command data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task<CommandResult<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            => await RunHandler(request, cancellationToken);

        #endregion

    }
}
