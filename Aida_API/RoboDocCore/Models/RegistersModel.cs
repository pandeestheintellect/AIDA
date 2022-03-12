using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocCore.Models
{
	public class ApplicationsAllotmentsModel
	{
		public int Id { get; set; }
		public string ApplicationDate { get; set; }
		public string AllotmentDate { get; set; }
		public string AllotmentNo { get; set; }
		public int Applied { get; set; }
		public int Allotted { get; set; }
		public decimal Price { get; set; }

		public decimal AmountCalled { get; set; }
		public decimal AmountPaid { get; set; }
		public string Consideration { get; set; }
		public string CertificateNo { get; set; }
		public string FolioNo { get; set; }
		public string IdentityType { get; set; }

		public int BusinessProfileId { get; set; }
		public string BusinessProfileName { get; set; }
		public int OfficerId { get; set; }
		public string OfficerName{ get; set; }
		public string OfficerBirthDate { get; set; }
		public string OfficerAddress { get; set; }
		public string OfficerIdentity { get; set; }
	}

	public class MembersRegisterModel
	{
		public int Id { get; set; }
		public string EntryDate { get; set; }
		public string AllotmentDate { get; set; }
		public string AllotmentNo { get; set; }
		public string CertificateNo { get; set; }
		public decimal Price { get; set; }
		public string SrFrom { get; set; }
		public string SrTo { get; set; }

		public int Acquisitions { get; set; }
		public int Disposals { get; set; }
		public int Balance { get; set; }
		public string ClassOfShare { get; set; }
		public string IdentityType { get; set; }

		public int BusinessProfileId { get; set; }
		public string BusinessProfileName { get; set; }
		public int OfficerId { get; set; }
		public string OfficerName { get; set; }
		public string OfficerBirthDate { get; set; }
		public string OfficerAddress { get; set; }
		public string OfficerIdentity{ get; set; }

	}


	public class TransfersRegisterModel
	{
		public int Id { get; set; }
		public string TransferNo { get; set; }
		public string TransferDate { get; set; }

		public int TransferorId { get; set; }
		public string TransferorFolioNo { get; set; }
		public string TransferorNoShares { get; set; }
		public string TransferorCertificateNo { get; set; }
		public string BalanceNoShares { get; set; }
		public string BalanceCertificateNo { get; set; }
		public string TransferorName { get; set; }
		public string TransferorAddress { get; set; }

		public int TransfereeId { get; set; }
		public string TransfereeFolioNo { get; set; }
		public string TransfereeNoShares { get; set; }
		public string TransfereeCertificateNo { get; set; }
		public string TransfereeName { get; set; }
		public string TransfereeAddress { get; set; }

		public int BusinessProfileId { get; set; }
		public string BusinessProfileName { get; set; }
	}

	public class OfficersRegisterModel
	{
		public int Id { get; set; }
		public string EntryDate { get; set; }

		public string StartDate { get; set; }
		public string CessationDate { get; set; }
		public string Remarks1 { get; set; }
		public string Remarks2 { get; set; }

		public int OfficerId { get; set; }
		public string OfficerName { get; set; }
		public string OfficerBirthDate { get; set; }
		public string OfficerAddress { get; set; }
		public string OfficerIdentity { get; set; }
		public string IdentityType { get; set; }
		public int BusinessProfileId { get; set; }
		public string BusinessProfileName { get; set; }
	}


	public class AuditorsRegisterModel
	{
		public int Id { get; set; }
		public string EntryDate { get; set; }

		public string Name { get; set; }
		public string Address { get; set; }
		public string RegistrationNo { get; set; }

		public string StartDate { get; set; }
		public string CessationDate { get; set; }
		public string Remarks1 { get; set; }
		public string Remarks2 { get; set; }
		public string Remarks3 { get; set; }

		public int BusinessProfileId { get; set; }
		public string BusinessProfileName { get; set; }
	}

	public class MortgagesChargesModel
	{
		public int Id { get; set; }
		public string EntryDate { get; set; }

		public string Name { get; set; }
		public string Address { get; set; }
		public string ShortDescription { get; set; }

		public decimal Amount { get; set; }
		public string RegistrarDate { get; set; }
		public string NoOfCertificate { get; set; }
		public string DischargeDate { get; set; }
		public string Remarks { get; set; }

		public int BusinessProfileId { get; set; }
		public string BusinessProfileName { get; set; }
	}

	public class ControllersCorporateRegisterModel
	{
		public int Id { get; set; }
		public string EntryDate { get; set; }

		public string Name { get; set; }
		public string Address { get; set; }
		public string RegistrationNo { get; set; }

		public string StartDate { get; set; }
		public string CessationDate { get; set; }
		public string LegalForm { get; set; }
		public string Jurisdiction { get; set; }
		public string IdentityNo { get; set; }
		public string Statue { get; set; }
		public string RegisterName { get; set; }


		public int BusinessProfileId { get; set; }
		public string BusinessProfileName { get; set; }
	}

	public class NomineeRegisterModel
	{
		public int Id { get; set; }
		public string EntryDate { get; set; }
		public string Nature { get; set; }
		public int OfficerId { get; set; }
		public string OfficerName { get; set; }
		public string NominatorId { get; set; }
		public string NominatorName { get; set; }

		public int BusinessProfileId { get; set; }
		public string BusinessProfileName { get; set; }
	}
	public class NomineeRegisterPDFModel: NomineeRegisterModel
	{
		public string Remarks1 { get; set; }
		public string Remarks2 { get; set; }
		public string Remarks3 { get; set; }
		public string Remarks4 { get; set; }
		public string Remarks5 { get; set; }
		public string Remarks6 { get; set; }
		public string Remarks7 { get; set; }
	}
}


