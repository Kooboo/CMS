using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.IoC;

namespace Kooboo.Domain
{
    /// <summary>
    /// support for nested transaction context
    /// </summary>
    public class TransactionContext :IDisposable
    {
        public TransactionContext()
        {
            var transactionConext = ContextContainer.Current.Resolve<ITransactionContext>();
            if (transactionConext.IsStarted == false)
            {
                this.CurrentTransactionContext = transactionConext;
                this.CurrentTransactionContext.Begin();
            }
        }

        ITransactionContext CurrentTransactionContext
        {
            get;
            set;
        }

        bool HasCommitted
        {
            get;
            set;
        }


        public void Commit()
        {
            if (this.CurrentTransactionContext != null)
            {
                //this.CurrentTransactionContext.Commit();
                this.HasCommitted = true;
            }
        }

        public void Rollback()
        {
            if (this.CurrentTransactionContext != null)
            {
                this.CurrentTransactionContext.Rollback();
            }
        }

  
        #region Dispose


        ~TransactionContext()
        {
            Dispose(false); 
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //free managed objects
                if (this.HasCommitted)
                {
                    if (this.CurrentTransactionContext != null)
                    {
                        try
                        {
                            this.CurrentTransactionContext.Commit();
                        }
                        catch
                        {
                            this.CurrentTransactionContext.Rollback();
                            throw;
                        }
                        finally
                        {
                            this.CurrentTransactionContext = null;//free context
                            this.HasCommitted = false;//reset state
                        }
                    }
                }
            }

            //free unmanaged objectss
         
            if (disposing)
            {
                //remove me from finalization list
                GC.SuppressFinalize(this);   
            }
        }

 
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
