using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Specifications
{
    public class NotificationsForUserSpecification : BaseSpecifications<Notification>
    {
        public NotificationsForUserSpecification(string userId)
           : base(n => n.UserId == userId)
        {
            AddOrderByDesc(n => n.CreatedAt);
        }
    }
}
