export interface ClientProfileModel {
    id: number;
    name: string;
    formerName:string;
    uen: string;
    incorpDate: Date;
    address1: string;
    address2:string;
    city: string;
    country: string;
    pincode: string;
    mobile: string;
    email:string;
    industryType: string ;
    status: string ;
    statusDate: Date ;
    issuedCapital:number;
    issuedShares:number;
    issuedCurrency:string;
    issuedShareType:string;

    paidupCapital:number;
    paidupShares:number;
    paidupCurrency:string;
    paidupShareType:string; 

    tradingName:string;
    phone:string;
    nature:string;
    clientType:string;
}

export interface ClientProfileActivity {
    businessProfileId: number;
    name: string;
    description: string;
}


export interface EntityShareholderModel {
    id: number;
    name: string;
    formerName:string;
    tradingName:string;
    uen: string;
    address: string;
    country: string;
    incorpDate: Date;
    phone:string;
    nature:string;
    status: string ;
    businessProfileId: number;
    representativeId:number;
    representativeName:string;

}
