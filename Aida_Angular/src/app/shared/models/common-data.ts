export interface DropDownModel {
  value: string;
  text: string;
}

export interface ResponseModel {
  isSuccess: boolean;
  message: string;
}

export interface DialogDataModel {
  action: string;
  data: any;
}
export interface DialogOfficersDataModel {
  action: string;
  data: any;
  officers: [];
  identityTypes: [];
}

export interface DocPreviewModel
{
  serviceBusinessId:number;
  officerStepId:number;
  fileName:string;
}