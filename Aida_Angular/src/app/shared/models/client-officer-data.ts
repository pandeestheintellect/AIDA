export interface ClientOfficerModel {
    officerId: number;
    businessProfileId: number;
    name: string;
    address: string;
    nationality:string ;
    birthDate: Date;
    birthCountry:string;
    position: string;
    userRole: string;
    mobile: string;
    email:string;

    nricNo:string;
    nricIssueDate:Date;
    finNo:string;
    finIssueDate:Date;
    finExpiryDate:Date;
    passportNo:string;
    passportIssueDate:Date;
    passportIssuePlace:string;
    passportExpiryDate:Date;

    sex:string; 
    phone:string; 
    birthPlace:string; 
    joinDate:Date; 
    passportIssueCountry:string; 
    numberOfShares:number;

    aliasName:string; 
    residentialStatus:string; 
    race:string; 
    passType:string;  

    myInfoRequestId:number;

}

