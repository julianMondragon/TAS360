﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAS360.Models;

namespace TAS360.Models.ViewModel
{
    public class PendientesViewModel
    {
        public PendientesViewModel() 
        {
            currentList = new List<CurrentList>();
        }

        public int id { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        [MaxLength(length: 250, ErrorMessage = "La Descripcion debe contener menos de 250 caracteres")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Clasificación")]
        public int id_Clasificacion { get; set; }

        [Display(Name = "Observación")]
        [MaxLength(length: 3000, ErrorMessage = "La Observacion debe contener menos de 3000 caracteres")]
        public string Observacion { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Rango 0 a 100")]
        [Display(Name = "% de avance")]
        public double Avance { get; set; }

        [Required]
        [Display(Name = "Prioridad")]
        public int id_Prioridad { get; set; }

        [Required]
        [Display(Name = "Acitividades pendientes por el fabricante (SUSESS)")]
        [MaxLength(length: 3000, ErrorMessage = "Las Acitividades pendientes por el fabricante (SUSESS) debe contener menos de 3000 caracteres")]
        public string Actividades_Pend_Susess { get; set; }

        [Required]
        [Display(Name = "Responsable")]
        [MaxLength(length: 100, ErrorMessage = "El campo Responsable debe contener menos de 100 caracteres")]
        public string Responsable { get; set; }

        [Required]
        [Display(Name = "Fecha de creacion")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Fecha Compromiso de entrega")]
        public DateTime Fecha_Compromiso { get; set; }

        
        public int id_User { get; set; }

        [Display(Name = "Ticket relacionado")]
        public int id_Ticket { get; set; }

        [Required]
        [Display(Name = "Terminal de Almacenamiento y Reparto")]
        public int id_Terminal { get; set; }

        [Required]
        [Display(Name = "Subsistema relacionado")]
        public int id_Subsistema { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int id_status { get; set; }

        [Required]
        [Display(Name = "Version donde fue encontrado")]
        public string version_where_the_Pending_was_found { get; set; }

        
        [Display(Name = "Version donde fue solventado")]
        public string version_where_the_Pending_is_fixed { get; set; }

        [Display(Name = "Es PAS")]
        public bool is_PAS { get; set; }

        [Display(Name = "Es PAF")]
        public bool is_PAF { get; set; }

        public Clasificacion_Pendiente Clasificacion_Pendiente { get; set; }        
        public User User { get; set; }      
        public Subsistema Subsistema { get; set; }
        public Terminal Terminal { get; set; }
        public Prioridad_de_Pendiente Prioridad { get; set; }
        public List<Pendiente_Record_Status> Record_Status { get; set;  }

        public bool is_selected { get; set; }

        public List<CurrentList> currentList { get; set; }


    }

    public class CurrentList
    {
        public int id { get; set; }
        public bool is_selected { get;  set; }
    }
}