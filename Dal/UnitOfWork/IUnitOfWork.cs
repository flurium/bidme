<<<<<<< Updated upstream
﻿namespace Dal.UnitOfWork
{
    public interface IUnitOfWork
    {
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
  public interface IUnitOfWork
  {
        public IOrderRepository OrderRepository { get; }
>>>>>>> Stashed changes
    }
}