using apprecipes.Config;
using apprecipes.DataAccess.Query;
using apprecipes.DataTransferObject.Object;
using apprecipes.DataTransferObject.OtherObject;
using apprecipes.Generic;
using apprecipes.ServerObjet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apprecipes.Controllers
{
    public class RecipeController : ControllerGeneric<SoRecipe>
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<SoRecipe>> PaginationByCategory(Guid idCategory, int pageNumber, int pageSize)
        {
            try
            {
                QRecipe qRecipe = new();
                _so.data.additional = new Pagination();
                (_so.data.listDto, _so.data.additional) = await qRecipe.GetRecipesByCategory(idCategory,pageNumber, pageSize);
                _so.message.Success();
            }
            catch (Exception ex)
            {
                _so.message.listMessage.Add("Ocurrió un error inesperado. Estamos trabajando para resolverlo.");
                _so.message.listMessage.Add("ERROR_EXCEPTION:" + ex.Message);
                _so.message.Error();
            }
            return _so;
        }
        
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoRecipe> TheMostLiked()
        {
            try
            {
                QRecipe qRecipe = new();
                _so.data.dto = qRecipe.TheMostLiked();
                _so.message.Success();
            }
            catch (Exception ex)
            {
                _so.message.listMessage.Add("Ocurrió un error inesperado. Estamos trabajando para resolverlo.");
                _so.message.listMessage.Add("ERROR_EXCEPTION:" + ex.Message);
                _so.message.Error();
            }
            return _so;
        }
        
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoRecipe> GetById( Guid id )
        {
            try
            {
                if (id == Guid.Empty || id.ToString() == "")
                {
                    _so.data.dto = null;
                    _so.message.listMessage.Add("Proporcione la ID de categoria válida.");
                    return _so;
                }
                
                QRecipe qRecipe = new();
                _so.data.dto = qRecipe.GetByIdIncludeImageVideo(id);
                if (_so.data.dto is not null)
                {
                    _so.message.Success();
                }
                else
                {
                    _so.data.dto = null;
                    _so.message.listMessage.Add("Receta no existente.");
                    _so.message.Error();
                }
            }
            catch (Exception ex)
            {
                _so.data.dto = null;
                _so.message.listMessage.Add("Ocurrió un error inesperado. Estamos trabajando para resolverlo.");
                _so.message.listMessage.Add("ERROR_EXCEPTION:" + ex.Message);
                _so.message.Error();
            }
            return _so;
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        [Route("[action]")]
        public ActionResult<SoRecipe> Create([FromBody] SoRecipe so)
        {
            try
            {
                Guid idUser = Guid.Parse(TokenUtils.GetUserIdFromAccessToken(Request.Headers["Authorization"].ToString()));
                _so.message = ValidateDto(so.data.dto, new List<string>() 
                {
                    nameof(so.data.dto.title),
                    nameof(so.data.dto.description),
                    nameof(so.data.dto.instruction),
                    nameof(so.data.dto.preparation),
                    nameof(so.data.dto.cooking),
                    nameof(so.data.dto.estimated),
                    nameof(so.data.dto.difficulty),
                });

                if (so.data.dto.idCategory == Guid.Empty)
                {
                    _so.message.listMessage.Add("Proporcione la ID de la Categoria.");
                }
                else
                {
                    QCategory qCategory = new QCategory();
                    if (!qCategory.ExistCategoryById(so.data.dto.idCategory))
                    {
                        _so.message.listMessage.Add("Proporcione una Categoria existente.");
                    }
                }
                
                if (so.data.dto.images.Count == 0)
                {
                    _so.message.listMessage.Add("Debe proporcionar al menos una imagen con URL válida.");
                }
                foreach (DtoImage image in so.data.dto.images)
                {
                    if (string.IsNullOrEmpty(image.url))
                    {
                        _so.message.listMessage.Add("Debe proporcionar al menos una imagen con URL válida.");
                        break;
                    }
                }
                
                if (_so.message.ExistsMessage())
                {
                    _so.data.dto = null;
                    _so.message.Error();
                    return _so;
                }

                QRecipe qRecipe = new();
                if (qRecipe.ExistByTitle(so.data.dto.title))
                {
                    _so.data.dto = null;
                    _so.message.Error();
                    _so.message.listMessage.Add("Este título para la receta ya esta registrada proporcione otro.");
                }
                
                if (qRecipe.CreateRecipe(so.data.dto, idUser) != 0)
                {
                    so.data.dto = null;
                    _so.message.listMessage.Add("Regsitro exitoso.");
                    so.message.Success();
                }
            }
            catch (Exception ex)
            {
                _so.data.dto = null;
                _so.message.listMessage.Add("Ocurrió un error inesperado. Estamos trabajando para resolverlo.");
                _so.message.listMessage.Add("ERROR_EXCEPTION:" + ex.Message);
                _so.message.Error();
            }
            return _so;
        }
        
        [Authorize(Roles="Admin")]
        [HttpPut]
        [Route("[action]")]
        public ActionResult<SoRecipe> Update([FromBody] SoRecipe so)
        {
            try
            {
                Guid idUser = Guid.Parse(TokenUtils.GetUserIdFromAccessToken(Request.Headers["Authorization"].ToString()));
                _so.message = ValidateDto(so.data.dto, new List<string>() 
                {
                    nameof(so.data.dto.title),
                    nameof(so.data.dto.description),
                    nameof(so.data.dto.instruction),
                    nameof(so.data.dto.preparation),
                    nameof(so.data.dto.cooking),
                    nameof(so.data.dto.estimated),
                    nameof(so.data.dto.difficulty),
                });
                
                QCategory qCategory = new QCategory();
                QRecipe qRecipe = new();
                QImage qImage = new QImage();
                QVideo qVideo = new QVideo();
                if (so.data.dto.id == Guid.Empty)
                {
                    _so.message.listMessage.Add("Proporcione la ID de la receta.");
                }
                else
                {
                    if (!qRecipe.ExistById(so.data.dto.id))
                    {
                        _so.message.listMessage.Add("Esta receta no existe seleccione una existente.");
                    }
                }
                
                if (so.data.dto.idCategory == Guid.Empty)
                {
                    _so.message.listMessage.Add("Proporcione la ID de la categoria.");
                }
                else
                {
                    if (!qCategory.ExistCategoryById(so.data.dto.idCategory))
                    {
                        _so.message.listMessage.Add("Proporcione una Categoria existente.");
                    }
                }
                
                if (so.data.dto.images.Count == 0)
                {
                    _so.message.listMessage.Add("Debe proporcionar al menos una imagen con URL válida.");
                }
                foreach (DtoImage image in so.data.dto.images)
                {
                    if (string.IsNullOrEmpty(image.url))
                    {
                        _so.message.listMessage.Add("Debe proporcionar al menos una imagen con URL válida.");
                        break;
                    }
                }
                
                if (_so.message.ExistsMessage())
                {
                    _so.data.dto = null;
                    _so.message.Error();
                    return _so;
                }
                
                DtoRecipe dtoRecipeTemp = qRecipe.GetByTitle(so.data.dto.title);
                if (qRecipe.GetByTitle(so.data.dto.title) is not null && 
                    dtoRecipeTemp.title == so.data.dto.title && dtoRecipeTemp.id != so.data.dto.id)
                {
                    _so.data.dto = null;
                    _so.message.Error();
                    _so.message.listMessage.Add("Este título para la receta ya esta registrada proporcione otro.");
                }

                foreach (DtoImage image in so.data.dto.images)
                {
                    DtoImage dtoImageTemp = qImage.GetImageByUrl(image.url);
                    DtoImage imagebyId = qImage.GetImageByIdRecipe(so.data.dto.id);
                    if (qImage.GetImageByUrl(image.url) is not null && dtoImageTemp.url == image.url && dtoImageTemp.id != imagebyId.id)
                    {
                        _so.data.dto = null;
                        _so.message.Error();
                        _so.message.listMessage.Add("Este URL de imagen para la receta ya esta registrada proporcione otro.");
                        return _so;
                    }
                }

                if (so.data.dto.videos.Count > 0)
                {
                    foreach (DtoVideo video in so.data.dto.videos)
                    {
                        DtoVideo dtoVideoTemp = qVideo.GetVideoByUrl(video.url);
                        DtoVideo videobyId = qVideo.GetVideoByIdRecipe(so.data.dto.id);
                        if (qVideo.GetVideoByUrl(video.url) is not null && dtoVideoTemp.url == video.url && dtoVideoTemp.id != videobyId.id)
                        {
                            _so.data.dto = null;
                            _so.message.Error();
                            _so.message.listMessage.Add("Este URL de video para la receta ya esta registrada proporcione otro.");
                            return _so;
                        }
                    }
                }
                
                if (qRecipe.UpdateRecipe(so.data.dto, idUser) != 0)
                {
                    so.data.dto = null;
                    _so.message.listMessage.Add("Actualización exitoso.");
                    so.message.Success();
                }
            }
            catch (Exception ex)
            {
                _so.data.dto = null;
                _so.message.listMessage.Add("Ocurrió un error inesperado. Estamos trabajando para resolverlo.");
                _so.message.listMessage.Add("ERROR_EXCEPTION:" + ex.Message);
                _so.message.Error();
            }
            return _so;
        }
    }
}
