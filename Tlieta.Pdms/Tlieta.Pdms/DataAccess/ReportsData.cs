﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace Tlieta.Pdms.DataAccess
{
    public class ReportData
    {
        public string Value { get; set; }
        public int Count { get; set; }
    }

    public class ReportsData
    {
        PDMSDataEntities entities = new PDMSDataEntities();

        public List<ReportData> SurgeriesByHospital()
        {
            List<ReportData> data = new List<ReportData>();
            var report = entities.Surgeries.GroupBy(s => s.HospitalId).Select(s =>
                new
                {
                    Hospital = entities.Hospitals.Where(h => h.HospitalId == s.Key).Select(h => h.HospitalName).FirstOrDefault(),
                    SurgeryCount = s.Count()
                }).ToList();
            foreach (var r in report)
            {
                data.Add(new ReportData() { Value = r.Hospital, Count = r.SurgeryCount });
            }
            return data;
        }

        public List<ReportData> OperationsCount()
        {
            List<ReportData> data = new List<ReportData>();
            var report = entities.Surgeries.GroupBy(s => s.OperationId).Select(s =>
                new
                {
                    Operation = entities.Operations.Where(o => o.OperationId == s.Key).Select(o => o.OperationName).FirstOrDefault(),
                    SurgeryCount = s.Count()
                });
            foreach (var r in report)
            {
                data.Add(new ReportData() { Value = r.Operation, Count = r.SurgeryCount });
            }
            return data;
        }

        public List<ReportData> MonthlyOperations(int operation, int year)
        {
            List<ReportData> data = new List<ReportData>();
            List<Surgery> surgeries = entities.Surgeries.Where(s => s.SurgeryDate.Value.Year == year).ToList();
            if (operation > 0)
            {
                surgeries = surgeries.Where(s => s.OperationId == operation).ToList();
            }

            data.Add(new ReportData() { Value = "Jan", Count = surgeries.Where(t => t.SurgeryDate.Value.Month == 1).Count() });
            data.Add(new ReportData() { Value = "Feb", Count = surgeries.Where(t => t.SurgeryDate.Value.Month == 2).Count() });
            data.Add(new ReportData() { Value = "Mar", Count = surgeries.Where(t => t.SurgeryDate.Value.Month == 3).Count() });
            data.Add(new ReportData() { Value = "Apr", Count = surgeries.Where(t => t.SurgeryDate.Value.Month == 4).Count() });
            data.Add(new ReportData() { Value = "May", Count = surgeries.Where(t => t.SurgeryDate.Value.Month == 5).Count() });
            data.Add(new ReportData() { Value = "Jun", Count = surgeries.Where(t => t.SurgeryDate.Value.Month == 6).Count() });
            data.Add(new ReportData() { Value = "Jul", Count = surgeries.Where(t => t.SurgeryDate.Value.Month == 7).Count() });
            data.Add(new ReportData() { Value = "Aug", Count = surgeries.Where(t => t.SurgeryDate.Value.Month == 8).Count() });
            data.Add(new ReportData() { Value = "Sep", Count = surgeries.Where(t => t.SurgeryDate.Value.Month == 9).Count() });
            data.Add(new ReportData() { Value = "Oct", Count = surgeries.Where(t => t.SurgeryDate.Value.Month == 10).Count() });
            data.Add(new ReportData() { Value = "Nov", Count = surgeries.Where(t => t.SurgeryDate.Value.Month == 11).Count() });
            data.Add(new ReportData() { Value = "Dec", Count = surgeries.Where(t => t.SurgeryDate.Value.Month == 12).Count() });
            return data;
        }

        public Object GetLogBook(int operation, int hospital)
        {
            List<Surgery> surgeries = entities.Surgeries.Where(s => s.OperationId == operation).ToList();
            if (hospital > 0)
            {
                surgeries = surgeries.Where(s => s.HospitalId == hospital).ToList();
            }
            var report = from s in surgeries
                         join p in entities.Patients.ToList() on s.PatientId equals p.PatientId
                         select new
                         {
                             Id = p.PatientId,
                             Name = p.FirstName,
                             Age = p.Age,
                             Gender = p.Gender,
                             IPNumber = s.IPNumber,
                             SurgeryDate = s.SurgeryDate.Value.ToShortDateString(),
                             Operation = entities.Operations.Where(o => o.OperationId == s.OperationId).Select(o => o.OperationName).FirstOrDefault(),
                             Clinic = entities.Hospitals.Where(o => o.HospitalId == s.HospitalId).Select(o => o.HospitalName).FirstOrDefault()
                         };
            return report.ToList();
        }
    }
}
