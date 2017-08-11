using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TotalDTO.Helpers
{
    public interface IQuantityDTO
    {
        decimal TotalQuantity { get; }
    }

    public abstract class QuantityDTO<TQuantityDetailDTO> : BaseWithDetailDTO<TQuantityDetailDTO>, IQuantityDTO
        where TQuantityDetailDTO : class, IQuantityDetailDTO
    {
        public virtual decimal TotalQuantity { get { return this.DtoDetails().Select(o => o.Quantity).Sum(); } }
        public virtual decimal TotalVolume { get { return this.DtoDetails().Select(o => o.Volume).Sum(); } }

        //public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    foreach (var result in base.Validate(validationContext)) { yield return result; }

        //    if (this.TotalQuantity != this.GetTotalQuantity()) yield return new ValidationResult("Lỗi tổng số lượng", new[] { "TotalQuantity" });
        //    if (this.TotalVolume != this.GetTotalVolume()) yield return new ValidationResult("Lỗi tổng số lượng", new[] { "TotalVolume" });
        //}
    }
}
