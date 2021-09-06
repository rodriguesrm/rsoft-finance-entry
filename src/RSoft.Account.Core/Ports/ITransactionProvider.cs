﻿using RSoft.Account.Core.Entities;
using RSoft.Lib.Design.Infra.Data;
using System;

namespace RSoft.Account.Core.Ports
{

    public interface ITransactionProvider : IRepositoryBase<Transaction, Guid>
    {
    }
}