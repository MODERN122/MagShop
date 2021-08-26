﻿using ApplicationCore.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IBasketRepository:IAsyncRepository<Basket>
    {
        Task<Basket> AddBasketAsync(string userId, CancellationToken token = default);
        Task<Basket> FirstAsync(string userId, ISpecification<Basket> spec, CancellationToken token = default);
        Task<Basket> TransferBasketAsync(string anonymousUserId, CancellationToken token = default);
    }
}
