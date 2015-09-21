using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VH.Model
{
    public sealed class ActionResult<TResult>
           where TResult : class
    {
        #region · Properties ·

        public TResult Data
        {
            get;
            set;
        }

        public ActionResultType Result
        {
            get;
            set;
        }

        #endregion

        #region · Constructors ·

        public ActionResult()
        {
        }

        #endregion
    }
}
