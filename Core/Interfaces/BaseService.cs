using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tenant_service.Core.Interfaces
{
    public abstract class BaseService
    {
        protected object? Request { get; private set; }
        protected object? Model { get; private set; }

        /// <summary>
        /// Set request
        /// </summary>
        /// <param name="request"></param>
        public virtual void SetRequest(object request)
        {
            this.Request = request;
        }

        /// <summary>
        /// Set model for service
        /// </summary>
        /// <param name="model"></param>
        public virtual void SetModel(object model)
        {
            this.Model = model;
        }
    }
}