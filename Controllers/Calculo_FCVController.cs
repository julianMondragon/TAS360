using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Models.ViewModel;
using static System.Math;

namespace TAS360.Controllers
{
    public class Calculo_FCVController : Controller
    {
        /// <summary>
		/// GET: Calculo_FCV
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
        public ActionResult Index(FCV_ViewModel model)
        {
            if(model == null)
            {
                FCV_ViewModel FCV = new FCV_ViewModel();
                return View(FCV);
            }
            return View(model);
           
        }

        public ActionResult CalculoFCV(FCV_ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            try
            {
				  
				model.dens = Redondeo05((float)Math.Round(model.dens, 1));
				model.temp = (float) Math.Round(Redondeo005(model.temp), 2);
				model.factor = FCV(model.dens, model.temp);				
				if(model.volnat != 0)
					model.volcor = Math.Round(model.volnat * model.factor);

			}
			catch(Exception ex)
            {
				//ViewBag.warning = ex.Message;
            }
			

            return View("Index", model);
        }

		/// <summary>
		/// Calculo del coeficiente de expansion termica
		/// </summary>
		/// <param name="ar_densidad"></param>
		/// <returns></returns>
		double CoefExpTerm(double ar_densidad)
		{
			double K0 = 0.0, K1 = 0.0, A = 0.0, B = 0.0, la_alpha;
			int tipo = 1;

			if ((ar_densidad >= 653.0) && (ar_densidad < 770.0))
			{
				/*VMAG. tipo = GASOLINA;*/
				tipo = 1;
			}

			else if ((ar_densidad >= 770.0) && (ar_densidad < 788.0))
			{
				/*VMAG. tipo = GASOLINA_TURBOSINA;*/
				tipo = 2;
			}
			else if ((ar_densidad >= 788.0) && (ar_densidad < 839.0))
			{
				/*VMAG. tipo = TURBOSINA_KEROSINA;*/
				tipo = 3;
			}
			else if ((ar_densidad >= 839.0) && (ar_densidad < 1075.0))
			{
				/*VMAG. tipo = COMBUSTOLEOS; */
				tipo = 4;
			}
			

			switch (tipo)
			{
				case 1:

					K0 = 346.4228;
					K1 = 0.4388;
					break;

				case 2:
					A = -0.00336312;
					B = 2680.3206;
					break;

				case 3:
					K0 = 594.5418;
					K1 = 0;
					break;

				case 4:
					K0 = 186.9696;
					K1 = 0.4862;
					break;

					/*case PETROLEO_CRUDO :
						 K0 = 613.9723;
						 K1 = 0.0;
						 break;

					case ACEITE_LUBRICANTE :
						 K0 = 0.0;
						 K1 = 0.6278;
						 break;*/

			}

			if (tipo == 2)
			{
				la_alpha = A + (B / ar_densidad / ar_densidad);
			}
			else
			{
				la_alpha = (K0 / ar_densidad / ar_densidad + K1 / ar_densidad);
			}

			return (la_alpha);

		}

		/// <summary>
		/// Calculo de FCV a 20 a partir de la densidad corregida y de
		/// la temperatura de almacenamiento.
		/// </summary>
		/// <param name="ar_densidad"></param>
		/// <param name="ar_temp_alm"></param>
		/// <returns></returns>
		public double FCV(float ar_densidad, float ar_temp_alm)
		{
			double la_densidad_ajustada;
			double la_temp_ajustada;
			double la_inc_temp;
			double la_densidad_inicial;
			double la_densidad_final;
			double la_alpha;
			double la_fcv20;
			double la_fcv15;
			double la_truncado1;
			double la_truncado2;
			double decimal_analizar;
			double la_inc_temp_20;
			double la_dif_densidad, la_densidad_final_ajustada;

            //la_densidad_ajustada = Redondeo05(ar_densidad);
            //la_temp_ajustada = Redondeo005(ar_temp_alm);
            la_densidad_ajustada = ar_densidad;
            la_temp_ajustada = ar_temp_alm;

            la_inc_temp_20 = 20 - 15;
			la_densidad_inicial = la_densidad_ajustada;

			la_alpha = CoefExpTerm(la_densidad_ajustada);

			la_fcv15 = (float)(Exp(-la_alpha * la_inc_temp_20 - 0.8 * la_alpha * la_alpha * la_inc_temp_20 * la_inc_temp_20));

			la_densidad_final = la_densidad_ajustada / la_fcv15;

			while (la_densidad_inicial != la_densidad_final)
			{

				la_densidad_inicial = la_densidad_final;
				la_alpha = CoefExpTerm(la_densidad_inicial);

				la_fcv15 = (float)(Exp(-la_alpha * la_inc_temp_20 - 0.8 * la_alpha * la_alpha * la_inc_temp_20 * la_inc_temp_20));

				la_densidad_final = la_densidad_ajustada / la_fcv15;

				/* Se comprueba iguadad hasta el quinto decimal */
				la_dif_densidad = la_densidad_inicial - la_densidad_final;

				if (la_dif_densidad <= 0.00001)
				{

					break;
				}

				/* Truncamos el quinto decimal */
				la_densidad_final = (float)(la_densidad_final * 10000.0);
				//la_densidad_final = (float)(la_densidad_final);
				la_densidad_final = (float)(la_densidad_final / 10000.0);

			}

			la_truncado1 = (la_densidad_final * 100.0);
			la_truncado2 = (la_densidad_final * 10.0);

			decimal_analizar = la_truncado1 - (la_truncado2 * 10);

			if ((decimal_analizar >= 0) && (decimal_analizar < 5))
			{
				la_densidad_final_ajustada = (la_truncado2 / 10.0);
			}
			else
			{
				la_densidad_final_ajustada = (la_truncado2 / 10.0 + 0.1);
			}



			/******* PASO 4 *********************/
			la_inc_temp = la_temp_ajustada - 20;


			/********* PASO 5 ***************/

			la_alpha = CoefExpTerm(la_densidad_final_ajustada);

			/* Calculo fcv a 20º */
			la_fcv20 = Exp(-la_alpha * la_inc_temp - 8.0 * la_alpha * la_alpha * la_inc_temp - 0.8 * la_alpha * la_alpha * la_inc_temp * la_inc_temp);


			/* Redondeo del sexto decimal para dejarlo igual que el programa Pemex */
			la_truncado1 = (long)((la_fcv20) * 1000000.0);
			la_truncado2 = (long)((la_fcv20) * 100000.0);

			decimal_analizar = la_truncado1 - (la_truncado2 * 10);

			if ((decimal_analizar >= 0) && (decimal_analizar < 5))
			{
				la_fcv20 = la_truncado2 / 100000.0;
			}
			else
			{
				la_fcv20 = la_truncado2 / 100000.0 +0.00001;
			}

			la_fcv20 = Round(la_fcv20,6);

			return (la_fcv20);


		}

		/// <summary>
		///  Redondeo del primer decimal
		///  X.0, X.1, X.2 -> X.0
		///	 X.3, X.4, X.5, X.6, X.7 -> X.5
		///	 X.8, X.9 ->(X+1).0
		/// </summary>
		/// <param name="valor"></param>
		/// <returns></returns>
		float Redondeo05(float valor)
		{
			
            string[] x = valor.ToString().Split('.');
			if (x.Length <= 1)
				return valor;
            int y = int.Parse(x[1].Substring(0,1));
			valor = (float)Math.Floor(valor);
			double z = (y % 10) * 0.1;
			valor = valor + (float)z;
			if (y > 0)
            {
				switch (y)
                {
					case 1:
						valor = valor - (float)0.1;
						break;
					case 2:
						valor = valor - (float)0.2;						
						break ;
					case 3:
						valor = valor + (float)0.2;						
						break;
					case 4:
						valor = valor + (float)0.1;						
						break;					
					case 6:
						valor = valor - (float)0.1;
						break;
					case 7:
						valor = valor - (float)0.2;
						break;
					case 8:
						valor = valor + (float)0.2;
						break;
					case 9:
						valor = valor + (float)0.1;						
						break;
					default:
						return valor;
				}
            }            
			return valor;
            #region Codigo c
            //double truncado1 = 0, truncado2 = 0, aux = 0/*, truncado3*/;
            ///*	int decimal_analizar;*/
            //double analizar;
            //aux = valor * 10;
            //truncado2 = Math.Floor(valor);
            //analizar = valor - (truncado2);

            //if ((analizar >= 0.0) && (analizar < 0.299))
            //{
            //    return ((float)truncado2);
            //}
            //else if ((analizar >= 0.3001) && (analizar < 0.799))
            //{
            //    return ((float)(truncado2 + 0.5));
            //}
            //else if ((analizar >= 0.299) && (analizar < 0.3001))
            //{
            //    return ((float)(truncado2 + 0.5));
            //}
            //else if ((analizar >= 0.799) && (analizar < 0.8001))
            //{
            //    return ((float)truncado2 + 1);
            //}

            //else
            //    return ((float)truncado2 + 1);
            #endregion

        }
        /// <summary>
        /// Redondeo del segundo decimal
        /// X.Y0, X.Y1, X.Y2 -> X.Y0
        /// X.Y3, X.Y4, X.Y5, X.Y6, X.Y7 -> X.Y5
        /// X.Y8, X.Y9 ->X.(Y+1).0
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        double Redondeo005(double valor)
		{

			valor = Math.Round(valor, 2);
			string[] x = valor.ToString().Split('.');
			if (x.Length <= 1)
				return valor;
            if (x[1].Length <= 1)
            {
				//valor = Redondeo05((float)valor);
				return valor;
            }
			int w = int.Parse(x[1].Substring(0, 1));
			int y = int.Parse(x[1].Substring(1, 1));
			valor = (float)Math.Floor(valor);
			double z = (w % 10) * 0.1;
			valor = valor + (float)z;
			if (y > 0)
			{
				switch (y)
				{
					case 3:
						valor = valor + (float)0.05;
						break;
					case 4:
						valor = valor + (float)0.05;
						break;
					case 5:
						valor = valor + (float)0.05;
						break;
					case 6:
						valor = valor + (float)0.05;
						break;
					case 7:
						valor = valor + (float)0.05;
						break;
					case 8:
						valor = valor + (float)0.1;
						break;
					case 9:
						valor = valor + (float)0.1;
						break;
					default:
						return valor;
				}
			}
			return valor;
			#region Codigo C
			//double truncado1 = 0, truncado2 = 0, aux = 0;			
			//double analizar;
			//double y, i;

			//aux = valor * 10;
			//truncado2 = Floor(valor);

			//truncado1 = Math.Ceiling(aux);

			//analizar = valor - (truncado2);

			////y = modf(analizar * 10.0, &i);
			//y = Truncate(analizar * 10);
			//analizar = y;

			//valor = (float)Floor(valor * 10.000);

			//valor = (float)(valor * 10.0);

			///* Manejamos intervalos */
			//if ((analizar >= 0.0) && (analizar < 0.299))
			//{
			//	return ((float)((double)valor / 100.0));
			//}
			//else if ((analizar >= 0.3001) && (analizar < 0.799))
			//{
			//	return ((float)((double)(valor + 5) / 100.0));
			//}
			//else if ((analizar >= 0.299) && (analizar < 0.3001))
			//{
			//	return ((float)((double)(valor + 5) / 100.0));
			//}
			//else if ((analizar >= 0.799) && (analizar < 0.8001))
			//{
			//	return ((float)((double)(valor + 10) / 100.0));
			//}

			//else
			//	return ((float)((double)(valor + 10) / 100.0));
			#endregion


		}
    }
}