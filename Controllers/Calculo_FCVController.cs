﻿using System;
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
        // GET: Calculo_FCV
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
				model.factor = FCV(model.dens, model.temp);
				model.volcor = model.volnat * model.factor;

			}
			catch(Exception ex)
            {

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
			/*************************** VARIABLES AUTOMATICAS ***************************/
			double K0 = 0.0, K1 = 0.0, A = 0.0, B = 0.0, la_alpha;
			int tipo = 1;

			/************************ VARIABLES ESTATICAS LOCALES ************************/


			/********************************** CODIGO ***********************************/


			/*if ( ar_tipoprod == 0)
			{*/

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
			/*}
			else if (ar_tipoprod == 1)
			{
				tipo = PETROLEO_CRUDO;
			}
			else if (ar_tipoprod == 2)
			{
				tipo = ACEITE_LUBRICANTE;
			}*/

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
			

			double la_densidad_ajustada, la_temp_ajustada, la_inc_temp, la_densidad_inicial, la_densidad_final;
			double la_alpha, la_fcv20, la_fcv15;

			double la_truncado1, la_truncado2;

			double decimal_analizar;

			double la_inc_temp_20;

			double la_dif_densidad, la_densidad_final_ajustada;

			/************************ VARIABLES ESTATICAS LOCALES ************************/


			/********************************** CODIGO ***********************************/

			/* Redondeos iniciales */
			la_densidad_ajustada = Redondeo05(ar_densidad);
			la_temp_ajustada = Redondeo005(ar_temp_alm);


			la_inc_temp_20 = 20 - 15;
			la_densidad_inicial = la_densidad_ajustada;
			la_alpha = CoefExpTerm(la_densidad_ajustada/*,ar_tipoprod*/);

			la_fcv15 = (float)(Exp(-la_alpha * la_inc_temp_20 - 0.8 * la_alpha * la_alpha * la_inc_temp_20 * la_inc_temp_20));

			la_densidad_final = la_densidad_ajustada / la_fcv15;

			while (la_densidad_inicial != la_densidad_final)
			{

				la_densidad_inicial = la_densidad_final;
				la_alpha = CoefExpTerm(la_densidad_inicial/*,ar_tipoprod*/);

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
				la_densidad_final = (float)(la_densidad_final);
				la_densidad_final = (float)(la_densidad_final / 10000.0);

			}

			la_truncado1 = (float)((long)((la_densidad_final) * 100.0));
			la_truncado2 = (float)((long)((la_densidad_final) * 10.0));

			decimal_analizar = la_truncado1 - (la_truncado2 * 10);

			if ((decimal_analizar >= 0) && (decimal_analizar < 5))
			{
				la_densidad_final_ajustada = (float)(la_truncado2 / 10.0);
			}
			else
			{
				la_densidad_final_ajustada = (float)(la_truncado2 / 10.0 + 0.1);
			}



			/******* PASO 4 *********************/
			la_inc_temp = la_temp_ajustada - 20;


			/********* PASO 5 ***************/

			la_alpha = CoefExpTerm(la_densidad_final_ajustada/*,ar_tipoprod*/);

			/* Calculo fcv a 20º */
			la_fcv20 = (float)Exp(-la_alpha * la_inc_temp - 8.0 * la_alpha * la_alpha * la_inc_temp - 0.8 * la_alpha * la_alpha * la_inc_temp * la_inc_temp);


			/* Redondeo del sexto decimal para dejarlo igual que el programa Pemex */
			la_truncado1 = (long)((la_fcv20) * 1000000.0);
			la_truncado2 = (long)((la_fcv20) * 100000.0);

			decimal_analizar = la_truncado1 - (la_truncado2 * 10);

			if ((decimal_analizar >= 0) && (decimal_analizar < 5))
			{
				la_fcv20 = (float)la_truncado2 / (float)100000.0;
			}
			else
			{
				la_fcv20 = (float)la_truncado2 / (float)100000.0 + (float)0.00001;
			}


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
			
			double truncado1 = 0, truncado2 = 0, aux = 0/*, truncado3*/;
			/*	int decimal_analizar;*/
			double analizar;
			aux = valor * 10;
			truncado2 = Math.Floor(valor);
			analizar = valor - (truncado2);

			if ((analizar >= 0.0) && (analizar < 0.299))
			{
				return ((float)truncado2);
			}
			else if ((analizar >= 0.3001) && (analizar < 0.799))
			{
				return ((float)(truncado2 + 0.5));
			}
			else if ((analizar >= 0.299) && (analizar < 0.3001))
			{
				return ((float)(truncado2 + 0.5));
			}
			else if ((analizar >= 0.799) && (analizar < 0.8001))
			{
				return ((float)truncado2 + 1);
			}

			else
				return ((float)truncado2 + 1);


		}
		/// <summary>
		/// Redondeo del segundo decimal
		/// X.Y0, X.Y1, X.Y2 -> X.Y0
		/// X.Y3, X.Y4, X.Y5, X.Y6, X.Y7 -> X.Y5
		/// X.Y8, X.Y9 ->X.(Y+1).0
		/// </summary>
		/// <param name="valor"></param>
		/// <returns></returns>
		float Redondeo005(float valor)
		{

			double truncado1 = 0, truncado2 = 0, aux = 0;			
			double analizar;
			double y, i;
			
			aux = valor * 10;
			truncado2 = Floor(valor);

			truncado1 = Math.Ceiling(aux);

			analizar = valor - (truncado2);

			//y = modf(analizar * 10.0, &i);
			y = Truncate(analizar * 10);
			analizar = y;

			valor = (float)Floor(valor * 10.000);

			valor = (float)(valor * 10.0);

			/* Manejamos intervalos */
			if ((analizar >= 0.0) && (analizar < 0.299))
			{
				return ((float)((double)valor / 100.0));
			}
			else if ((analizar >= 0.3001) && (analizar < 0.799))
			{
				return ((float)((double)(valor + 5) / 100.0));
			}
			else if ((analizar >= 0.299) && (analizar < 0.3001))
			{
				return ((float)((double)(valor + 5) / 100.0));
			}
			else if ((analizar >= 0.799) && (analizar < 0.8001))
			{
				return ((float)((double)(valor + 10) / 100.0));
			}

			else
				return ((float)((double)(valor + 10) / 100.0));



		}
	}
}