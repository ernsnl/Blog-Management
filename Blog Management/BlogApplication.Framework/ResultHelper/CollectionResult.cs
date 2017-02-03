using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace BlogApplication.Framework.ResultHelper
{
    public class CollectionResult<TEntity> : Result
    {
        public List<TEntity> Data { get; set; }

        public long TotalDataCount { get; set; }

        public bool HasData {
            get
            {
                if (Data != null && Data.Count > 0)
                    return true;
                return false;
            }
        }

        public void SetData(int totalCount, List<TEntity> list)
        {
            this.SetData(Convert.ToInt64(totalCount), list);
        }

        public void SetData(long totalCount, List<TEntity> list)
        {
            this.TotalDataCount = totalCount;
            this.Data = list;
            this.HasFailed = false;
        }

        public void SetData(List<TEntity> list)
        {
            this.Data = list;
            this.HasFailed = false;
        }
    }
}