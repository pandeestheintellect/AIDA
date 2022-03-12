export interface DocumentModel {
    code: string;
    name: string;
    filePath: string;
    fileName: string;
    effectiveDate: Date;
    versionNo: string;
    status: string;
    serviceName: string;
}
export interface DocumentFieldModel { 
    code: string;
    keyword: string;
    label: string;
    control: string;
    nature:string;
    isRequired:number;
}