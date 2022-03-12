export interface ServicesDefinitionModel
{
    code: string;
    name: string;
    remarks: string;
    hasOptionalDocument:string
}

export interface  ServicesSOPModel
{
    stepId: number;
    stepNo: number;
    code: string;
    executor: string;
    remarks: string;
    dependencyStepNo: number;
    documentName: string;
    versionNo: string;
    filePath:string;
}

export interface ServiceSOPDetail
{
    action: string;
    serviceCode: string;
    executor: string;
}

export interface ServiceRegistrationDisplayModel
{
    created:string;
    serviceName: string;
    businessProfileName: string;
    officerName: string;
    documentName: string;
    executor: string;
    remarks: string;
    status:string;
    serviceBusinessId:number;
    officerId: number;
    officerStepId:number;
    generatedFileName: string;
    downloadedFileName: string;
    
}

export interface ServiceRegistrationViewModel
{
    created:string;
    serviceName: string;
    businessProfileId: number;
    officerName: string;
    status:string;
    serviceBusinessId:number;
    serviceCode: string;
    
}


export interface  ServiceRegistrationClientDisplayModel
{
    id:string;
    businessProfileName: string;
    status:string;
    uen:string;
    generatedDate:string;
    serviceBusinessId:number;
    
}

export interface ServiceRegistraionDocument
{
    id: number;
    name: string;
    email: string;
    mobile: string;
    message: string;
    documentNames: string;
    filePaths: string;
    status: string;
    documentType:string;
}

export interface UploadedDocumentModel
{
    documentId: number;
    created: string;
    serviceName: string;
    officerName: string;
    serviceCode: string;
    documentType: string;
    actualFileName: string;
    filePath: string;
    businessProfileId: number;
}

export interface SendFormModel
{
    businessProfileId: number;
    name: string;
    email: string;
    mobile: string;
    message: string;
    documentType:string;
    documentName?:string;
    documentNames: ServicesSOPModel[];
}