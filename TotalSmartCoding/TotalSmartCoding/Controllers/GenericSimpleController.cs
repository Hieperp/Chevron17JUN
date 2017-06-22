using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using TotalDTO;
using TotalModel;

using TotalCore.Services;
using TotalSmartCoding.ViewModels.Helpers;
using TotalSmartCoding.Builders;


namespace TotalSmartCoding.Controllers
{
    public abstract class GenericSimpleController<TEntity, TDto, TPrimitiveDto, TSimpleViewModel> : BaseController

        where TEntity : class, IPrimitiveEntity, IBaseEntity, new()
        where TDto : class, TPrimitiveDto
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TSimpleViewModel : TDto, ISimpleViewModel, new() //Note: constraints [TSimpleViewModel : TDto] and also [TViewDetailViewModel : TDto  -> in GenericViewDetailController]: is required for this.genericService.Editable(TDto) only!!! If there is any reason need to remove this constraints, just consider for this.genericService.Editable(TDto) only [should change this.genericService.Editable(TDto) only if needed -- means after remove this constraints]
    {


        protected readonly IGenericService<TEntity, TDto, TPrimitiveDto> GenericService;
        private readonly IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder;

        private bool isSimpleCreate;
        private bool isCreateWizard;




        public GenericSimpleController(IGenericService<TEntity, TDto, TPrimitiveDto> genericService, IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder)
            : this(genericService, viewModelSelectListBuilder, false, true)
        {
        }

        public GenericSimpleController(IGenericService<TEntity, TDto, TPrimitiveDto> genericService, IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder, bool isCreateWizard)
            : this(genericService, viewModelSelectListBuilder, isCreateWizard, false)
        {
        }

        public GenericSimpleController(IGenericService<TEntity, TDto, TPrimitiveDto> genericService, IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder, bool isCreateWizard, bool isSimpleCreate)
            : base(genericService)
        {
            this.GenericService = genericService;
            this.viewModelSelectListBuilder = viewModelSelectListBuilder;

            this.isCreateWizard = isCreateWizard;
            this.isSimpleCreate = isSimpleCreate;
        }



        protected virtual bool Save(TSimpleViewModel simpleViewModel)
        {
            try
            {
                //if (!ModelState.IsValid) return false;//Check Viewmodel IsValid

                //TDto dto = simpleViewModel;// Mapper.Map<TSimpleViewModel, TDto>(simpleViewModel);//Convert from Viewmodel to DTO

                //if (!this.TryValidateModel(dto)) return false;//Check DTO IsValid
                //else
                //    if (this.GenericService.Save(dto))
                //    {
                //        simpleViewModel.SetID(dto.GetID());
                //        this.BackupViewModelToSession(simpleViewModel);

                //        return true;
                //    }
                //    else
                //        return false;

                return true;
            }
            catch (Exception exception)
            {
                //ModelState.AddValidationErrors(exception);
                return false;
            }
        }


        protected virtual bool Delete(TSimpleViewModel simpleViewModel)
        {
            return true;
        }

    }
}
