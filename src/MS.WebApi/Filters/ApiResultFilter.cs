using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MS.WebCore.Core;
using System;
using System.Net;

namespace MS.WebApi.Filters
{
    /// <summary>
    /// 给api返回结果包一层状态码
    /// </summary>
    public class ApiResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result != null)
            {
                if (context.Result is ObjectResult objectResult)
                {
                    if (objectResult.DeclaredType is null) //返回的是IActionResult类型
                    {
                        context.Result = new JsonResult(new
                        {
                            status = objectResult.StatusCode,
                            data = objectResult.Value
                        });
                    }
                    else//返回的是string、List这种其他类型，此时没有statusCode，应尽量使用IActionResult类型
                    {
                        //0728增加判断请求是否执行成功(IsSucceed) 不成功返回状态码400
                        if (objectResult.Value is ExecuteResult executeResult)
                        {
                            if (executeResult.IsSucceed == false)
                            {
                                context.Result = new JsonResult(new
                                {
                                    status = HttpStatusCode.BadRequest,
                                    data = objectResult.Value
                                });
                            }
                        }
                        else
                        {
                            context.Result = new JsonResult(new
                            {
                                status = 200,
                                data = objectResult.Value
                            });
                        }
                    }
                }
                else if (context.Result is EmptyResult)
                {
                    context.Result = new JsonResult(new
                    {
                        status = 200,
                        data = ""
                    });
                }
                else
                {
                    throw new Exception($"未经处理的Result类型：{ context.Result.GetType().Name}");
                }

            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}
