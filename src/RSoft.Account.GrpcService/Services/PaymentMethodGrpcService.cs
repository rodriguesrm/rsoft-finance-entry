﻿using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using RSoft.Account.Contracts.Commands;
using RSoft.Account.Contracts.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RSoft.Account.GrpcService.Extensions;
using RSoft.Account.Grpc.PaymentMethod;

namespace RSoft.Account.GrpcService.Services
{

    /// <summary>
    /// PaymentMethod gRPC Service
    /// </summary>
    [Authorize]
    public class PaymentMethodGrpcService : PaymentMethod.PaymentMethodBase
    {

        #region Local objects/variables

        private readonly ILogger<PaymentMethodGrpcService> _logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new PaymentMethod gRPC Service
        /// </summary>
        /// <param name="logger">Logger object</param>
        public PaymentMethodGrpcService(ILogger<PaymentMethodGrpcService> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Create a new PaymentMethod
        /// </summary>
        /// <param name="request">PaymentMethod request data</param>
        /// <param name="context">Server call context object</param>
        public override async Task<CreatePaymentMethodReply> CreatePaymentMethod(CreatePaymentMethodRequest request, ServerCallContext context)
            => await GrpcServiceHelpers.SendCommand<CreatePaymentMethodReply, CreatePaymentMethodCommand, Guid?>
            (
                nameof(CreatePaymentMethod), 
                () => new(request.Name, request.PaymentType),
                (reply, result) => reply.Id = result.Response.ToString(),
                logger: _logger
            );

        /// <summary>
        /// Update a existing PaymentMethod
        /// </summary>
        /// <param name="request">PaymentMethod request data</param>
        /// <param name="context">Server call context object</param>
        public override async Task<UpdatePaymentMethodReply> UpdatePaymentMethod(UpdatePaymentMethodRequest request, ServerCallContext context)
            => await GrpcServiceHelpers.SendCommand<UpdatePaymentMethodReply, UpdatePaymentMethodCommand, bool>
            (
                nameof(UpdatePaymentMethod),
                () => new(new Guid(request.Id), request.Name, request.PaymentType),
                logger: _logger
            );

        /// <summary>
        /// Enable a PaymentMethod
        /// </summary>
        /// <param name="request">PaymentMethod request data</param>
        /// <param name="context">Server call context object</param>
        public override async Task<ChangeStatusPaymentMethodReply> EnablePaymentMethod(ChangeStatusPaymentMethodRequest request, ServerCallContext context)
            => await GrpcServiceHelpers.SendCommand<ChangeStatusPaymentMethodReply, ChangeStatusPaymentMethodCommand, bool>
            (
                nameof(EnablePaymentMethod),
                () => new(new Guid(request.Id), true),
                logger: _logger
            );

        /// <summary>
        /// Disable a PaymentMethod
        /// </summary>
        /// <param name="request">PaymentMethod request data</param>
        /// <param name="context">Server call context object</param>
        public override async Task<ChangeStatusPaymentMethodReply> DisablePaymentMethod(ChangeStatusPaymentMethodRequest request, ServerCallContext context)
            => await GrpcServiceHelpers.SendCommand<ChangeStatusPaymentMethodReply, ChangeStatusPaymentMethodCommand, bool>
            (
                nameof(DisablePaymentMethod),
                () => new(new Guid(request.Id), false),
                logger: _logger
            );

        /// <summary>
        /// Get PaymentMethod by id
        /// </summary>
        /// <param name="request">PaymentMethod request data</param>
        /// <param name="context">Server call context object</param>
        public override async Task<GetPaymentMethodReply> GetPaymentMethod(GetPaymentMethodRequest request, ServerCallContext context)
            => await GrpcServiceHelpers.SendCommand<GetPaymentMethodReply, GetPaymentMethodByIdCommand, PaymentMethodDto>
            (
                nameof(GetPaymentMethod),
                () => new(new Guid(request.Id)),
                (reply, result) => reply.Data = result.Response.Map(),
                logger: _logger
            );

        /// <summary>
        /// List all categories
        /// </summary>
        /// <param name="request">PaymentMethod request data</param>
        /// <param name="context">Server call context object</param>
        public override async Task<ListPaymentMethodReply> ListPaymentMethod(ListPaymentMethodRequest request, ServerCallContext context)
            => await GrpcServiceHelpers.SendCommand<ListPaymentMethodReply, ListPaymentMethodCommand, IEnumerable<PaymentMethodDto>>
            (
                nameof(ListPaymentMethod),
                () => new(),
                (reply, result) => reply.Data.Add(result.Response.Map()),
                logger: _logger
            );

        #endregion

    }
}
