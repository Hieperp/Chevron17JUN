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
    public class FillingLineAPIs
    {
        private readonly IFillingLineAPIRepository fillingLineAPIRepository;

        public FillingLineAPIs(IFillingLineAPIRepository fillingLineAPIRepository)
        {
            this.fillingLineAPIRepository = fillingLineAPIRepository;
        }


        public ICollection<FillingLineIndex> GetFillingLineIndexes()
        {
            return this.fillingLineAPIRepository.GetEntityIndexes<FillingLineIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate).ToList();
        }

        public IList<FillingLineBase> GetFillingLineBases()
        {
            return this.fillingLineAPIRepository.GetFillingLineBases();
        }

    }
}
