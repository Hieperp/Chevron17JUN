using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using TotalModel.Models;
using TotalCore.Repositories;



using TotalModel; //for Loading (09/07/2015) - let review and optimize Loading laster




namespace TotalDAL.Repositories
{
    public class GenericWithDetailRepository<TEntity, TEntityDetail> : GenericRepository<TEntity>, IGenericWithDetailRepository<TEntity, TEntityDetail>
        where TEntity : class, IAccessControlAttribute //IAccessControlAttribute: for Loading (09/07/2015) - let review and optimize Loading laster
        where TEntityDetail : class
    {
        private DbSet<TEntityDetail> modelDetailDbSet = null;

        public GenericWithDetailRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : this(totalSmartCodingEntities, null) { }

        public GenericWithDetailRepository(TotalSmartCodingEntities totalSmartCodingEntities, string functionNameEditable)
            : this(totalSmartCodingEntities, functionNameEditable, null) { }

        public GenericWithDetailRepository(TotalSmartCodingEntities totalSmartCodingEntities, string functionNameEditable, string functionNameApproved)
            : this(totalSmartCodingEntities, functionNameEditable, functionNameApproved, null) { }

        public GenericWithDetailRepository(TotalSmartCodingEntities totalSmartCodingEntities, string functionNameEditable, string functionNameApproved, string functionNameDeletable)
            : this(totalSmartCodingEntities, functionNameEditable, functionNameApproved, functionNameDeletable, null) { }

        public GenericWithDetailRepository(TotalSmartCodingEntities totalSmartCodingEntities, string functionNameEditable, string functionNameApproved, string functionNameDeletable, string functionNameVoidable)
            : base(totalSmartCodingEntities, functionNameEditable, functionNameApproved, functionNameDeletable, functionNameVoidable)
        {
            modelDetailDbSet = this.TotalSmartCodingEntities.Set<TEntityDetail>();
        }


        public virtual TEntityDetail RemoveDetail(TEntityDetail entityDetail)
        {
            return this.modelDetailDbSet.Remove(entityDetail);
        }

        public virtual IEnumerable<TEntityDetail> RemoveRangeDetail(IEnumerable<TEntityDetail> entityDetails)
        {
            return this.modelDetailDbSet.RemoveRange(entityDetails);
        }
    }
}
