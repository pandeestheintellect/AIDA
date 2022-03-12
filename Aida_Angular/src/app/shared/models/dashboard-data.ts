export interface StatusCountModel
{
    periods:number
    counts:number
    displayName: string ;
    status: string ;
}

export interface ServiceStatusCountModel
{
    monthlyCounts:number
    yearlyCounts:number
    name: string ;
    serviceCode: string ;
}