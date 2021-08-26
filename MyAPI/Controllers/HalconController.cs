using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAPI.Database;
using HalconDotNet;
namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HalconController : ControllerBase
    {

        public HalconController(ILogger<HalconController> logger, DatabaseContext databaseContext)
        {
            this.Logger = logger;

            this.DatabaseContext = databaseContext;
        }

        private ILogger<HalconController> Logger { get; }
        private DatabaseContext DatabaseContext { get; }

        // /api/Halcon/
        [HttpGet]
        public IActionResult GetInfo()
        {
            try
            {
                //* CheckVersion Halcon
               String info = "This is Halcon Web API From SP vision ";   
               Logger.LogInformation($"Login {info} { DateTime.Now.ToString("HH:mm:ss")}");          
                return Ok($"Login {info} { DateTime.Now.ToString("HH:mm:ss")}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Halcon ReadImge {ex.Message}");
                
                return BadRequest();
            }
        }

        [HttpGet("version")]
        public IActionResult GetVersion()
        {
            try
            {
                //* CheckVersion Halcon
                HTuple Version;
                HOperatorSet.GetSystemInfo("version", out Version);
                Logger.LogInformation($"Halcon Version  {Version.ToString()}");              
                return Ok($"Halcon Version:  {Version.ToString()}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Halcon ReadImge {ex.Message}");
                return BadRequest();
            }
        }
        // hostlocal:5000/api/Halcon//value1?
        [HttpGet("SizeImage")]
        public string SizeImage([FromQuery]string[] pathImg)
        {
            HObject RawImage;
            HTuple Width, Height;
            HOperatorSet.ReadImage(out RawImage, pathImg);
            HOperatorSet.GetImageSize(RawImage, out Width, out Height);
            return pathImg[0].ToString() + "\n" + "Width : " + Width.ToString() + "\n" + "Height : " + Height.ToString();
        }
        //https://localhost:5001/api/Halcon/SizeImage?pathImg=/opt/halcon/examples/images/color/citrus_fruits_01.png

        [HttpGet("DL/Classify")]
        public string RunDL([FromQuery]string[] pathImg)
        {
            HObject RawImage;
            HTuple DLResult,Confiden;
            HOperatorSet.ReadImage(out RawImage, pathImg);
            HDevEngine Engine = new HDevEngine();
            Engine.SetProcedurePath("/home/sp/WebApiHalcon/WebAPI_Training/MyAPI/Models/dl_classification_workflow/res_dl_classification_workflow/");
            HDevProcedure procedure = new HDevProcedure("Run_Classification2");
            HDevProcedureCall ProcCall = new HDevProcedureCall(procedure);
            ProcCall.SetInputIconicParamObject("Image",RawImage);
            ProcCall.Execute();
            DLResult = ProcCall.GetOutputCtrlParamTuple("Result_name");
            Confiden = ProcCall.GetOutputCtrlParamTuple("Result_Confiden");
            return pathImg[0].ToString() + "\n" + "Class Name : " + DLResult.ToString() + "\n" + "Confiden : " + Confiden.ToString() ;
        }
        //https://localhost:5001/api/Halcon/DL/Classify?pathImg=/opt/halcon/examples/images/color/citrus_fruits_01.png
        // // hostlocal:5000/api/private/Product/value1?/value2
        // [HttpGet("Image")]
        // public Bitmap GetHObject()
        // {
           
        //     HTuple type, width, height;
        //     HImage patras = new HImage(@"C:\Users\Public\Documents\MVTec\HALCON-20.11-Steady\examples\images\bicycle\bicycle_01.png");
        //     IntPtr ptr = patras.GetImagePointer1(out type, out width, out height);
        //     Bitmap img = new Bitmap(width/4, height, width, PixelFormat.Format16bppGrayScale, ptr);
        //     return img ;
        // }
    }
}