using System;
using System.Net;
using System.Linq;


using AutoMapper;

using TotalCore.Extensions;
using TotalBase.Enums;
using TotalModel;
using TotalDTO;
using TotalCore.Services;

using TotalSmartCoding.Builders;
using TotalSmartCoding.ViewModels.Helpers;
using TotalDTO.Commons;


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
        private readonly TSimpleViewModel simpleViewModel;

        

        private bool isSimpleCreate;
        private bool isCreateWizard;




        public GenericSimpleController(IGenericService<TEntity, TDto, TPrimitiveDto> genericService, IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder, TSimpleViewModel simpleViewModel)
            : this(genericService, viewModelSelectListBuilder, simpleViewModel, false, true)
        {
        }

        public GenericSimpleController(IGenericService<TEntity, TDto, TPrimitiveDto> genericService, IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder, TSimpleViewModel simpleViewModel, bool isCreateWizard)
            : this(genericService, viewModelSelectListBuilder, simpleViewModel, isCreateWizard, false)
        {
        }

        public GenericSimpleController(IGenericService<TEntity, TDto, TPrimitiveDto> genericService, IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder, TSimpleViewModel simpleViewModel, bool isCreateWizard, bool isSimpleCreate)
            : base(genericService, simpleViewModel)
        {
            this.GenericService = genericService;
            this.viewModelSelectListBuilder = viewModelSelectListBuilder;

            this.simpleViewModel = simpleViewModel; //New object vs MVC
            //--NGAY SAU KHI INIT simpleViewModel => SHOULD CHECK FOR Newable???? => to bind to toolstrip (because: many time: the index list is empty => not select the first row in index list)


            this.isCreateWizard = isCreateWizard;
            this.isSimpleCreate = isSimpleCreate;
        }

        public virtual void Index(int? id)
        {
            //***********ViewBag.SelectedEntityID = id == null ? -1 : (int)id;
            //***********ViewBag.ShowDiscount = this.GenericService.GetShowDiscount();

            base.AddRequireJsOptions();
            //-----return View();
        }









        public override void Open(int? id)
        {
            if (!this.AccessLevelAuthorize(GlobalEnums.AccessLevel.Readable)) throw new System.ArgumentException("Lỗi phân quyền", "Không có quyền truy cập dữ liệu");

            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable, false, false, true);
            if (simpleViewModel == null) new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            base.AddRequireJsOptions();
            //-----return View(simpleViewModel);
        }




        /// <summary>
        /// Create NEW from an empty ViewModel object
        /// </summary>
        /// <returns></returns>



        public override void Create()
        {
            if (!this.AccessLevelAuthorize()) throw new System.ArgumentException("Lỗi phân quyền", "Không có quyền truy cập dữ liệu");

            if (!this.isSimpleCreate) new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            base.AddRequireJsOptions();








            this.simpleViewModel.StopTracking();

            this.LastID = this.simpleViewModel.GetID();
            this.simpleViewModel.ApplyDefaults(); //NEED TO CALL this.simpleViewModel.ApplyDefaults(), INTEAD OF call new TSimpleViewModel() AS MVC, BECAUSE THE VIEW CONTROL IS BINDING TO this.simpleViewModel
            //this.Call int of base object
            //Check default
            //Implement all property for property change


            this.TailorViewModel(this.InitViewModelByPrior(this.InitViewModelByDefault(this.simpleViewModel))); //IN MVC: SIMPLE: Need to call new TSimpleViewModel() to ensure construct TSimpleViewModel object using Constructor!

            this.simpleViewModel.StartTracking();
            this.simpleViewModel.Reset();
            this.Reset();

            //return View();
        }




        public virtual void Create(TSimpleViewModel simpleViewModel)
        {
            if (!this.isSimpleCreate) new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            base.AddRequireJsOptions();

            if ((simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Save || simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Closed || simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Create) && this.Save())
                RedirectAfterSave(simpleViewModel);
            else
            {
                //-----return View(this.TailorViewModel(simpleViewModel));
            }
        }







        /// <summary>
        /// Create NEW by show a CreateWizard dialog, where user HAVE TO SELECT A RELATIVE OBJECT to INITIALIZE ViewModel, then SUBMIT the ViewModel
        /// </summary>
        /// <returns></returns>



        public virtual void CreateWizard()
        {
            if (this.AccessLevelAuthorize()) throw new System.ArgumentException("Lỗi phân quyền", "Không có quyền truy cập dữ liệu");

            if (!this.isCreateWizard) new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            base.AddRequireJsOptions();

            //-----return View(this.TailorViewModel(this.InitViewModelByDefault(new TSimpleViewModel())));
        }

        /// <summary>
        /// The SUBMITTED ViewModel will be pass to EDIT VIEW to SHOW for editing data
        /// </summary>
        /// <param name="simpleViewModel"></param>
        /// <returns></returns>



        public virtual void CreateWizard(TSimpleViewModel simpleViewModel)
        {
            if (!this.isCreateWizard) new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ModelState.Clear(); //Add this on 10-Dec-2016: When using Required attribute for a Nullable<System.DateTime>, the Kendo().DateTimePickerFor can not pass when submit. Don't know why!!!
            //This ModelState.Clear(): may be a very good idea -may be very very good :), because: we don't need to pre check model error here (Note ... Note: right here then, we always to return Edit view to input data, the: the model will be check by edit view)

            base.AddRequireJsOptions();

            //-----return View("Edit", this.TailorViewModel(this.DecorateViewModel(this.InitViewModelByPrior(simpleViewModel))));
        }









        public override void Edit(int? id)
        {
            base.Edit(id);

            if (!this.AccessLevelAuthorize(GlobalEnums.AccessLevel.Readable)) throw new System.ArgumentException("Lỗi phân quyền", "Không có quyền truy cập dữ liệu");

            this.StopTracking();

            this.LastID = this.simpleViewModel.GetID();
            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable);
            if (simpleViewModel == null) new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            this.StartTracking();
            this.Reset();

            base.AddRequireJsOptions();

            //-----return View(simpleViewModel);
        }


        /// <summary>
        /// Use SubmitTypeOption to DISTINGUISH two type of submit:
        ///     1.SubmitTypeOption.Save: Submit by EDIT VIEW to SAVE ViewModel
        ///     2.SubmitTypeOption.Popup: Submit by CreateWizard dialog, where user BE ABLE TO CHANGE A RELATIVE OBJECT to current ViewModel, then SUBMIT the ViewModel (Note on: 07/07/2015: for example: User may want to change the current edited purchase invoice to adapt to another purchase order - THE RELATIVE OBJECT)
        /// </summary>
        /// <param name="simpleViewModel"></param>
        /// <returns></returns>



        public virtual void Edit(TSimpleViewModel simpleViewModel)
        {
            base.AddRequireJsOptions();

            if ((simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Save || simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Closed || simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Create) && this.Save())
                RedirectAfterSave(simpleViewModel);
            else
            {
                if (simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Popup) this.DecorateViewModel(simpleViewModel);
                //-----return View(this.TailorViewModel(simpleViewModel));
            }
        }





        public virtual void RedirectAfterSave(TSimpleViewModel simpleViewModel)
        {
            if (simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Create)
                if (this.isSimpleCreate)
                    RedirectToAction("Create");
                else
                {
                    TSimpleViewModel createWizardViewModel = InitViewModelByCopy(simpleViewModel);
                    if (createWizardViewModel == null) new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    CreateWizard(createWizardViewModel);
                }
            else
            {
                if (simpleViewModel.PrintAfterClosedSubmit) this.TempData["PrintOptionID"] = simpleViewModel.PrintOptionID;
                RedirectToAction(simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Save ? "Edit" : simpleViewModel.PrintAfterClosedSubmit ? "Print" : "Index", new { id = simpleViewModel.GetID() });
            }
        }



        public override void CancelDirty(bool withRestore)
        {
            base.CancelDirty(withRestore);
            if (withRestore || this.simpleViewModel.GetID() <= 0)
                this.Edit(this.LastID);
        }



        #region Approve/ UnApprove
        public virtual TSimpleViewModel Approve(int? id)
        {
            if (!this.AccessLevelAuthorize(GlobalEnums.AccessLevel.Readable)) throw new System.ArgumentException("Lỗi phân quyền", "Không có quyền truy cập dữ liệu");
            base.AddRequireJsOptions();
            
            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable, true);
            if (simpleViewModel == null) throw new System.ArgumentException("Lỗi", "Dữ liệu không tồn tại hoặc không có quyền truy cập.");

            if (!simpleViewModel.Approved)
                if (this.GenericService.GetApprovalPermitted(simpleViewModel.OrganizationalUnitID))
                    simpleViewModel.Approvable = this.GenericService.Approvable(simpleViewModel);
                else //USER DON'T HAVE PERMISSION TO DO
                    throw new System.ArgumentException("Lỗi", "Không có quyền truy cập.");

            if (simpleViewModel.Approved)
                if (this.GenericService.GetUnApprovalPermitted(simpleViewModel.OrganizationalUnitID))
                    simpleViewModel.UnApprovable = this.GenericService.UnApprovable(simpleViewModel);
                else //USER DON'T HAVE PERMISSION TO DO
                    throw new System.ArgumentException("Lỗi", "Không có quyền truy cập.");

            return simpleViewModel;
        }


        public virtual bool ApproveConfirmed(TSimpleViewModel simpleViewModel)
        {
            return this.GenericService.ToggleApproved(simpleViewModel);





            try
            {
                if (this.GenericService.ToggleApproved(simpleViewModel))
                    RedirectToAction("Index");
                else
                    throw new System.ArgumentException("Lỗi vô hiệu dữ liệu", "Dữ liệu này không thể vô hiệu.");
            }
            catch (Exception exception)
            {
                //***********ModelState.AddValidationErrors(exception);
                RedirectToAction("Approve", simpleViewModel.GetID());
            }
        }


        #endregion Approve/ UnApprove







        public override bool Delete(int id)
        {
            if (!this.AccessLevelAuthorize()) throw new System.ArgumentException("Lỗi phân quyền", "Không có quyền truy cập dữ liệu");
            return this.GenericService.Delete(id);



            //TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Editable, true);
            //if (simpleViewModel == null) new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //base.AddRequireJsOptions();

            //-----return View(simpleViewModel);
        }



        public virtual void DeleteConfirmed(int id)
        {
            try
            {
                if (this.GenericService.Delete(id))
                    RedirectToAction("Index");
                else
                    throw new System.ArgumentException("Lỗi xóa dữ liệu", "Dữ liệu này không thể xóa được.");

            }
            catch (Exception exception)
            {
                //***********ModelState.AddValidationErrors(exception);
                RedirectToAction("Delete", id);
            }
        }









        public virtual void Alter(int? id)
        {
            if (this.AccessLevelAuthorize()) throw new System.ArgumentException("Lỗi phân quyền", "Không có quyền truy cập dữ liệu");

            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Editable, false, true);
            if (simpleViewModel == null) new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            base.AddRequireJsOptions();

            //-----return View(simpleViewModel);
        }




        public virtual void AlterConfirmed(TSimpleViewModel simpleViewModel)
        {
            try
            {
                if (this.GenericService.Alter(simpleViewModel))
                    RedirectToAction("Index");
                else
                    throw new System.ArgumentException("Lỗi vô hiệu dữ liệu", "Dữ liệu này không thể vô hiệu.");

            }
            catch (Exception exception)
            {
                //***********ModelState.AddValidationErrors(exception);
                //-----return View("Alter", this.TailorViewModel(simpleViewModel, false, true));
                //RedirectToAction("Alter", simpleViewModel.GetID());
            }
        }











        #region Void/ UnVoid



        public virtual void Void(int? id)
        {
            if (this.AccessLevelAuthorize(GlobalEnums.AccessLevel.Readable)) throw new System.ArgumentException("Lỗi phân quyền", "Không có quyền truy cập dữ liệu");

            base.AddRequireJsOptions();

            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable, true);
            if (simpleViewModel == null) new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!simpleViewModel.InActive)
                if (this.GenericService.GetVoidablePermitted(simpleViewModel.OrganizationalUnitID))
                {
                    simpleViewModel.Voidable = this.GenericService.Voidable(simpleViewModel);
                    //********RequireJsOptions.Add("Voidable", simpleViewModel.Voidable, RequireJsOptionsScope.Page);
                }
                else //USER DON'T HAVE PERMISSION TO DO
                    new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            if (simpleViewModel.InActive)
                if (this.GenericService.GetUnVoidablePermitted(simpleViewModel.OrganizationalUnitID))
                    simpleViewModel.UnVoidable = this.GenericService.UnVoidable(simpleViewModel);
                else //USER DON'T HAVE PERMISSION TO DO
                    new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            //-----return View(simpleViewModel);
        }


        public virtual void VoidConfirmed(TSimpleViewModel simpleViewModel)
        {
            try
            {
                if (simpleViewModel.VoidTypeID == null || simpleViewModel.VoidTypeID <= 0) throw new System.ArgumentException("Lỗi hủy dữ liệu", "Vui lòng nhập lý do hủy đơn hàng.");

                if (this.GenericService.ToggleVoid(simpleViewModel))
                    RedirectToAction("Index");
                else
                    throw new System.ArgumentException("Lỗi duyệt dữ liệu", "Dữ liệu này không thể duyệt được.");
            }
            catch (Exception exception)
            {
                //***********ModelState.AddValidationErrors(exception);
                RedirectToAction("Void", simpleViewModel.GetID());
            }
        }


        #endregion Void/ UnVoid








        protected virtual TEntity GetEntityAndCheckAccessLevel(int? id, GlobalEnums.AccessLevel accessLevel)
        {
            TEntity entity;
            if (id == null || (entity = this.GenericService.GetByID((int)id)) == null) return null;

            if (this.GenericService.GetAccessLevel(entity.OrganizationalUnitID) < accessLevel) return null;

            return entity;
        }


        public override bool Save()
        {
            try
            {
                if (!base.Save()) return false;

                if (!ModelState.IsValid) return false;//Check Viewmodel IsValid

                TDto dto = simpleViewModel;// Mapper.Map<TSimpleViewModel, TDto>(simpleViewModel);//Convert from Viewmodel to DTO

                if (!this.TryValidateModel(dto)) return false;//Check DTO IsValid
                else
                    if (this.GenericService.Save(dto))
                    {
                        simpleViewModel.SetID(dto.GetID());
                        this.Edit(simpleViewModel.GetID()); //WINFORM: RELOAD AFTER SAVE (IN MVC: WE REDIRECT TO NEW ACTION/ OR RELOAD CURRENT VIEW AGIAN => THIS WILL RELOAD AUTOMATICALLY AFTER SAVE SUCCESSFULY)

                        this.BackupViewModelToSession(simpleViewModel);

                        return true;
                    }
                    else
                        return false;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        protected bool TryValidateModel(TDto dto)
        {
            return true;
        }









        protected virtual TSimpleViewModel MapEntityToViewModel(TEntity entity)
        {
            //TSimpleViewModel simpleViewModel = Mapper.Map<TSimpleViewModel>(entity);
            //return simpleViewModel;

            this.simpleViewModel.StopTracking();
            Mapper.Map<TEntity, TSimpleViewModel>(entity, this.simpleViewModel);
            this.simpleViewModel.StartTracking();
            this.simpleViewModel.Reset();

            return this.simpleViewModel;
        }



        protected virtual TSimpleViewModel InitViewModelByPrior(TSimpleViewModel simpleViewModel)
        {
            return simpleViewModel;
        }


        /// <summary>
        /// Init new viewmodel by set default value. Default, this procedure does nothing and just return the passing parameter simpleViewModel.
        /// Each module should override this InitViewModelByDefault to init its's viewmodel accordingly, if needed
        /// </summary>
        /// <param name="simpleViewModel"></param>
        /// <returns></returns>
        protected virtual TSimpleViewModel InitViewModelByDefault(TSimpleViewModel simpleViewModel)
        {
            return simpleViewModel;
        }


        /// <summary>
        /// Backup ViewModel to HttpContext.Session for reuse later
        /// </summary>
        /// <param name="simpleViewModel"></param>
        protected virtual void BackupViewModelToSession(TSimpleViewModel simpleViewModel) { }


        /// <summary>
        /// Init new viewmodel by copy from current viewmodel object. Default, this procedure does nothing and return null.
        /// Each module should override this InitViewModelByCopy to init its's viewmodel accordingly, if needed
        /// </summary>
        /// <param name="simpleViewModel"></param>
        /// <returns></returns>
        protected virtual TSimpleViewModel InitViewModelByCopy(TSimpleViewModel simpleViewModel)
        {
            return null;
        }




        private TSimpleViewModel BuildViewModel(TEntity entity, bool forDelete, bool forAlter, bool forOpen)
        {
            return this.TailorViewModel(this.DecorateViewModel(this.MapEntityToViewModel(entity)), forDelete, forAlter, forOpen);
        }

        protected virtual TSimpleViewModel TailorViewModel(TSimpleViewModel simpleViewModel)
        {
            return this.TailorViewModel(simpleViewModel, false);
        }

        protected virtual TSimpleViewModel TailorViewModel(TSimpleViewModel simpleViewModel, bool forDelete)
        {
            return this.TailorViewModel(simpleViewModel, forDelete, false);
        }

        protected virtual TSimpleViewModel TailorViewModel(TSimpleViewModel simpleViewModel, bool forDelete, bool forAlter)
        {
            return this.TailorViewModel(simpleViewModel, forDelete, forAlter, false);
        }

        protected virtual TSimpleViewModel TailorViewModel(TSimpleViewModel simpleViewModel, bool forDelete, bool forAlter, bool forOpen)
        {
            simpleViewModel.Newable = this.AccessLevelAuthorize();

            if (!forOpen)
            {
                if (!forDelete)//Be caution: the value of simpleViewModel.Editable should be SET EVERY TIME THE simpleViewModel LOADED! This means: if it HAVEN'T SET YET, the default value of simpleViewModel.Editable is FALSE               (THE CONDITIONAL CLAUSE: if (!forDelete) MEAN: WHEN SHOW VIEW FOR DELETE, NO NEED TO CHECK Editable => Editable SHOULD BE FALSE)
                    simpleViewModel.Editable = this.GenericService.Editable(simpleViewModel);

                //if (forDelete) --- WINFORM: NEED TO BIND Deletable TO TOOLSTRIP => ALWAYS CHECK FOR Deletable // || simpleViewModel is ServiceContractViewModel                    //WHEN forDelete, IT SHOULD BE CHECK FOR Deletable ATTRIBUTE, SURELY.          BUT, WHEN OPEN VIEW FOR EDIT, NOW: ONLY VIEW ServiceContract NEED TO USE Deletable ATTRIBUTE ONLY. SO, THIS CODE IS CORRECT FOR NOW, BUT LATER, IF THERE IS MORE VIEWS NEED THIS Deletable ATTRIBUTE, THIS CODE SHOULD MODIFY MORE GENERIC!!!
                simpleViewModel.Deletable = this.GenericService.Deletable(simpleViewModel);

                if (forAlter)//NOW THIS GlobalLocked attribute ONLY be considered WHEN ALTER ACTION to USE IN ALTER VIEW: to ALLOW or NOT ALTER.
                    simpleViewModel.GlobalLocked = this.GenericService.GlobalLocked(simpleViewModel);
            }

            simpleViewModel.ShowDiscount = this.GetShowDiscount(simpleViewModel);

            //********RequireJsOptions.Add("Editable", simpleViewModel.Editable, RequireJsOptionsScope.Page);
            //********RequireJsOptions.Add("Deletable", simpleViewModel.Deletable, RequireJsOptionsScope.Page);

            simpleViewModel.UserID = this.GenericService.UserID; //CAU LENH NAY TAM THOI DUOC SU DUNG DE SORT USER DROPDWONLIST. SAU NAY NEN LAM CACH KHAC, CACH NAY KHONG HAY

            this.viewModelSelectListBuilder.BuildSelectLists(simpleViewModel); //Buil select list for dropdown box using IEnumerable<SelectListItem> (using for short data list only). For the long list, it should use Kendo automplete instead.

            return simpleViewModel;
        }

        protected virtual TSimpleViewModel DecorateViewModel(TSimpleViewModel simpleViewModel)
        {
            return simpleViewModel;
        }

        protected virtual bool GetShowDiscount(TSimpleViewModel simpleViewModel)
        {
            return this.GenericService.GetShowDiscount();
        }

        #region GetViewModel
        /// <summary>
        /// There are serveral times have to build ViewModel from Entity, by the same steps
        /// This is the reason to have GetViewModel. This is not for any purpose. This GetViewModel may change or ormit if needed
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected TSimpleViewModel GetViewModel(int? id, GlobalEnums.AccessLevel accessLevel)
        {
            return this.GetViewModel(id, accessLevel, false);
        }

        protected TSimpleViewModel GetViewModel(int? id, GlobalEnums.AccessLevel accessLevel, bool forDelete)
        {
            return this.GetViewModel(id, accessLevel, forDelete, false);
        }

        protected TSimpleViewModel GetViewModel(int? id, GlobalEnums.AccessLevel accessLevel, bool forDelete, bool forAlter)
        {
            return this.GetViewModel(id, accessLevel, forDelete, forAlter, false);
        }

        protected TSimpleViewModel GetViewModel(int? id, GlobalEnums.AccessLevel accessLevel, bool forDelete, bool forAlter, bool forOpen)
        {
            TEntity entity = this.GetEntityAndCheckAccessLevel(id, accessLevel);
            if (entity == null) return null;

            return this.BuildViewModel(entity, forDelete, forAlter, forOpen);
        }
        #endregion GetViewModel





        public void Print(int? id)
        {
            base.AddRequireJsOptions();
            //-----return View(InitPrintViewModel(id));
        }

        protected virtual PrintViewModel InitPrintViewModel(int? id)
        {
            PrintViewModel printViewModel = new PrintViewModel() { Id = id != null ? (int)id : 0 };
            if (this.TempData["PrintOptionID"] != null)
                printViewModel.PrintOptionID = (int)this.TempData["PrintOptionID"];
            return printViewModel;
        }

        //Create/CreateWizard: by Authorize Attribute (Editable)
        //Edit: by Authorize Attribute (Readonly?) -> then: Get Entity by ID (Need to check editable ACCESS for entity) -> Check Ediatable of Entity (by service) -> Add FLAG STATUS for Editable/ Readonly -> Set View Status!
        //Index: by Authorize Attribute (Readonly) -> Then load entity list by permission check
        //Save: Check for Ediable for entity (Should check by servicelayer only?)

    }

}