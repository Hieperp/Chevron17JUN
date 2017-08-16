using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;


using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Inventories;

using TotalCore.Repositories.Commons;
using TotalBase;

namespace TotalSmartCoding.Controllers.APIs.Commons
{
    public class BinLocationAPIs
    {
        private readonly IBinLocationAPIRepository binLocationAPIRepository;

        public BinLocationAPIs(IBinLocationAPIRepository binLocationAPIRepository)
        {
            this.binLocationAPIRepository = binLocationAPIRepository;
        }


        public ICollection<BinLocationIndex> GetBinLocationIndexes()
        {
            return this.binLocationAPIRepository.GetEntityIndexes<BinLocationIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate).ToList();
        }

        public IList<BinLocationBase> GetBinLocationBases()
        {
            return this.binLocationAPIRepository.GetBinLocationBases();
        }

    }
}
