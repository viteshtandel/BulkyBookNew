using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Dapper;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType objCoverType =new CoverType();
            if (id == null)
            {
                return View(objCoverType);
            }
            var idParameter=new DynamicParameters();
            idParameter.Add("@Id",id);
            objCoverType = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, idParameter);
            if (objCoverType == null)
            {
                return NotFound();
            }

            return View(objCoverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                var Parameter =new DynamicParameters();
                Parameter.Add("@Name",coverType.Name);
                if (coverType.Id == 0)
                {
                    _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Create,Parameter);
                }
                else
                {
                    Parameter.Add("@Id",coverType.Id);
                    _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Update,Parameter);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }
        #region Cover types  API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allCoverTypes = _unitOfWork.SP_Call.List<CoverType>(SD.Proc_CoverType_GetAll, null);
            return Json(new {data = allCoverTypes});
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var idParamater =new DynamicParameters();
            idParamater.Add("@Id",id);
            var objCoverType = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, idParamater);
            if (objCoverType == null)
            {
                return Json(new {sucess = false, message = "Error while deleting cover types."});
            }
            _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Delete,idParamater);
            _unitOfWork.Save();
            return Json(new {sucess = true, message = "Deleted Sucessfull"});
        }

        #endregion
    }
}
