<<<<<<< Updated upstream
﻿namespace Dal.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {
        }
=======
﻿using Dal.Repository;
using Dal.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.UnitOfWork
{
  public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IOrderRepository orderRepository)
        {
            OrderRepository = orderRepository;
        }

        public IOrderRepository OrderRepository { get; }
>>>>>>> Stashed changes
    }
}