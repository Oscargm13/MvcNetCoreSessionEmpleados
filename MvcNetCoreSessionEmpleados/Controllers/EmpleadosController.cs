using Microsoft.AspNetCore.Mvc;
using MvcNetCoreSessionEmpleados.Extensions;
using MvcNetCoreSessionEmpleados.Models;
using MvcNetCoreSessionEmpleados.Repositories;

namespace MvcNetCoreSessionEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;
        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }




        public async Task<IActionResult> SessionEmpleadosV5(int? idEmpleado)
        {
            if (idEmpleado != null)
            {
                //ALMACENAREMOS LO MINIMO QUE PODAMOS (int) 
                List<int> idsEmpleados;
                if (HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") == null)
                {
                    //NO EXISTE Y CREAMOS LA COLECCION 
                    idsEmpleados = new List<int>();
                }
                else
                {
                    //EXISTE Y RECUPERAMOS LA COLECCION 
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                idsEmpleados.Add(idEmpleado.Value);
                //REFRESCAMOS LOS DATOS DE SESSION 
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: "
                + idsEmpleados.Count;
            }
            //COMPROBAMOS SI TENEMOS IDS EN SESSION 
            List<int> ids = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            
                List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
                return View(empleados);
            
        }

        public async Task<IActionResult> SessionEmpleadosV4(int? idEmpleado)
        {
            if (idEmpleado != null)
            {
                //ALMACENAREMOS LO MINIMO QUE PODAMOS (int) 
                List<int> idsEmpleados;
                if (HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") == null)
                {
                    //NO EXISTE Y CREAMOS LA COLECCION 
                    idsEmpleados = new List<int>();
                }
                else
                {
                    //EXISTE Y RECUPERAMOS LA COLECCION 
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                idsEmpleados.Add(idEmpleado.Value);
                //REFRESCAMOS LOS DATOS DE SESSION 
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: "
                + idsEmpleados.Count;
            }
            //COMPROBAMOS SI TENEMOS IDS EN SESSION 
            List<int> ids = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (ids == null)
            {
                List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
                return View(empleados);
            }
            else
            {
                List<Empleado> empleados = await this.repo.GetEmpleadoNotSessionAsync(ids);
                return View(empleados);
            }
        }

        public async Task<IActionResult> EmpleadosAlmacenadosV4()
        {
            //DEBEMOS RECUPERAR LOS IDS DE EMPLEADOS QUE TENGAMOS 
            //EN SESSION 
            List<int> idsEmpleados =
        HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "No existen empleados almacenados "
                + " en Session.";
                return View();
            }
            else
            {
                List<Empleado> empleados =
                await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }
        }

        public async Task<IActionResult> EmpleadosAlmacenadosOk()
        {
            //DEBEMOS RECUPERAR LOS IDS DE EMPLEADOS QUE TENGAMOS 
            //EN SESSION 
            List<int> idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "No existen empleados almacenados "
                + " en Session.";
                return View();
            }
            else
            {
                List<Empleado> empleados =
                await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }
        }

        public async Task<IActionResult> SessionEmpleadosOk(int? idEmpleado)
        {
            if(idEmpleado != null)
            {
                //ALMACENAREMOS LO MINIMO QUE PODAMOS
                List<int> idsEmpleados;
                if(HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") == null)
                {
                    idsEmpleados = new List<int>();
                }
                else
                {
                    //EXISTE Y RECUPERAMOS LA COLLECION
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                idsEmpleados.Add(idEmpleado.Value);
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: " + idsEmpleados.Count;

            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }


        public async Task<IActionResult> SessionEmpleados(int? idEmpleado)
        {
            if(idEmpleado != null)
            {
                Empleado empleado = await this.repo.FindEmpleadoAsync(idEmpleado.Value);
                //EN SESSION TENDREMOS UN CONJUNTO DE EMPLEADOS
                List<Empleado> empleadosList;
                //DEBEMOS PREGUNTAR SI TENEMOS EL CONJUNTO DE EMPLEADOS ALMACENADOS EN SESSION
                if(HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS") != null)
                {
                    //RECCUPERAMOS LOS EMPLEADOS QUE TENGAMOS EN SESSION
                    empleadosList = HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS");
                }
                else
                {
                    //SI NO EXISTE, INSTACIAMOS LA COLECCION
                    empleadosList = new List<Empleado>();
                }
                //ALMACENAMOS EL EMPLEADO DENTRO DE NUESTRA COLLECCION
                empleadosList.Add(empleado);
                HttpContext.Session.SetObject("EMPLEADOS", empleadosList);
                ViewData["MENSAJE"] = "Empleado" + empleado.Apellido + " almacenado correctamente";

            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult EmpleadosAlmacenados()
        {
            return View();
        }

        public async Task<IActionResult> SessionSalarios(int? salario)
        {
            if(salario != null)
            {
                //NECESITAMOS ALMACENAR EL SALARIO DEL EMPLEADO Y LA SUMA TOTAL DE SALARIOS QUE TENGAMOS
                int sumaSalarial = 0;
                //PREGUNTAREMOS SI YA TENEMOS LA SUMA ALAMACENADA EN SESSION
                if(HttpContext.Session.GetString("SUMASALARIAL") != null)
                {
                    ///SI YA EXISTE LAS SUMA SALARRIAL RECUPERAMOS SU VALOR
                    sumaSalarial = HttpContext.Session.GetObject<int>("SUMASALARIAL");
                }
                //REALIZAMOS LA SUMA
                sumaSalarial += salario.Value;
                //ALMACENAMOS EL NUEVO VALOR DE SUMA SALARIAL DENTRI DE SESION
                HttpContext.Session.SetObject("SUMASALARIAL", sumaSalarial);
                ViewData["MENSAJE"] = "Salario almacenado: " + salario.Value;
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult SumaSalarial()
        {
            return View();
        }
    }
}
